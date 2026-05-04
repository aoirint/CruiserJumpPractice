<!-- SPDX-License-Identifier: Unlicense -->

## Unreleased

This is a maintenance release reflecting internal improvements.

No gameplay changes are introduced.

### Changed

- Dropped backward compatibility with older CruiserJumpPractice versions:
    - Affects CruiserJumpPractice v0.1.4 and earlier.
    - Caused by an internal change.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
    - Imperium v1.3.0 appears to have some cruiser-related issues:
        - See [this issue comment][imperium-cruiser-workaround] for a
          workaround.
    - Lethal Company v73: It still seems to work.
    - Lethal Company v56: Major features work as expected, but there is a minor
      known issue: <https://github.com/aoirint/CruiserJumpPractice/issues/5>.

## v0.1.4 - 2025-11-30 UTC

### Changed

- Enables keybind actions while the player is dead, same as Imperium.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information for the v0.2.0
          release timing.

## v0.1.3 - 2025-11-30 UTC [YANKED]

### Notes

- Yanked release due to a build issue.
- No compatibility information was backfilled because this release was yanked.

## v0.1.2 - 2025-11-29 UTC

### Changed

- Disables keybind actions while the player is in a menu, using the terminal,
  typing in chat, or is dead.

### Fixed

- Fixed an issue where the magnet lever on the ship wall did not reflect the
  magnet status when toggled via keybind.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information for the v0.2.0
          release timing.

## v0.1.1 - 2025-11-29 UTC

### Changed

- Updated documentation.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information for the v0.2.0
          release timing.

## v0.1.0 - 2025-11-29 UTC

### Added

- Initial release on Thunderstore.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information for the v0.2.0
          release timing.

[imperium-cruiser-workaround]: https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735
