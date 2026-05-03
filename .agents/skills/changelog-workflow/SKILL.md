---
# SPDX-License-Identifier: Unlicense
name: changelog-workflow
description: Create and update a canonical developer changelog. Use when adding developer-facing release history, maintaining Keep a Changelog sections, or preparing canonical version entries.
---

# Changelog Workflow

## When to Use

- Use this skill when updating the canonical developer changelog.
- Use this skill when preparing canonical versioned release history before a
  release.
- Use this skill when preserving developer-facing prerelease, compatibility,
  migration, CI/build/package, or implementation context.

## Goals

- Keep the confirmed canonical changelog, often `CHANGELOG.md`, as the
  Keep a Changelog-style release history.
- Record notable changes for maintainers in a human-readable, newest-first
  format.
- Preserve prerelease and internal context in the canonical changelog even when
  it will not appear in user-facing release notes.
- Preserve enough rationale and history for user-facing release notes to explain
  why each user-visible change matters.
- Keep user-facing release-note rewriting and release-readiness checks in the
  dedicated release-note workflow.

Reference:

- Keep a Changelog 1.1.0: http://keepachangelog.com/en/1.1.0/

## Workflow

1. Update the canonical changelog first when a release-history change is
   needed. If the file is not clearly `CHANGELOG.md`, locate or confirm the
   canonical changelog before editing.
2. Keep `Unreleased` at the top.
3. Move `Unreleased` entries into a versioned section only when the maintainer
   has selected the release version and UTC release date.
   - Do not create placeholder version headings with `TBD`,
     `<maintainer-selected date>`, or similar unfinished release metadata.
     Keep draft material under `Unreleased` until the version and date are
     known.
4. Use Keep a Changelog categories when applicable: `Added`, `Changed`,
   `Deprecated`, `Removed`, `Fixed`, and `Security`.
   - Put supplemental release metadata that is not itself a change type, such as
     compatibility notes and test environments, under `Notes` instead of making
     separate top-level change-category headings.
5. Keep versioned sections newest first, with headings such as
   `## v1.2.3 - 2026-05-03 UTC`.
6. Record developer-facing details that help future maintainers, including:
   - Internal migrations or architecture changes.
   - CI, build, packaging, or dependency context.
   - Prerelease entries and why they matter.
   - User-impact rationale or history for each user-visible change, such as
     the problem it solves, why behavior changed, compatibility constraints, or
     migration reason.
   - A sourced user impact statement may be enough when it explains why users
     should care. Generic bullets such as documentation updates, dependency
     updates, crash fixes, or save-format changes still need source material
     describing the user-facing benefit, risk, security impact, compatibility
     impact, or migration reason.
   - Compatibility notes, test environments, known limitations, yanked
     releases, and migration constraints.
   - Record compatibility, support, environment, and migration claims only when
     they are maintainer-confirmed or clearly sourced. Otherwise list them as
     missing inputs or draft source material needing confirmation, not as final
     compatibility facts.
7. When prerelease entries are later superseded, keep enough canonical history
   to explain what changed and what reached the stable release. If the stable
   release is still only planned, leave the stable roll-up material under
   `Unreleased` instead of creating an unfinished stable heading.
8. If the user asks for user-facing release notes, verify or prepare only the
   canonical source material here, then use the appropriate release-note
   workflow for the user-facing rewrite and release-readiness checks.
   - Update the canonical changelog only when the source material is missing,
     stale, or needs maintainer-facing correction.
   - Treat missing rationale or history for a user-visible change as missing
     canonical source material; add or request it before user-facing rewriting.
   - If the canonical changelog already contains enough source material, stop
     there and hand off user-facing rewriting and readiness checks.
   - Identify the release-note workflow by name when it exists; otherwise
     discover the relevant release-note guidance before producing user-facing
     notes.
   - If confirmed version, UTC release date, target audience, publication
     channel, or release-note workflow are missing, state those as input gaps
     and stop after canonical source preparation.

## Boundaries

- This skill owns the canonical developer changelog; it does not own generated
  or user-facing release-note files.
- Do not publish, tag, create releases, upload artifacts, or edit release
  metadata unless another workflow and explicit user request call for those
  side effects.
- Keep private workspace paths, temporary worktree names, command transcripts,
  and authentication details out of public changelog text.
