---
# SPDX-License-Identifier: Unlicense
name: pull-request-quality-check
description: Quality-check repository pull requests, review comments, replies, and PR-thread notes.
---

# Pull Request Quality Check

## When to Use

- Use this skill when creating, updating, reviewing, or validating pull request
  titles, bodies, review comments, replies, or PR-thread notes.

## Title

Check that the title uses a Conventional Commits-style format:

```text
<type>[optional scope][optional !]: <description>
```

Use `commit-message-quality-check` for type selection and breaking-change notation.

Reference: https://www.conventionalcommits.org/en/v1.0.0/

## Required LLM Alert

When the PR was prepared with LLM assistance, check for this GitHub alert.
It must appear at the very top of the PR body:

```markdown
> [!WARNING]
> This pull request was created with assistance from LLMs.
```

The alert should appear before every other heading, summary, checklist, or
metadata block.

## Pull Request Template

Before creating or replacing a pull request body, check for a repository pull
request template.
Use the first applicable template found in the normal GitHub locations, such as:

- `.github/pull_request_template.md`
- `.github/PULL_REQUEST_TEMPLATE.md`
- `docs/pull_request_template.md`
- `docs/PULL_REQUEST_TEMPLATE.md`
- A user-selected file under `.github/PULL_REQUEST_TEMPLATE/` or `docs/PULL_REQUEST_TEMPLATE/`

When a template exists:

- Read the template file before drafting the body. Do not rely on memory,
  previous PR bodies, or heading summaries.
- Follow the template's visible headings, required checkboxes, and required-if-applicable sections.
- Use HTML comments in the template as author guidance. Do not copy those comments into the rendered PR body.
- Keep required alerts, checklist confirmations, and section names compatible with the template.
- Fill sections with `None` or `Not applicable` only when the template or
  surrounding policy asks for an explicit value.
- Do not replace template-specific headings with generic defaults. For example,
  do not use `Verification` when the template uses `Testing`.
- Do not add obsolete checklist items that are no longer present in the repository template.
- Copy required checklist wording from the current template exactly. Change
  unchecked boxes to checked boxes only when the author can truthfully confirm
  the item.
- When updating an existing PR body, remove or revise stale body text that
  describes removed template items. For example, remove an old AI-disclosure
  checkbox that no longer exists.
- If the template should apply but the exact file or selected variant is
  unavailable, stop and get the template. Do not invent placeholder headings or
  checklist text.

## Verification Evidence

- Do not present autonomous AI review, inspection, or scenario analysis as a manual check.
- Manual checks should describe checks performed without AI automation.
- If an AI-assisted inspection was requested, report it under a `### AI-assisted inspections` subsection inside
  `## Testing`, after `### Automated checks` when both sections are present.
- In `### AI-assisted inspections`, use a short `Request: ...` line to summarize the requested inspection. Nest the
  `AI-assisted result: ...` under that request summary.
- Keep automated commands, CI results, non-AI manual checks, screenshots, videos, and AI-assisted inspection results
  distinct from each other.

When no repository template exists, use the same visible structure as the
repository pull request template.
Keep the top-level headings, testing subsection order, and CLA checklist from this fallback scaffold. When a scaffold
subsection has no applicable content, write `None` or `Not applicable` instead of removing the heading:

```markdown
> [!WARNING]
> This pull request was created with assistance from LLMs.

## Summary
- ...

## Related Issues
- None.

## Notes for reviewers
- ...

### AI disclosure
- ...

## Testing

### Automated checks
- ...

### AI-assisted inspections
- ...

### Manual checks
- ...

### Screenshots / videos
- ...

## Checklist

As the pull request author, I have checked all required items:

- [ ] I have read `CONTRIBUTING.md` and agree to the Contribution License Agreement.
```

Use fallback sections this way:

