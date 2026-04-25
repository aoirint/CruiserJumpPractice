## Unreleased

This is a maintenance release reflecting internal improvements. No functional or compatibility changes are introduced, so you likely don’t need to update immediately, but staying up to date is recommended.

### Changed

- Drops backward compatibility with CruiserJumpPractice v0.1.4 and earlier.
    - The internal NetworkBehaviour name has been changed.
- Explicitly declare support for Lethal Company v81 (2026-04-17 UTC, Manifest ID: `6423525044216269478`).
    - Test environment
        - BepInExPack v5.4.2305 (2026-03-17 UTC)
        - Imperium v1.3.0 (2026-04-08 UTC)
        - LethalCompany_InputUtils v0.7.13 (2026-03-31 UTC)
        - LethalNetworkAPI v3.3.3 (2026-04-02 UTC)
        - OdinSerializer v2024.2.2700 (2025-05-18 UTC)
        - BepInEx_MonoMod_Debug_Patcher v1.1.1 (2025-04-03 UTC)
    - NOTE: Imperium v1.3.0 appears to have some cruiser-related issues. See this issue comment for a workaround: https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735
- Explicitly declare support for Lethal Company v73 (2025-10-04 UTC, Manifest ID: `1749099131234587692`).
    - Test environment
        - BepInExPack v5.4.2305 (2026-03-17 UTC)
        - Imperium v1.1.1 (2025-10-27 UTC)
        - LethalCompany_InputUtils v0.7.12 (2025-10-24 UTC)
        - LethalNetworkAPI v3.3.2 (2024-12-29 UTC)
        - OdinSerializer v2024.2.2700 (2025-05-18 UTC)
- Refactored internal architecture to improve maintainability.

## v0.1.4 - 2025-11-30 UTC

### Changed

- Enables keybind actions while the player is dead, same as Imperium.

## v0.1.3 - 2025-11-30 UTC

Yanked release due to a build issue.

## v0.1.2 - 2025-11-29 UTC

### Changed

- Disables keybind actions while the player is in a menu, using the terminal, typing in chat, or is dead.

### Fixed

- Fixed an issue where the magnet lever on the ship wall did not reflect the magnet status when toggled via keybind.

## v0.1.1 - 2025-11-29 UTC

### Changed

- Updated documentation.

## v0.1.0 - 2025-11-29 UTC

### Added

- Initial release on Thunderstore.
