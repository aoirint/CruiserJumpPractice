<!-- SPDX-License-Identifier: MIT -->

This changelog is the user-facing release notes for Thunderstore.

For internal implementation details and developer-facing release history, see
the [GitHub changelog][github-changelog].

If you find a release-note error, encounter a bug, or want to report another
project issue, see [CONTRIBUTING.md][contributing], then report it in
[GitHub Issues][github-issues].

## v0.2.0 - 2026-05-06 UTC

This release rebuilds CruiserJumpPractice for Lethal Company v81.5 and includes
internal improvements.

No gameplay changes are introduced.

### Changed

- Rebuilt for Lethal Company v81.5.
- Improved internal implementation structure and release flow.
- Added the Thunderstore `AI Generated` category to the package metadata:
    - The Lethal Company Thunderstore community currently provides this
      category for authors to disclose when a `significant portion` of a mod
      was created using AI tools.
    - This project uses the category to disclose AI assistance in project work;
      it is package metadata rather than a gameplay feature.
    - The project decided to use this category because the applicable
      disclosure threshold is not clear.
    - Human maintainer review remains the project policy.
- Dropped backward compatibility with older CruiserJumpPractice versions:
    - Affects CruiserJumpPractice v0.1.4 and earlier.
    - Caused by a mod-internal change.

### Notes

- Compatibility:
    - Compatible with Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
      `6423525044216269478`).
        - Normally used together with Imperium; the v81.5 test environment
          used Imperium v1.3.0.
        - Imperium v1.3.0 appears to have some cruiser-related issues:
            - See the [Imperium issue comment][imperium-cruiser-workaround]
              for a workaround.

## v0.1.4 - 2025-11-30 UTC

### Changed

- Enables keybind actions while the player is dead, matching Imperium.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.3 - 2025-11-30 UTC [YANKED]

### Notes

- Yanked release due to a build issue.
- No compatibility information was backfilled while preparing the v0.2.0
  release because this release was yanked.

## v0.1.2 - 2025-11-29 UTC

### Changed

- Disables keybind actions while the player is in a menu, using the terminal,
  typing in chat, or dead.

### Fixed

- Fixed an issue where the magnet lever on the ship wall did not reflect the
  magnet status when toggled via keybind.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.1 - 2025-11-29 UTC

### Changed

- Updated documentation.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

## v0.1.0 - 2025-11-29 UTC

### Added

- Initial release on Thunderstore.

### Notes

- Compatibility:
    - Compatible with Lethal Company v73 (2025-10-04 UTC, Manifest ID:
      `1749099131234587692`).
        - Backfilled as reference compatibility information while preparing
          the v0.2.0 release.

[imperium-cruiser-workaround]: https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735
[contributing]: https://github.com/aoirint/CruiserJumpPractice/blob/main/CONTRIBUTING.md
[github-changelog]: https://github.com/aoirint/CruiserJumpPractice/blob/main/CHANGELOG.md
[github-issues]: https://github.com/aoirint/CruiserJumpPractice/issues
