---
name: pull-request-quality-check
description: Quality-check repository pull requests. Use when creating or updating pull requests.
---

# Pull Request Quality Check

## When to Use

- Use this skill when creating, updating, reviewing, or validating pull request titles or bodies for this repository.

## Title

Check that the title uses a Conventional Commits-style format:

```text
<type>[optional scope][optional !]: <description>
```

Use `commit-message-quality-check` for type selection and breaking-change notation.

Reference: https://www.conventionalcommits.org/en/v1.0.0/

## Required LLM Alert

When the PR was prepared with LLM assistance, check that this GitHub alert appears at the very top of the PR body:

```markdown
> [!WARNING]
> This pull request was generated with LLM assistance.
```

The alert should appear before every other heading, summary, checklist, or metadata block.

## Body Structure

Check that the body is concise and uses these sections when applicable:

```markdown
> [!WARNING]
> This pull request was generated with LLM assistance.

## Summary
- ...

## Verification
- ...
```

Recommend sections only when they carry useful information:

- `Summary`: user-facing or maintainer-facing changes, grouped by behavior or area.
- `Verification`: commands run and their results, such as `dotnet build CruiserJumpPractice.sln`.
- `Notes`: limitations, skipped checks, migration notes, or reviewer attention points.
- `Breaking Changes`: required when the title or commits include `!` or `BREAKING CHANGE`.

## Style

- Keep PR bodies short and reviewable.
- Prefer bullets over long paragraphs.
- Mention paths or commands in backticks.
- Do not paste large diffs.
- Be explicit when verification was not run.
- Align the PR title type with the dominant change: for example, `docs:` for documentation-only skill additions and `refactor:` for behavior-preserving code cleanup.
