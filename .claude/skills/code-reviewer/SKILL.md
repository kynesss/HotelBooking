---
name: code-reviewer
description: Reviews uncommitted changes (staged, unstaged and untracked) before a commit for clean code, SOLID, coupling and cohesion, Clean Architecture and modular monolith boundaries, DDD, and consistency with existing project conventions, then fixes the findings. Use when the developer asks for a code review, a pre-commit check, or to review the diff.
allowed-tools: Bash(git status *) Bash(git diff *) Bash(git log *) Bash(git ls-files *) Bash(dotnet build *) Bash(dotnet test *) Read Grep Glob Edit Write
---

# Code Reviewer

## Objective

Review the pending changes in the working tree as a strict senior
reviewer, then fix what the review found. The review judges the change
against three references, in this order of priority:

1. The rules in `.claude/CLAUDE.md`.
2. The way equivalent code is already written in this repository.
3. General principles: clean code, SOLID, low coupling, high cohesion,
   Clean Architecture, DDD.

Consistency with the project outranks personal preference. A change that
is "better" in the abstract but introduces a second style is a finding.

## Scope

- Review only what the diff changes or adds. Read surrounding code for
  context, but do not report problems in untouched code unless the change
  makes them worse or depends on them.
- If a pre-existing problem blocks a correct change, mention it
  separately as `pre-existing` and do not fix it without approval.
- Never review generated code: `Migrations/`, `*.Designer.cs`,
  `*ModelSnapshot.cs`, `bin/`, `obj/`.

## Workflow

1. Collect the changes:
   - `git status --porcelain`
   - `git diff --staged`
   - `git diff`
   - `git ls-files --others --exclude-standard` for new files, which do
     not appear in `git diff`; read them in full.
2. If there are no changes, say so and stop.
3. Read `.claude/CLAUDE.md`, `.editorconfig` and `Directory.Build.props`
   if they are not already in context.
4. For every changed file, read the whole file, not only the hunk.
5. For every new type, find and read its closest existing equivalent
   (another handler, endpoint, entity, configuration, module
   registration) and compare structure, naming, and style.
6. Check project references (`*.csproj`) of every project touched by the
   change to verify dependency direction.
7. Run `dotnet build` on the solution. Analyzers run with warnings as
   errors, so every build error or warning is a finding.
8. Evaluate the change against the checklist below.
9. Report the findings (see Output).
10. Fix the findings (see Fixing).
11. Run `dotnet build` again, and `dotnet test` if test projects exist.
    Then re-review the files you edited, because fixes can introduce new
    findings.

## Checklist

### Project consistency

- The new code mirrors the closest existing equivalent: folder, file
  name, type name, member order, constructor style, return style.
- Namespaces are file-scoped and match the folder structure.
- File name matches the type name.
- Feature folders follow the existing layout, e.g.
  `Application/<Feature>/<UseCase>/<UseCase>Command.cs`,
  `...CommandHandler.cs`, `...CommandValidator.cs`, and
  `Presentation/<Feature>/<UseCase>.cs`.
- Naming follows existing suffixes: `Command`, `Query`, `Handler`,
  `Validator`, `Configuration`, `Repository`, `Errors`, `DomainEvent`,
  `Options`, `Module`.
- Dependency injection uses primary constructors.
- Registration lives in `Add<Module>Module` or in the Common
  `AddApplication` / `AddInfrastructure`, written as C# `extension`
  blocks, one per static class.
- No comments, including XML documentation comments.
- `var` for local variables.
- No new NuGet packages, patterns, or abstractions that were not agreed.

### Explicit and restrictive types

- `internal` unless the type is used outside its project.
- `private` for members not used outside their type.
- `sealed` for every class and record not designed for inheritance.
- `static` for classes and methods without instance state, but no
  `static` on lambdas.
- `readonly` fields, `const` for compile-time constants.
- `init` or `private set` instead of public setters; `required` for
  properties that must be set.
- `record` for commands, queries, requests, responses, and DTOs.

### Clean code

- Names describe intent; no abbreviations, no `Manager`, `Helper`,
  `Util`, `Data`, or `Info` names that hide responsibility.
- Methods are short and operate at one level of abstraction.
- No duplicated logic that already exists elsewhere in the repository.
- No dead code, unused parameters, unused usings, or speculative
  members added "for later".
- No magic strings or numbers; use constants or options classes, as the
  existing code does.
- Guard clauses instead of deep nesting.
- Asynchronous APIs for I/O, `CancellationToken` propagated through
  every async call that accepts it.
- No `async void`, `.Result`, `.Wait()`, or `GetAwaiter().GetResult()`.

### SOLID

- **SRP**: each class has one reason to change. A handler handles one
  use case; an endpoint only maps HTTP; an entity guards its own
  invariants.
- **OCP**: new behavior is added by new types (endpoints discovered via
  `IEndpoint`, pipeline behaviors, configurations), not by growing
  `switch` statements or modifying shared code for one feature.
