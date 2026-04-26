---
name: commit-message-rules
description: Apply repository commit-message rules. Use when Codex creates, edits, reviews, or suggests Git commit messages, especially when Conventional Commits format or commit type selection is requested.
---

# Commit Message Rules

Use Conventional Commits 1.0.0 for commit messages.

Reference: https://www.conventionalcommits.org/en/v1.0.0/

## Format

Write the first line as:

```text
<type>[optional scope][optional !]: <description>
```

Add an optional body after one blank line, and optional footer lines after one blank line from the body:

```text
<type>[optional scope][optional !]: <description>

[optional body]

[optional footer(s)]
```

## Components

- `type`: required noun that communicates the kind of change.
- `scope`: optional noun in parentheses that names the affected area, such as `interop`, `build`, `docs`, or `input`.
- `!`: optional marker immediately before `:` for a breaking change.
- `description`: required short summary after `: `. Use imperative mood, lowercase after the type unless a proper noun is needed, and no trailing period.
- `body`: optional free-form explanation of what changed and why. Start it one blank line after the description.
- `footer`: optional trailer-style metadata. Use tokens such as `Refs`, `Reviewed-by`, or `BREAKING CHANGE`.

## Breaking Changes

Mark breaking changes with either:

```text
feat(api)!: remove legacy save endpoint
```

or:

```text
feat(api): remove legacy save endpoint

BREAKING CHANGE: legacy save endpoint is no longer available.
```

`BREAKING CHANGE` must be uppercase when used as a footer. `BREAKING-CHANGE` is equivalent when used as a footer token.

## Type Selection

Use these types consistently:

- `feat`: add a user-visible feature or capability. SemVer: minor.
- `fix`: correct a bug. SemVer: patch.
- `perf`: improve runtime performance without changing behavior.
- `refactor`: change code structure without adding features or fixing bugs.
- `docs`: change documentation, comments intended as documentation, or repository guidance.
- `test`: add, update, or remove tests.
- `build`: change build scripts, project files, packaging, dependencies, or generated build configuration.
- `ci`: change continuous integration workflows or automation.
- `style`: formatting-only change with no behavior impact.
- `chore`: maintenance that does not fit the other types and does not affect source, tests, build, docs, or CI in a more specific way.
- `revert`: revert previous commits; include references in the body or footers when useful.

Prefer the most specific type. If one logical change needs multiple types, split it into multiple commits when practical.

## Examples

```text
docs: add agent workflow skills
```

```text
refactor(interop): remove redundant role checks
```

```text
fix(input): ignore hotkeys while local player is busy
```

```text
feat!: require current game interop adapters

BREAKING CHANGE: legacy versioned adapters are no longer loaded.
```
