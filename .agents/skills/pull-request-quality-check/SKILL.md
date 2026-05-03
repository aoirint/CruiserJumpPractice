---
# SPDX-License-Identifier: Unlicense
name: pull-request-quality-check
description: Quality-check repository pull requests. Use when creating or updating pull requests.
---

# Pull Request Quality Check

## When to Use

- Use this skill when creating, updating, reviewing, or validating pull request
  titles or bodies.

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
> This pull request was created with assistance from LLMs.
```

The alert should appear before every other heading, summary, checklist, or metadata block.

## Body Structure

Check that the body is concise and uses these sections when applicable:

```markdown
> [!WARNING]
> This pull request was created with assistance from LLMs.

## Summary
- ...

## Verification
- ...
```

Recommend sections only when they carry useful information:

- `Summary`: user-facing or maintainer-facing changes, grouped by behavior or area.
- `Verification`: commands run and their results, such as project builds,
  tests, linters, formatters, or structural validators.
- `Notes`: limitations, skipped checks, migration notes, or reviewer attention points.
- `Breaking Changes`: required when the title or commits include `!` or `BREAKING CHANGE`.

## Style

- Write pull request titles, pull request bodies, review comments, and replies in English.
- Preserve non-English text only when quoting source text, branch names, commit messages, file
  contents, logs, or existing discussion snippets that must remain exact.
- Keep PR bodies short and reviewable.
- Prefer bullets over long paragraphs.
- Mention paths or commands in backticks.
- Do not paste large diffs.
- Be explicit when verification was not run.
- Align the PR title type with the dominant change: for example, `docs:` for documentation-only skill additions and `refactor:` for behavior-preserving code cleanup.

## Pull Request Replies and Reviews

When a pull request reply or review was prepared with LLM assistance, check that this GitHub alert appears at the very top of the comment or review body:

```markdown
> [!WARNING]
> This comment was created with assistance from LLMs.
```

The alert should appear before every other paragraph, heading, checklist, quote, finding, or metadata block.

Use `Update Note` or `Discussion Note` sections only when the user or maintainer explicitly asks for process notes,
decision logs, or granular PR-thread updates. Do not add them by default: frequent process notes can clutter the PR
conversation and may expose unnecessary implementation context. When enabled:

- Use `Update Note` for a concrete change that was just made to the pull request.
- Use `Discussion Note` for a decision, tradeoff, or rationale that should remain visible in the PR thread.
- Keep each note concise and limited to information that is safe and useful for future reviewers.
- Base notes on confirmed PR context. If a note includes an inference or assumption, label it as such.
- Do not include secrets, private discussion, local-only paths, hidden chain-of-thought, or unrelated implementation
  details.
- Prefer a normal short reply when the comment only needs to answer a question, report review results, or acknowledge
  completion.

## CLI Safety

When creating or editing PR bodies with a shell command, avoid passing Markdown directly through command arguments if it contains backticks, quotes, dollar signs, backslashes, or multiple lines. Shells such as PowerShell and bash can interpret those characters and silently corrupt the body.

- Prefer writing the body to a temporary Markdown file and passing it with `gh pr create --body-file <file>` or `gh pr edit --body-file <file>`.
- After creating or editing a PR through `gh`, verify the stored body with `gh pr view --json body` and fix any quoting issues before finishing.
- Remove any temporary body file from the worktree after verification.