- **LSP**: implementations honor the contract of their abstraction; no
  `NotImplementedException` or narrowed behavior.
- **ISP**: interfaces are small and shaped by their consumer; no
  methods the consumer does not need.
- **DIP**: Application depends on abstractions it owns
  (e.g. `IIdentityProviderService`, `IUnitOfWork`, repositories), and
  Infrastructure implements them.

### Coupling and cohesion

- A type depends on as few other types as it needs; flag constructors
  with many dependencies as a sign of mixed responsibilities.
- No knowledge of concrete infrastructure (EF Core, `HttpClient`,
  Keycloak, Npgsql) leaks into Domain or Application.
- No dependency on another module's Domain, Application,
  Infrastructure, or `DbContext`. Any cross-module communication must
  have been agreed upon first; otherwise it is a finding and a question
  for the developer.
- Everything belonging to one use case lives together in its slice;
  unrelated things are not grouped together just because they share a
  technical type.
- Shared code goes to `src/Common` only if it is purely technical and
  used by more than one module. Common never references a module.

### Clean Architecture and project boundaries

- Domain references nothing but `HotelBooking.Common.Domain`.
- Application references Domain and Common.Application only.
- Presentation references Application and Common.Presentation.
- Infrastructure references Application and Presentation.
- The API host references only module Infrastructure projects and
  contains no endpoints or business logic.
- Public surface between layers is minimal: implementation details of
  Infrastructure are `internal`.

### Domain-Driven Design

- Business rules live in the domain, not in handlers or endpoints.
- Entities inherit from `Entity`, expose private setters, are created
  through factory methods, and record domain events with `Raise`.
- Invariants cannot be bypassed from outside the aggregate.
- Domain events are named in the past tense and carry only what
  consumers need.
- Repositories exist per aggregate root and live behind interfaces in
  Domain.

### Result pattern and errors

- Expected failures return `Result` / `Result<TValue>`; exceptions are
  only for unexpected or programming errors.
- Errors use `Error` factory methods (`Failure`, `NotFound`,
  `Problem`, `Conflict`) or `ValidationError`.
- Error codes follow `<Feature>.<Reason>` and are defined in a
  `<Feature>Errors` class, as existing code does.
- `IsFailure` is checked before `Value`, and errors are propagated.

### HTTP endpoints

- Minimal APIs implementing `IEndpoint` in the module's Presentation
  project; no controllers.
- The endpoint only maps the request, sends it through MediatR
  (`ISender`), and maps the `Result` with `Match` and `ApiResults`.
- The request record is a `private sealed record` inside the endpoint,
  as in existing endpoints.
- Authorization (`AllowAnonymous` or a policy) and `WithTags` are set
  explicitly.

### Persistence

- Each module uses its own `DbContext`, schema from `Schemas`, and
  `IEntityTypeConfiguration<T>` classes.
- Database names are snake_case.
- `SaveChangesAsync` is called through `IUnitOfWork` in the handler, not
  inside repositories.
- Schema changes include a migration; flag a model change without one.

### Security and robustness

- No secrets, connection strings, or credentials in code or in
  committed configuration; they belong in `.env` / user secrets.
- Input is validated by a FluentValidation validator for every command
  that accepts external input.
- Nullable reference warnings are not suppressed with `!` without a
  clear reason.

## Severity

- **critical**: broken architecture boundary, bug, data loss, security
  issue, build failure.
- **major**: SOLID violation, tight coupling, business logic in the
  wrong layer, inconsistency with established project conventions,
  violation of a CLAUDE.md rule.
- **minor**: naming, missing `sealed` / `internal` / `readonly`,
  readability, small duplication.
- **question**: an ambiguous business requirement or an architectural
  decision that belongs to the developer.

## Output

Respond to the developer in Polish. Keep code, identifiers, and error
codes in English.

1. A one-line summary of what the change does.
2. Build result.
3. Findings grouped by severity, most severe first. Each finding has:
   - the location as a clickable link `[File.cs:42](path/File.cs:42)`,
   - what is wrong,
   - why it matters (which rule or principle, and the existing code it
     diverges from, if any),
   - the proposed fix.
4. A short list of what is done well, only if it is genuinely notable.

If there are no findings, say so plainly and do not invent any.

## Fixing

- After reporting, fix every `critical`, `major`, and `minor` finding
  whose fix is unambiguous and local to the change.
- Do not fix `question` findings or anything that requires an
  architectural or business decision. Present the options with
  trade-offs and let the developer decide, as required by CLAUDE.md.
- Do not fix `pre-existing` problems in untouched code without approval.
- Do not widen the scope: no unrelated refactoring, no new features.
- Write each fix the same way the surrounding project code is written.
- After fixing, list in Polish what was changed and what was left for
  the developer, with the reason.
- Never stage files, create commits, or push changes.
