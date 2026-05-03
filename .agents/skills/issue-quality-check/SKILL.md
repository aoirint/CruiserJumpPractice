---
name: issue-quality-check
description: Quality-check repository issues and issue replies. Use when creating or updating GitHub issues or comments on issues.
---

# Issue Quality Check

## When to Use

- Use this skill when creating, updating, reviewing, or validating GitHub issue titles or bodies for this repository.
- Use this skill when creating, updating, reviewing, or validating replies or comments on GitHub issues for this repository.

## Title

Check that the title is concise, specific, and written as a problem or task:

- Prefer a clear noun phrase or imperative task, such as `Add practice reset hotkey documentation` or `Fix cruiser state reload after scene transition`.
- Include the affected area when it helps triage, such as `MagnetService:` or `docs:`.
- Avoid vague titles such as `Bug`, `Question`, `Help`, or `Does not work`.
- Do not force Conventional Commits format for issues unless the repository explicitly asks for it in that issue flow.

## Required LLM Alert

When the issue was prepared with LLM assistance, check that this GitHub alert appears at the very top of the issue body:

```markdown
> [!WARNING]
> This issue was created with assistance from LLMs.
```

For Japanese issue bodies, use this alert text instead:

```markdown
> [!WARNING]
> このIssueはLLMの支援を受けて作成されました。
```

The alert should appear before every other heading, summary, checklist, template field, or metadata block.

## Body Structure

Check that the body is concise and uses these sections when applicable:

```markdown
> [!WARNING]
> This issue was created with assistance from LLMs.

## Summary
- ...

## Details
- ...

## Acceptance Criteria
- ...
```

Recommend sections only when they carry useful information:

- `Summary`: the problem, request, or outcome in maintainer-facing language.
- `Details`: relevant context, reproduction notes, affected paths, logs, or constraints.
- `Acceptance Criteria`: concrete checks that would make the issue complete.
- `Verification`: commands, manual checks, or observations already performed.
- `Notes`: limitations, related work, dependencies, or reviewer attention points.

## Style

- Keep issue bodies short and scannable.
- Prefer bullets for facts, steps, and criteria.
- Mention paths, commands, classes, and config keys in backticks.
- Include exact expected and actual behavior for bugs.
- Include reproduction steps only when they are known and useful.
- Do not paste large logs, stack traces, or diffs; summarize and link or attach details when needed.
- Be explicit when verification or reproduction was not run.

## CLI Safety

When creating or editing issue bodies with a shell command, avoid passing Markdown directly through command arguments if it contains backticks, quotes, dollar signs, backslashes, or multiple lines. Shells such as PowerShell and bash can interpret those characters and silently corrupt the body.

- Prefer writing the body to a temporary Markdown file and passing it with `gh issue create --body-file <file>` or `gh issue edit --body-file <file>`.
- After creating or editing an issue through `gh`, verify the stored body with `gh issue view --json body` and fix any quoting issues before finishing.
- Remove any temporary body file from the worktree after verification.

## Issue Replies

When the issue reply was prepared with LLM assistance, check that this GitHub alert appears at the very top of the comment body:

```markdown
> [!WARNING]
> This comment was created with assistance from LLMs.
```

For Japanese issue replies, use this alert text instead:

```markdown
> [!WARNING]
> このコメントはLLMの支援を受けて作成されました。
```

The alert should appear before every other paragraph, heading, checklist, quote, or metadata block.

Check that the reply is concise and uses only the structure needed for the situation:

```markdown
> [!WARNING]
> This comment was created with assistance from LLMs.

Thanks for the report. I can reproduce this with ...

## Next Steps
- ...
```

Recommend sections only when they carry useful information:

- Opening sentence: acknowledge the report, question, or prior comment directly.
- `Findings`: facts discovered from code, logs, reproduction, or maintainer investigation.
- `Next Steps`: what will be done next, what is blocked, or what input is needed.
- `Verification`: commands or manual checks run while investigating.
- `Notes`: caveats, related issues, or constraints that should remain visible.

For issue replies:

- Keep replies focused on the current issue thread.
- Answer direct questions before adding broader context.
- Quote only the smallest useful part of a previous comment.
- Mention paths, commands, classes, and config keys in backticks.
- Be clear whether something is confirmed, inferred, untested, or still unknown.
- Ask for specific missing information when needed, such as logs, mod versions, reproduction steps, or save data details.
- Avoid large diffs, large logs, and unrelated implementation plans.
- Do not promise timelines unless they are already agreed.

When creating or editing issue replies with a shell command:

- Prefer writing the reply to a temporary Markdown file and passing it with `gh issue comment --body-file <file>`.
- After creating or editing a reply through `gh`, verify the stored comment body when possible and fix any quoting issues before finishing.
- Remove any temporary body file from the worktree after verification.
