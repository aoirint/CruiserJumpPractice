---
# SPDX-License-Identifier: Unlicense
name: changelog-workflow
description: Create and update this repository's canonical developer changelog and Thunderstore-facing user release notes. Use when preparing release notes, updating stable-release notes, or deriving Thunderstore package changelog text from CHANGELOG.md.
---

# Changelog Workflow

## When to Use

- Use this skill when updating `CHANGELOG.md`.
- Use this skill when preparing or reviewing `assets/CHANGELOG.md` for a
  Thunderstore package.
- Use this skill when stable release notes should be derived from developer
  changelog entries.

## Goals

- Keep `CHANGELOG.md` as the canonical Keep a Changelog-style release history.
- Keep `assets/CHANGELOG.md` stable-release-only, user-facing, and suitable for
  Thunderstore packaging.
- Preserve prerelease and maintainer context in the canonical changelog without
  publishing prerelease-only noise to Thunderstore.
- Make derivation and release verification reviewable before publishing.

Reference:

- Keep a Changelog 1.1.0: http://keepachangelog.com/en/1.1.0/

## Workflow

1. Update `CHANGELOG.md` first with notable developer-facing release history.
2. Use Keep a Changelog categories when applicable: `Added`, `Changed`,
   `Deprecated`, `Removed`, `Fixed`, and `Security`.
3. Keep `Unreleased` at the top and move entries into a versioned section with
   a release date when preparing a release.
   - Do not finalize a versioned section until the maintainer has selected the
     stable version and UTC release date.
4. Decide whether the release is stable:
   - Stable releases use a version without a prerelease suffix, such as
     `1.2.3`.
   - Prerelease versions, such as `1.2.3-alpha.1`, remain developer-facing and
     are not published to Thunderstore because Thunderstore does not support
     prerelease package versions for this project.
5. Roll prerelease entries into the next stable release when they still affect
   stable users.
6. Omit prerelease-only details from `assets/CHANGELOG.md` when they were
   internal, superseded before stable release, or useful only to maintainers.
7. Keep `assets/CHANGELOG.md` as the cumulative Thunderstore-facing stable
   release history, with the latest stable release at the top.
   - Before packaging, the top section must be a stable version heading, not
     `Unreleased`.
8. Derive `assets/CHANGELOG.md` from exact stable entries in `CHANGELOG.md` and
   rewrite the text around user-visible behavior, installation, compatibility,
   and known limitations.
   - If the exact canonical changelog section is unavailable, write draft
     Thunderstore notes and list the missing inputs instead of presenting the
     output as final.
9. Preserve user-critical notes in `assets/CHANGELOG.md`, including breaking
   changes, compatibility changes, installation or update notes, removals,
   deprecations, security fixes, yanked releases, and known limitations.
10. Keep test environment details in `CHANGELOG.md`. Include them in
    `assets/CHANGELOG.md` only when they are needed as user-facing compatibility
    or support context.
    - Use maintainer-confirmed compatibility metadata from the canonical
      changelog, prior release notes, tested game/dependency versions, or
      explicit maintainer input. Do not invent compatibility claims.
11. Before release packaging, verify:
   - `CruiserJumpPractice/CruiserJumpPractice.csproj` contains the intended
     release version.
   - The intended stable tag, using this repository's `v<version>` convention
     from the `generate-version` action, does not already exist locally or
     remotely. Existing versions are treated as edge builds. If remote tag
     verification cannot be performed, report it as a release-readiness blocker.
   - `assets/manifest.json` will receive the same version through the
     `generate-version` action.
   - `assets/CHANGELOG.md` contains only Thunderstore-appropriate stable
     release notes.
   - `.github/workflows/build.yml` packages `assets/CHANGELOG.md`.
   - `dotnet build CruiserJumpPractice.sln` succeeds after any related changes.

## Boundaries

- This skill covers changelog and release-note preparation, not publishing.
- Do not upload to Thunderstore, create GitHub releases, or tag releases unless
  the user explicitly asks for those side effects.
- Keep private workspace paths, temporary worktree names, command transcripts,
  and authentication details out of public changelog or release-note text.

## Review-Only Output

When the user asks for a release-readiness review instead of edits, return:

1. Current state of `CHANGELOG.md`, `assets/CHANGELOG.md`, project version,
   local and remote tag readiness, manifest version flow, and package workflow
   inclusion.
2. Required updates before packaging.
3. Blockers or missing inputs, especially stable version, UTC release date,
   exact canonical changelog text, and compatibility metadata.
4. Confirmation that no publishing side effects were performed.
