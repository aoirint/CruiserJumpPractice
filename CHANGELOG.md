<!-- SPDX-License-Identifier: Unlicense -->

# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable release entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

## v0.2.0-alpha.3 - 2026-05-05 UTC

### Added

- Automated stable-release publishing to Thunderstore from GitHub Actions:
    - Uses the Thunderstore API.
    - Reduces the need for manual artifact handling.

### Changed

- Refactored internal architecture to improve maintainability.
- Dropped backward compatibility with older CruiserJumpPractice versions:
    - Affects CruiserJumpPractice v0.1.4 and earlier.
    - Caused by a mod-internal `NetworkBehaviour` name change.
- Documented the tested Lethal Company version in the Thunderstore package
  description metadata and release checklist.
- Marked automated Thunderstore uploads with an additional Thunderstore
  category:
    - `AI Generated`
    - The current Thunderstore Lethal Company category API includes
      `AI Generated` with slug `ai-generated`:
        - <https://thunderstore.io/api/experimental/community/lethal-company/category/>
    - The current Lethal Company package list page describes the category as
      the place to disclose packages where a `significant portion` was created
      using AI tools:
        - <https://thunderstore.io/c/lethal-company/?included_categories=2086&ordering=last-updated&q=&section=mods>
    - This project uses the category to disclose AI assistance in project work;
      it is package metadata rather than a gameplay feature.
    - Issue #18 records the project decision to use this category because the
      applicable disclosure threshold is not clear.
    - Human maintainers have reviewed past AI-assisted changes and will
      continue reviewing future changes.
- Aligned Thunderstore manifest dependency strings with the documented v81.5
  test environment for BepInExPack and LethalCompany_InputUtils:
    - Treats the manifest dependency versions as part of the practical minimum
      runtime baseline for Thunderstore installs.
- Stopped carrying Lethal Company v73 and v56 compatibility notes forward into
  the unreleased v0.2.0 release notes because they no longer match the updated
  Thunderstore dependency baseline.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - Normally used together with Imperium; the v81.5 test environment
          used Imperium v1.3.0.
        - Imperium v1.3.0 appears to have some cruiser-related issues:
            - See
              <https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735>
              for a workaround.
