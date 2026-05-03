---
# SPDX-License-Identifier: Unlicense
name: release-note-workflow
description: Create, update, or review Thunderstore-facing release notes. Use when deriving user-facing stable release notes from a canonical changelog or checking release-note readiness before packaging.
---

# Release Note Workflow

## When to Use

- Use this skill when preparing or reviewing Thunderstore package release notes.
- Use this skill when deriving stable release notes from canonical
  changelog entries.
- Use this skill when checking release-note readiness before packaging.

## Goals

- Keep the Thunderstore-facing release-note file as the cumulative stable
  release history, with the latest stable release at the top.
- Derive release notes from the canonical developer changelog instead of making
  a second source of truth.
- Rewrite stable release notes for users, focusing on behavior, installation,
  compatibility, update impact, known limitations, and security.
- Make version, tag, manifest, and packaging readiness explicit before release.

## Workflow

1. Locate the canonical developer changelog, Thunderstore-facing release-note
   file, package metadata, manifest source or output path, packaging workflow,
   and verification command from repository evidence such as docs, configs,
   scripts, or workflow files. Report anything undiscoverable as a missing input
   or blocker.
2. Read the exact stable release section in the canonical developer changelog.
   - If the exact canonical section is unavailable, write draft Thunderstore
     notes and list the missing inputs instead of presenting the output as
     final.
   - If the UTC release date is missing, keep the output clearly marked as
     draft review text. Do not create a package-ready heading with `TBD`,
     `<date>`, or similar placeholder release metadata.
3. Confirm the release is stable:
   - Stable releases use a version without a prerelease suffix, such as
     `1.2.3`.
   - Prerelease versions, such as `1.2.3-alpha.1`, remain developer-facing and
     are not published to Thunderstore unless the package target explicitly
     supports prerelease versions.
   - Accept prerelease support only from maintained package-target policy,
     package documentation, explicit maintainer input, or existing package
     history.
4. Roll prerelease entries into the next stable release when they still affect
   stable users.
5. Omit prerelease-only details from the Thunderstore-facing release notes when
   they were internal, superseded before stable release, or useful only to
   maintainers.
6. Rewrite the stable entries around user-visible behavior, installation,
   compatibility, update impact, security, and known limitations.
7. Preserve user-critical notes in the Thunderstore-facing release notes,
   including breaking changes, compatibility changes, installation or update
   notes, removals, deprecations, security fixes, yanked releases, and known
   limitations.
8. Keep test environment details in the canonical developer changelog. Include
   them in Thunderstore-facing release notes only when they are needed as
   user-facing compatibility or support context.
   - Use maintainer-confirmed compatibility metadata from the canonical
     changelog, prior release notes, tested game/dependency versions, or
     explicit maintainer input. Do not invent compatibility claims.
9. Before packaging, verify:
   - The Thunderstore-facing release-note file has a stable version heading at
     the top, not `Unreleased`, and does not contain placeholder release
     metadata such as `TBD`.
   - Project package metadata contains the intended release version.
   - The intended package or manifest version is not already present in a way
     that would collide with the planned stable release.
   - The intended stable tag, using the repository's documented tag convention,
     does not already exist locally or remotely. Existing versions or tags are
     blockers for stable packaging until the maintainer chooses a new version or
     a documented edge-release path. If remote tag verification cannot be
     performed, report it as a release-readiness blocker.
   - The Thunderstore manifest will receive the same version as the package
     metadata.
   - The packaging workflow includes the Thunderstore-facing release-note file.
   - The repository's documented build or packaging verification succeeds after
     any related changes.

## Review-Only Output

When the user asks for a release-readiness review instead of edits, do not edit
files. Return:

1. Current state of the Thunderstore-facing release-note file, source changelog
   section, project version, local and remote tag readiness, manifest version
   flow, and package workflow inclusion.
2. Required updates before packaging.
3. Blockers or missing inputs, especially stable version, UTC release date,
   exact canonical changelog text, and compatibility metadata.
4. Confirmation that no publishing side effects were performed.

## Output Shape

- Package-ready stable notes use the package target's required heading format
  with a confirmed stable version and UTC release date.
- Draft notes are clearly labeled as draft review text and list missing inputs
  before any proposed user-facing wording.
- Blockers name the missing or failed readiness item directly, such as stable
  metadata, tag convention, local or remote tag availability, manifest version
  flow, packaging inclusion, or verification command.

## Boundaries

- This skill owns Thunderstore-facing release notes and release-note readiness,
  not canonical developer changelog authoring.
- Use `changelog-workflow` first when the canonical developer changelog itself
  needs new entries or version-section changes.
- This skill may review publishing and tag readiness, but it does not perform
  publishing, release creation, or tag creation. Use a dedicated publishing or
  release workflow for those side effects.
- Treat missing stable metadata, missing tag convention, failed local or remote
  tag verification, and missing prerelease-support evidence as release-readiness
  blockers.
- Keep private workspace paths, temporary worktree names, command transcripts,
  and authentication details out of public release-note text.
