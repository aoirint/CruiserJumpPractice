---
# SPDX-License-Identifier: Unlicense
name: changelog-workflow
description: Create and update this repository's canonical developer changelog in CHANGELOG.md. Use when adding developer-facing release history, maintaining Keep a Changelog sections, or preparing canonical version entries.
---

# Changelog Workflow

## When to Use

- Use this skill when updating `CHANGELOG.md`.
- Use this skill when preparing canonical versioned release history before a
  release.
- Use this skill when preserving developer-facing prerelease, compatibility,
  migration, CI/build/package, or implementation context.

## Goals

- Keep `CHANGELOG.md` as the canonical Keep a Changelog-style release history.
- Record notable changes for maintainers in a human-readable, newest-first
  format.
- Preserve prerelease and internal context in the canonical changelog even when
  it will not appear in Thunderstore-facing release notes.
- Keep Thunderstore-specific derivation and release-readiness checks in
  `release-note-workflow`.

Reference:

- Keep a Changelog 1.1.0: http://keepachangelog.com/en/1.1.0/

## Workflow

1. Update `CHANGELOG.md` first when a release-history change is needed.
2. Keep `Unreleased` at the top.
3. Move `Unreleased` entries into a versioned section only when the maintainer
   has selected the release version and UTC release date.
   - Do not create placeholder version headings with `TBD`,
     `<maintainer-selected date>`, or similar unfinished release metadata.
     Keep draft material under `Unreleased` until the version and date are
     known.
4. Use Keep a Changelog categories when applicable: `Added`, `Changed`,
   `Deprecated`, `Removed`, `Fixed`, and `Security`.
5. Keep versioned sections newest first, with headings such as
   `## v1.2.3 - 2026-05-03 UTC`.
6. Record developer-facing details that help future maintainers, including:
   - Internal migrations or architecture changes.
   - CI, build, packaging, or dependency context.
   - Prerelease entries and why they matter.
   - Compatibility notes, test environments, known limitations, yanked
     releases, and migration constraints.
7. When prerelease entries are later superseded, keep enough canonical history
   to explain what changed and what reached the stable release. If the stable
   release is still only planned, leave the stable roll-up material under
   `Unreleased` instead of creating an unfinished stable heading.
8. If the user asks for Thunderstore-facing notes, derive only the canonical
   source material here, then use `release-note-workflow` for the user-facing
   rewrite and release-readiness checks.

## Boundaries

- This skill owns `CHANGELOG.md`; it does not own `assets/CHANGELOG.md`.
- Do not publish, tag, create GitHub releases, upload to Thunderstore, or edit
  package metadata unless another workflow and explicit user request call for
  those side effects.
- Keep private workspace paths, temporary worktree names, command transcripts,
  and authentication details out of public changelog text.