- Test environment:
    - Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`)
    - BepInExPack v5.4.2305 (2026-03-17 UTC)
    - Imperium v1.3.0 (2026-04-08 UTC)
    - LethalCompany_InputUtils v0.7.13 (2026-03-31 UTC)
    - LethalNetworkAPI v3.3.3 (2026-04-02 UTC)
    - OdinSerializer v2024.2.2700 (2025-05-18 UTC)
    - BepInEx_MonoMod_Debug_Patcher v1.1.1 (2025-04-03 UTC)

## v0.2.0-alpha.2 - 2026-04-26 UTC

### Added

- Added repository-local Agent Skills and `AGENTS.md` guidance:
    - Commit message checks.
    - Pull request quality checks.
    - Agent workflow conventions.
- Added AI disclosure documentation to the repository README and Thunderstore
  package README for Thunderstore policy compliance.

### Changed

- Updated compile-time dependencies:
    - Lethal Company v73 to v81.5.
    - LethalCompany_InputUtils v0.7.13.
    - BepInEx.PluginInfoProps v2.1.0.
    - UnityEngine.Modules 2022.3.62.
- Replaced implicit restore package sources with explicit package source
  mapping:
    - Removed `RestoreAdditionalProjectSources`.
    - Added `nuget.config`.
    - Made dependency restores more deterministic.
- Clarified current-version interop naming:
    - Renamed interop adapters from `V73` to `Current`.
    - Removed game-version suffixes from reference aliases after confirming
      static multi-version validation is not practical with NuGet-managed
      package references.
- Simplified redundant network role guards:
    - RPC surrogate paths.
    - Frame-handling paths.
- Updated Thunderstore README compatibility language:
    - Focused the README on the latest stable Lethal Company version.
    - Withdrew the earlier alpha changelog wording that explicitly declared
      Lethal Company v73 support.
    - Kept Lethal Company v73 and v56 as best-effort compatibility notes in
      changelog context.
- Documented safer GitHub CLI pull request body handling:
    - Pass Markdown through body files.
    - Verify stored pull request bodies after creation.

### Fixed

- Fixed package source mapping for indirect dependencies after clean restore
  checks exposed missing mappings.

### Removed

- Removed unmaintained PowerShell scripts and stale references:
    - `Debug.ps1`
    - `InitProfiles.ps1`
    - Setup, debug, and Visual Studio launch-profile references.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - Normally used together with Imperium; the v81.5 test environment
          used Imperium v1.3.0.
    - Older base-game compatibility became best-effort after the project
      stopped pursuing static multi-version validation through NuGet
      references:
        - Lethal Company v73 still appeared to work.
        - Lethal Company v56 had lower-confidence compatibility, with a known
          minor issue found early.

## v0.2.0-alpha.1 - 2026-04-25 UTC

### Changed

- Refactored the runtime from a manager-centered structure into layered
  application architecture:
    - Added a composition root.
    - Added domain models and explicit use case result types.
    - Split client/server services and frame/startup handlers.
    - Later superseded by the stable-release roll-up and the follow-up
      v0.2.0-alpha.2 refactors.
- Replaced direct base game utility access with an `IGameInterop` abstraction
  and adapter layer so future game-version work can be isolated behind interop
  boundaries.
- Centralized in-game notifications through a notification use case for more
  consistent save, load, and magnet-toggle result handling.
- Split cruiser state and magnet behavior into explicit save/load/toggle use
  cases while preserving the existing user-facing gameplay flow.
- Changed the internal `NetworkBehaviour` and interop layout, making the
  v0.2.0-alpha.1 release not backward-compatible with CruiserJumpPractice
  v0.1.4 and earlier.

### Notes

- Compatibility:
    - Compatibility notes were first added in v0.2.0-alpha.1:
        - Earlier releases did not include compatibility information in this
          changelog.
        - Compatibility information for older releases was backfilled as
          reference material at the same time.
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Later withdrawn as explicit support wording in v0.2.0-alpha.2.
    - Lethal Company v56 has lower-confidence compatibility:
        - Core features appeared to work in limited checks.
        - A known minor issue was found early and is tracked in
          <https://github.com/aoirint/CruiserJumpPractice/issues/5>.
- Test environment:
    - Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`)
    - BepInExPack v5.4.2304 (2025-11-05 UTC)
    - Imperium v1.1.1 (2025-10-27 UTC)
    - LethalCompany_InputUtils v0.7.12 (2025-10-24 UTC)
    - LethalNetworkAPI v3.3.2 (2024-12-29 UTC)
    - OdinSerializer v2024.2.2700 (2025-05-18 UTC)

## v0.1.4 - 2025-11-30 UTC

### Changed

- Enabled keybind actions while the player is dead, same as Imperium.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0-alpha.1 release.
- Test environment:
    - Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`)
    - BepInExPack v5.4.2304 (2025-11-05 UTC)
    - Imperium v1.1.1 (2025-10-27 UTC)
    - LethalCompany_InputUtils v0.7.12 (2025-10-24 UTC)
    - LethalNetworkAPI v3.3.2 (2024-12-29 UTC)
    - OdinSerializer v2024.2.2700 (2025-05-18 UTC)

## v0.1.3 - 2025-11-30 UTC [YANKED]

### Notes

- Yanked release due to a build issue.
- No compatibility information was backfilled while preparing the
  v0.2.0-alpha.1 release because this release was yanked.

## v0.1.2 - 2025-11-29 UTC

### Changed

- Disabled keybind actions while the player is in a menu, using the terminal,
  typing in chat, or dead.

### Fixed

- Fixed an issue where the magnet lever on the ship wall did not reflect the
  magnet status when toggled via keybind.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0-alpha.1 release.

## v0.1.1 - 2025-11-29 UTC

### Changed

- Updated documentation.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0-alpha.1 release.

## v0.1.0 - 2025-11-29 UTC

### Added

- Initial Thunderstore release.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0-alpha.1 release.
