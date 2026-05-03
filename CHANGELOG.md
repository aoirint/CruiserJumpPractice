<!-- SPDX-License-Identifier: Unlicense -->

# Changelog

All notable changes to this project are documented in this file.

This changelog is the canonical developer-facing release history. The
Thunderstore-facing package changelog in `assets/CHANGELOG.md` is derived from
stable release entries in this file and rewritten for users.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

### Added

- Automated stable-release publishing to Thunderstore from GitHub Actions using
  the Thunderstore API, reducing the need for manual artifact handling.

### Changed

- Refactored internal architecture to improve maintainability.
- Dropped backward compatibility with CruiserJumpPractice v0.1.4 and earlier
  because the internal NetworkBehaviour name changed.
- Marked automated Thunderstore uploads with the `AI Generated`, `Mods`, and
  `Tweaks & Quality Of Life` Thunderstore categories.

### Notes

- Compatibility:
  - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
    `6423525044216269478`).
  - Lethal Company v73 still appears to work.
  - Lethal Company v56 mostly works, with a known minor issue tracked in
    <https://github.com/aoirint/CruiserJumpPractice/issues/5>.
  - Imperium v1.3.0 appears to have some cruiser-related issues; see
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

## v0.1.4 - 2025-11-30 UTC

### Changed

- Enabled keybind actions while the player is dead, same as Imperium.

### Notes

- Compatibility:
  - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
    `1749099131234587692`).
- Test environment:
  - Lethal Company v73 (2025-10-04 UTC, Manifest ID:
    `1749099131234587692`)
  - BepInExPack v5.4.2304 (2025-11-05 UTC)
  - Imperium v1.1.1 (2025-10-27 UTC)
  - LethalCompany_InputUtils v0.7.12 (2025-10-24 UTC)
  - LethalNetworkAPI v3.3.2 (2024-12-29 UTC)
  - OdinSerializer v2024.2.2700 (2025-05-18 UTC)

## v0.1.3 - 2025-11-30 UTC [YANKED]

### Removed

- Yanked release due to a build issue.

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

## v0.1.1 - 2025-11-29 UTC

### Changed

- Updated documentation.

### Notes

- Compatibility:
  - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
    `1749099131234587692`).

## v0.1.0 - 2025-11-29 UTC

### Added

- Initial Thunderstore release.

### Notes

- Compatibility:
  - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
    `1749099131234587692`).
