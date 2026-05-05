<!-- SPDX-License-Identifier: Unlicense -->

# Fallback Pull Request Body

Use this fallback only when the repository has no applicable pull request
template. If a repository template exists, the live template takes precedence.

## Contents

- [Fallback Scaffold](#fallback-scaffold)
- [Section Guidance](#section-guidance)

## Fallback Scaffold

Keep the fallback structure aligned with `.github/pull_request_template.md`.
When a scaffold subsection has no applicable content, write `None` or
`Not applicable` instead of removing the heading.

````markdown
<!--
If significant AI assistance affected this pull request, put this alert at the
very top of the PR body:

> [!WARNING]
> This pull request was created with assistance from LLMs.

Then describe the AI assistance under "AI disclosure" below.
-->

## Summary

<!--
Briefly describe what changed, who it affects, and why it is useful.
If this pull request mixes unrelated behavior, documentation, refactors, or
cleanup, split it or explain why the work should stay together.
If the title or commits include `!` or `BREAKING CHANGE`, include a
`### Breaking Changes` subsection.
Treat a change as breaking when it removes, renames, or incompatibly changes
public behavior, documented workflows, configuration, package or release
behavior, compatibility guarantees, or other project-facing contracts that
users, maintainers, automation, or downstream packaging reasonably rely on.

Optional H3 examples:
### User impact
### Contributor impact
### Maintainer impact
### Breaking Changes
-->

## Related Issues

<!--
Prefer Markdown list items for GitHub issues or pull requests so GitHub can
render rich references.
Use closing keywords, such as "Closes #123", when this pull request should
close an issue.
For non-GitHub references, include enough context for reviewers to understand
why the link matters.
Use "None" if there is no related issue.

Markdown list examples:
- Closes #123
- Refs #456
- Refs owner/repository#789
-->

## Notes for reviewers

<!--
Share non-testing context that helps reviewers understand or prioritize this
pull request.
For example, mention review focus, trade-offs, compatibility risks,
generated outputs, packaging concerns,
or areas that need extra attention for reasons other than AI assistance.
-->

### AI disclosure

<!--
If AI assistance significantly affected this pull request, disclose it here.
Mention what the AI helped with, how you reviewed or adapted the result, and
any AI-assisted areas you did not review closely.
Use "None" if no significant AI assistance was used.

Optional H3 examples:
### Review focus
### Lower-confidence areas
-->

## Testing

<!--
List the checks you ran and their results.
Include commands, manual in-game checks, screenshots, or videos when relevant.
For docs-only changes, mention proofreading, link checks, formatting checks,
or "Not run - docs only."
If you did not run a relevant check, explain why.
You are responsible for masking personal information, local absolute paths,
access tokens, and other sensitive details before posting logs, screenshots,
or videos.
Do not present AI-performed review, inspection, editing, verification, or other
work as "manual". For example, if you include AI-assisted inspection, list a
short `Request: ...` summary first, nest the `AI-assisted result: ...` under it,
and clearly label the result as AI-assisted.

Optional testing structure:
### Build log

<details>

```plain
$ DOTNET_CLI_UI_LANGUAGE=en dotnet build
Paste the relevant output here.
```

</details>

### Automated checks
### AI-assisted inspections
### Manual checks
### Screenshots / videos
-->

## Checklist

<!--
Check this item before submitting.
Pull requests cannot be merged without Contribution License Agreement
confirmation.
-->

As the pull request author, I have checked all required items:

- [ ] I have read `CONTRIBUTING.md` and agree to the Contribution License Agreement.
````

## Section Guidance

Use fallback sections this way:

- `Summary`: user-facing or maintainer-facing changes, grouped by behavior or
  area.
- `Related Issues`: GitHub issues, pull requests, external references, or
  `None`.
  When linking related work, state how it is related. If the explanation is too
  long for the reference line, put the reference on the parent bullet and the
  explanation on an indented child bullet. Keep the child explanation readable:
  split packed relationship-and-purpose wording into as many short sentences as
  needed when one sentence carries too many ideas.
- `Notes for reviewers`: limitations, skipped checks, migration notes, reviewer
  attention points, or review focus.
- `AI disclosure`: significant AI assistance details, or `None` when no
  significant AI assistance was used.
- `Testing`: automated commands, CI results, AI-assisted inspections, manual
  checks, screenshots, or videos.
- `Breaking Changes`: required when the title or commits include `!` or
  `BREAKING CHANGE`. Treat backward-incompatible changes to public behavior,
  documented workflows, configuration, package or release behavior,
  compatibility guarantees, or other project-facing contracts as breaking.
