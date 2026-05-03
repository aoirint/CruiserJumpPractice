---
name: changelog-workflow
description: Create and update repository CHANGELOG.md files using Keep a Changelog style. Use when Codex is asked to add release notes, update CHANGELOG.md, prepare a release changelog, move Unreleased entries into a version, or document notable changes for a version in this repository.
---

# Changelog Workflow

Use Keep a Changelog style for `CHANGELOG.md`, adapted for this repository.

Reference: https://keepachangelog.com/en/1.1.0/

## When to Use

- Use this skill when creating, updating, reviewing, or validating `CHANGELOG.md`.
- Use this skill when preparing release notes, moving `Unreleased` entries into a version, or
  documenting notable branch changes.

## Goals

- Keep `CHANGELOG.md` useful to people, not just a raw commit log.
- Preserve this repository's existing changelog language and Keep a Changelog structure.
- Keep release versions, dates, and compare links consistent with project metadata and Git tags.
- Record notable user-facing, release, packaging, CI, or contributor-workflow changes before PRs and
  releases.

## Workflow

1. Inspect existing `CHANGELOG.md`, project version metadata, tags, and recent commits.
2. Preserve user-written changelog wording unless it is clearly inconsistent with this skill.
3. Add notable changes under `Unreleased` while work is not released.
4. For a release, move relevant `Unreleased` entries into a new version section and add the release
   date.
5. Summarize user-visible changes. Avoid dumping every commit.
6. Group each entry by the most appropriate change type.
7. Add or update compare links at the bottom:
   - `[Unreleased]: <repo>/compare/vX.Y.Z...HEAD`
   - `[X.Y.Z]: <repo>/compare/vPREV...vX.Y.Z`
   - For the first release, `[X.Y.Z]: <repo>/releases/tag/vX.Y.Z`

## Format

- Name the file `CHANGELOG.md`.
- Write the changelog for people, not as a raw commit log.
- Keep releases in reverse chronological order, newest first.
- Keep `## [Unreleased]` at the top.
- Use linked version headings and link references at the bottom.
- Use ISO date format `YYYY-MM-DD`.
- Use the current local date for a release prepared today unless the user gives another date.
- Match the repository's existing version tag style, such as `vX.Y.Z` when tags use a `v` prefix.
- Use project metadata as the source of truth for release versions. In this repository, check the
  `<Version>` value in `CruiserJumpPractice/CruiserJumpPractice.csproj` when release versioning is
  in scope.
- Mention that the format is based on Keep a Changelog and that the project follows Semantic
  Versioning.

## Language

- Match the existing `CHANGELOG.md` language if the file already exists.
- If creating a new `CHANGELOG.md`, use English unless the user asks for another language.
- Keep only `Unreleased` and change-type headings in English.
- Keep the standard change-type headings exactly as:
  - `Added`
  - `Changed`
  - `Deprecated`
  - `Removed`
  - `Fixed`
  - `Security`
- Omit empty change-type sections.

## Automation Policy

- When project version metadata changes in the same task, always update `CHANGELOG.md` in the same
  branch and commit.
- If the task prepares a release, add a new released section with the release date even when
  `Unreleased` is empty.
- If there are no user-visible changes, add one concise bullet that explains why the release was cut.
- If the task is not a release, append notable items to `Unreleased` instead of creating a version section.
- Before finishing, verify that version headings and compare links remain consistent with the latest and previous tags.
- Before creating a commit, check whether the diff contains notable user-facing, release, packaging, CI, or contributor-workflow changes. If yes, update `Unreleased` in the same commit.
- Before creating or updating a pull request, re-check the branch diff and make sure `Unreleased` or the new release section reflects the latest branch state.

## Entry Style

- Use short bullet points.
- Start bullets with the affected feature or behavior when it improves scanning.
- Include breaking changes under `Changed` or `Removed`, and call them out clearly.
- Use `Security` only for vulnerability-related changes.
- Do not include internal-only chores unless they affect release, installation, packaging, operations, or contributor workflows.
