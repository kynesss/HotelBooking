# HotelBooking

## Communication

- Always respond to the developer in Polish.
- Keep code, identifiers, and commit messages in English.

## Project Overview

HotelBooking is a hotel reservation platform built as a modular monolith using Domain-Driven Design, Clean Architecture, and Vertical Slice Architecture.

The project is developed incrementally, with an emphasis on maintainability, modularity, and learning architectural principles.

## Technology Stack

- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Docker and Docker Compose

## Architecture

The solution is organized into three main directories:

- `src/API` — application host and composition root.
- `src/Common` — shared technical abstractions and implementations.
- `src/Modules` — independent business modules.

Each business module contains four projects:

- **Domain** — entities, value objects, domain events, and business rules.
- **Application** — use cases, commands, queries, handlers, and abstractions.
- **Infrastructure** — persistence, external integrations, and module registration.
- **Presentation** — HTTP endpoints and request/response mapping.

## Dependency Rules

- Domain must not depend on Application, Infrastructure, or Presentation.
- Application depends on Domain.
- Presentation depends on Application.
- Infrastructure depends on Application and Presentation.
- The API host references module Infrastructure projects.
- Modules may reference appropriate Common projects.
- Common projects must never depend on business modules.
- Modules must not directly access the internal implementation or persistence layer of other modules.
- Cross-module communication mechanisms must be agreed upon before implementation.

## Coding Guidelines

- Follow existing `.editorconfig` and `Directory.Build.props` settings.
- Use `var` for local variable declarations.
- Prefer clear, descriptive names.
- Keep business logic inside the domain where appropriate.
- Keep endpoints thin.
- Organize application features by use case.
- Use asynchronous APIs for I/O operations.
- Propagate cancellation tokens where applicable.
- Avoid unnecessary abstractions and premature optimization.
- Do not introduce new dependencies or architectural patterns without approval.

## Code Consistency

- Write new code the same way existing code in the project is written. Before adding code, read the closest existing equivalent and follow its structure, naming, and style.
- Follow these established conventions:
  - File-scoped namespaces that match the folder structure.
  - Domain and persistence types grouped by feature folder (e.g. `Users/`); database-wide types in `Database/`.
  - Entities inherit from `Entity`, expose properties with private setters, and record domain events with `Raise`.
  - Each module has its own `DbContext`, schema constant in `Schemas`, and `IEntityTypeConfiguration<T>` classes. Database names use snake_case.
  - Module registration lives in `<Module>Module` in Infrastructure (`Add<Module>Module`), using C# `extension` blocks.
  - Shared registration lives in `AddApplication` and `AddInfrastructure` in the Common projects.
  - Primary constructors for dependency injection.
  - Classes not used outside their project are `internal`; mark classes `sealed` when they are not designed for inheritance.
- If existing code conflicts with these rules or is inconsistent, point it out and ask instead of silently introducing another style.

## Result Pattern

- Use `Result` and `Result<TValue>` from `HotelBooking.Common.Domain` for operations that can fail for expected business or validation reasons.
- Do not throw exceptions for expected failures. Reserve exceptions for unexpected or programming errors.
- Describe failures with `Error` and its factory methods (`Failure`, `NotFound`, `Problem`, `Conflict`); use `ValidationError` for validation failures.
- Use stable, descriptive error codes in the `<Feature>.<Reason>` format (e.g. `Users.NotFound`).
- Check `IsFailure` before accessing `Value`, and propagate errors to the caller.
- Endpoints translate failed results into appropriate HTTP responses based on `ErrorType`.

## HTTP Endpoints

- Use Minimal APIs. Do not add MVC controllers, `AddControllers`, or `MapControllers`.
- Define endpoints in the module's Presentation project, not in `Program.cs`.
- Endpoints only map the request, send a command or query through MediatR, and map the `Result` to a response.

## Development Workflow

- Analyze the existing code before making changes.
- Clarify ambiguous business requirements instead of making assumptions.
- Discuss significant architectural decisions before implementation.
- Keep changes focused on the requested task.
- Do not implement unrelated features or refactor unrelated code.
- Explain important implementation decisions and trade-offs.
- Run relevant builds and tests after implementation.
- Do not create commits or push changes unless explicitly requested.

## Learning Objectives

This project is also a learning environment for the developer.

- Explain non-trivial architectural decisions.
- Present alternatives when meaningful.
- Do not introduce complex patterns without explaining the problem they solve.
- Allow the developer to make architectural and business decisions.
- Prefer incremental implementation over building speculative infrastructure.
