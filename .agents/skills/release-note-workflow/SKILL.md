---
# SPDX-License-Identifier: Unlicense
name: release-note-workflow
description: Create, update, or review this repository's Thunderstore-facing release notes in assets/CHANGELOG.md. Use when deriving user-facing stable release notes from CHANGELOG.md or checking release-note readiness before packaging.
---

# Release Note Workflow

## When to Use

- Use this skill when preparing or reviewing `assets/CHANGELOG.md` for a
  Thunderstore package.
- Use this skill when deriving stable release notes from canonical
  `CHANGELOG.md` entries.
- Use this skill when checking release-note readiness before packaging.

## Goals

- Keep `assets/CHANGELOG.md` as the cumulative Thunderstore-facing stable
  release history, with the latest stable release at the top.
- Derive release notes from `CHANGELOG.md` instead of making a second source of
  truth.
- Rewrite stable release notes for users, focusing on behavior, installation,
  compatibility, update impact, known limitations, and security.
- Make version, tag, manifest, and packaging readiness explicit before release.

## Workflow

1. Read the exact stable release section in `CHANGELOG.md`.
   - If the exact canonical section is unavailable, write draft Thunderstore
     notes and list the missing inputs instead of presenting the output as
     final.
   - If the UTC release date is missing, keep the output clearly marked as
     draft review text. Do not create a package-ready heading with `TBD`,
     `<date>`, or similar placeholder release metadata.
2. Confirm the release is stable:
   - Stable releases use a version without a prerelease suffix, such as
     `1.2.3`.
   - Prerelease versions, such as `1.2.3-alpha.1`, remain developer-facing and
     are not published to Thunderstore because Thunderstore does not support
     prerelease package versions for this project.
3. Roll prerelease entries into the next stable release when they still affect
   stable users.
4. Omit prerelease-only details from `assets/CHANGELOG.md` when they were
   internal, superseded before stable release, or useful only to maintainers.
5. Rewrite the stable entries around user-visible behavior, installation,
   compatibility, update impact, security, and known limitations.
6. Preserve user-critical notes in `assets/CHANGELOG.md`, including breaking
   changes, compatibility changes, installation or update notes, removals,
   deprecations, security fixes, yanked releases, and known limitations.
7. Keep test environment details in `CHANGELOG.md`. Include them in
   `assets/CHANGELOG.md` only when they are needed as user-facing compatibility
   or support context.
   - Use maintainer-confirmed compatibility metadata from the canonical
     changelog, prior release notes, tested game/dependency versions, or
     explicit maintainer input. Do not invent compatibility claims.
8. Before packaging, verify:
   - `assets/CHANGELOG.md` has a stable version heading at the top, not
     `Unreleased`, and does not contain placeholder release metadata such as
     `TBD`.
   - `CruiserJumpPractice/CruiserJumpPractice.csproj` contains the intended
     release version.
   - The intended stable tag, using this repository's `v<version>` convention
     from the `generate-version` action, does not already exist locally or
     remotely. Existing versions are treated as edge builds. If remote tag
     verification cannot be performed, report it as a release-readiness blocker.
   - `assets/manifest.json` will receive the same version through the
     `generate-version` action.
   - `.github/workflows/build.yml` packages `assets/CHANGELOG.md`.
   - `dotnet build CruiserJumpPractice.sln` succeeds after any related changes.

## Review-Only Output

When the user asks for a release-readiness review instead of edits, return:

1. Current state of `assets/CHANGELOG.md`, source `CHANGELOG.md` section,
   project version, local and remote tag readiness, manifest version flow, and
   package workflow inclusion.
2. Required updates before packaging.
3. Blockers or missing inputs, especially stable version, UTC release date,
   exact canonical changelog text, and compatibility metadata.
4. Confirmation that no publishing side effects were performed.

## Boundaries

- This skill owns Thunderstore-facing release notes and release-note readiness,
  not canonical developer changelog authoring.
- Use `changelog-workflow` first when `CHANGELOG.md` itself needs new
  canonical entries or version-section changes.
- Do not upload to Thunderstore, create GitHub releases, or tag releases unless
  the user explicitly asks for those side effects.
- Keep private workspace paths, temporary worktree names, command transcripts,
  and authentication details out of public release-note text.
