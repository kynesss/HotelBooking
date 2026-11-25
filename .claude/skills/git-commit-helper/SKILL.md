---
name: git-commit-helper
description: Generates concise, single-line English commit messages following the Conventional Commits specification based on Git changes.
disable-model-invocation: true
allowed-tools: Bash(git status *) Bash(git diff *) Bash(git log *)
---

# Git Commit Helper

## Objective

Generate clear, concise, and meaningful commit messages in English
following the Conventional Commits specification.

## Workflow

1. Inspect the repository using:
   - `git status --short`
   - `git diff --staged`
   - `git diff`
   - `git log -5 --oneline`
2. Prioritize staged changes when generating the commit message.
   If no changes are staged, analyze unstaged changes and clearly
   indicate that they are not staged.
3. Identify the primary purpose of the changes.
4. Select the appropriate commit type and optional scope.
5. Generate a commit message that accurately describes the changes.
6. If there are unrelated changes, suggest splitting them into
   separate commits.

## Commit Format

```text
<type>(<optional scope>): <description>
```

The commit message is always a single subject line. Never add a body
or footers.

## Commit Types

- feat: Introduce a new feature.
- fix: Fix a bug.
- refactor: Restructure code without changing behavior.
- perf: Improve performance.
- test: Add or update tests.
- docs: Update documentation.
- style: Apply formatting changes without affecting behavior.
- build: Modify build configuration or dependencies.
- ci: Modify CI/CD configuration.
- chore: Perform maintenance or project setup.
- revert: Revert a previous commit.

## Rules

- Always write commit messages in English.
- Use the imperative mood.
- Start the description with a lowercase letter.
- Do not end the description with a period.
- Aim for a subject line of 72 characters or fewer.
- Use a scope when it provides meaningful context.
- Describe the purpose of the changes, not just modified files.
- Never invent changes that are not present in the diff.
- Never include a commit body or description below the subject line.
- Mark breaking changes with `!` after the type or scope.
- Never stage files, create commits, or push changes.

## Output

Return only the recommended single-line commit message in a code block.

Avoid unnecessary explanations unless the changes are ambiguous
or should be split into multiple commits.

## Examples

```text
chore: initialize modular monolith structure
```

```text
feat(users): implement user registration
```

```text
fix(bookings): prevent overlapping reservations
```

```text
refactor(users): extract email validation logic
```

```text
build: update entity framework core dependencies
```

```text
feat(payments): add stripe payment integration
```