- `Summary`: user-facing or maintainer-facing changes, grouped by behavior or area.
- `Related Issues`: GitHub issues, pull requests, external references, or `None`.
  When linking related work, state how it is related. If the explanation is too
  long for the reference line, put the reference on the parent bullet and the
  explanation on an indented child bullet. Keep the child explanation readable:
  split packed relationship-and-purpose wording into as many short sentences as
  needed when one sentence carries too many ideas.
- `Notes for reviewers`: limitations, skipped checks, migration notes, reviewer
  attention points, or review focus.
- `AI disclosure`: significant AI assistance details, or `None` when no
  significant AI assistance was used.
- `Testing`: automated commands, CI results, AI-assisted inspections, manual checks, screenshots, or videos.
- `Breaking Changes`: required when the title or commits include `!` or `BREAKING CHANGE`.

## Style

- Write pull request titles, pull request bodies, review comments, and replies in English.
- Preserve non-English text only when quoting source text, branch names, commit messages, file
  contents, logs, or existing discussion snippets that must remain exact.
- Keep PR bodies short and reviewable.
- Use `document-quality-check` for explanatory prose that needs readability, structure, or nuance
  review.
- Prefer bullets over long paragraphs.
- Mention paths or commands in backticks.
- Do not paste large diffs.
- Be explicit when verification was not run.
- Align the PR title type with the dominant change. For example, use `docs:`
  for documentation-only skill additions and `refactor:` for behavior-preserving
  code cleanup.

## Pull Request Replies and Reviews

When a pull request reply or review was prepared with LLM assistance, check for
this GitHub alert.
It must appear at the very top of the comment or review body:

```markdown
> [!WARNING]
> This comment was created with assistance from LLMs.
```

The alert should appear before every other paragraph, heading, checklist, quote,
finding, or metadata block.

Use `Update Note`, `Discussion Note`, or `Review Note` sections only when the
active task asks for process notes, decision logs, review summaries, or granular
PR-thread updates. Do not add them by default. Frequent process notes can
clutter the PR conversation and may expose unnecessary implementation context.
When enabled:

- Use `Update Note` for a concrete change that was just made to the pull request.
- Use `Discussion Note` for a decision, tradeoff, or rationale that should
  remain visible in the PR thread.
- Use `Review Note` when reporting a review pass, consistency check, readiness
  check, or retrospective verification that did not itself make a concrete
  change.
- If the active task asks for "history so far", "notes so far", or similar
  retrospective PR-thread notes, do not dump raw commit history. Create separate
  concise notes grouped by meaningful decision or change theme, using
  `Update Note` for concrete PR changes and `Discussion Note` for decisions,
  tradeoffs, or rationale.
- If the requester asks for notes to be separate, or a correction includes two
  independently reviewable changes, post separate PR comments for each theme
  instead of combining them into one broad note.
- Immediately after the required LLM alert and before the note heading, add a
  neutral `Request addressed: ...` line. Use it only as a concise marker for
  later PR-body `### AI-assisted inspections` summaries; do not use it to
  classify requester identity, role, or authority.
- Keep each note concise and limited to information that is safe and useful for
  future reviewers.
- Base notes on confirmed PR context. If a note includes an inference or assumption, label it as such.
- Do not include secrets, private discussion, local-only paths, hidden
  chain-of-thought, or unrelated implementation details.
- Prefer a normal short reply when the comment only needs to answer a question,
  report review results, or acknowledge completion.

## CLI Safety

When creating or editing PR bodies with a shell command, avoid passing Markdown
directly through command arguments. Backticks, quotes, dollar signs, backslashes,
and multiple lines are easy to corrupt in shell arguments. Shells such as
PowerShell and bash can interpret those characters silently.

- Prefer writing the body to a temporary Markdown file. Pass it with
  `gh pr create --body-file <file>` or `gh pr edit --body-file <file>`.
- After creating or editing a PR through `gh`, verify the stored body with
  `gh pr view --json body`. Fix any quoting issues before finishing.
- Remove any temporary body file from the worktree after verification.
