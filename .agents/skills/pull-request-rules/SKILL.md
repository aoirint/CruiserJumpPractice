---
name: pull-request-rules
description: Apply repository pull-request rules. Use when Codex creates, edits, reviews, or suggests pull request titles and bodies, especially when an LLM-generated warning or Conventional Commits-aligned PR title is requested.
---

# Pull Request Rules

Use this skill when preparing pull requests for this repository.

## Title

Use a Conventional Commits-style title:

```text
<type>[optional scope][optional !]: <description>
```

Follow the commit-message skill for type selection and breaking-change notation:

```text
.agents/skills/commit-message-rules/SKILL.md
```

Reference: https://www.conventionalcommits.org/en/v1.0.0/

## Required LLM Alert

When the PR was prepared with LLM assistance, put this GitHub alert at the very top of the PR body:

```markdown
> [!WARNING]
> This pull request was generated with assistance from an LLM. Please review the changes carefully.
```

Keep the alert before every other heading, summary, checklist, or metadata block.

## Body Structure

Use a concise body with these sections when applicable:

```markdown
> [!WARNING]
> This pull request was generated with assistance from an LLM. Please review the changes carefully.

## Summary
- ...

## Verification
- ...
```

Add sections only when they carry useful information:

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
