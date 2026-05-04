<!-- SPDX-License-Identifier: Unlicense -->

# Fallback Pull Request Body

Use this fallback only when the repository has no applicable pull request
template. If a repository template exists, the live template takes precedence.

Keep the top-level headings, testing subsection order, and CLA checklist from
this scaffold. When a scaffold subsection has no applicable content, write
`None` or `Not applicable` instead of removing the heading.

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
  `BREAKING CHANGE`.
