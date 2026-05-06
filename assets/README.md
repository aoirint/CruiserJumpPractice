<!-- SPDX-License-Identifier: MIT -->

# CruiserJumpPractice

A [Lethal Company][lethal-company] mod that saves and loads cruiser position,
rotation, and condition, and lets you toggle the magnet remotely.

This mod helps you practice cruiser jumps repeatedly without having to manually
reset the cruiser every attempt.

## Compatibility

- [Lethal Company][lethal-company] v81.5 (2026-04-17 UTC, Manifest ID:
  `6423525044216269478`)
    - Test environment
        - [BepInExPack][bepinexpack] v5.4.2305 (2026-03-17 UTC)
        - [Imperium][imperium] v1.3.0 (2026-04-08 UTC)
        - [LethalCompany_InputUtils][input-utils] v0.7.13 (2026-03-31 UTC)
        - [LethalNetworkAPI][lethal-network-api] v3.3.3 (2026-04-02 UTC)
        - [OdinSerializer][odin-serializer] v2024.2.2700 (2025-05-18 UTC)
        - [BepInEx_MonoMod_Debug_Patcher][bepinex-monomod-debug-patcher]
          v1.1.1 (2025-04-03 UTC)
    - NOTE: Imperium v1.3.0 appears to have some cruiser-related issues. See
      the [Imperium issue comment][imperium-cruiser-workaround] for a
      workaround.

## What it does

- ✅ Keybind to save the current cruiser state (position, rotation, HP, turbo boosts).
- ✅ Keybind to load the saved cruiser state.
- ✅ Keybind to toggle the magnet on/off remotely.

Only the host can use all features of this mod. Clients will still receive the
synced cruiser state even without this mod installed.

[giosuel/Imperium][imperium] is practically required.

Important: this mod **does not** provide any way to:

- ❌ Instantly spawn a cruiser.
- ❌ Teleport a player.
- ❌ **Restore a destroyed cruiser**.

## Keybinds

You can change the keybinds from the
[Rune580/LethalCompany_InputUtils][input-utils] settings menu.

- Load Cruiser State: `[` (US: `[`, JP109: `@`)
- Save Cruiser State: `]` (US: `]`, JP109: `[`)
- Toggle Magnet: `\` (US: `\`, JP109: `]`)

## Known Issues and Limitations

- Known multiplayer issue:
    - When a guest client is driving and the host loads a saved cruiser state,
      the cruiser may briefly load on the host and then snap back. This is
      tracked in
      [GitHub issue #113](https://github.com/aoirint/CruiserJumpPractice/issues/113).
- Gear position is not saved or restored yet:
    - Park, Drive, and Reverse are not included in the saved cruiser state in
      this release. Gear save/load support is tracked in
      [GitHub issue #114](https://github.com/aoirint/CruiserJumpPractice/issues/114).

## Who needs to install

Host only.

Clients cannot use all features even if they install this mod.

## AI Disclosure

Some parts of this project were developed with AI tools based on large language
models (LLMs), including agent-based tools.
The project maintainer reviews the code.
This disclosure is made in compliance with [Thunderstore policies][thunderstore].

[imperium-cruiser-workaround]: https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735
[bepinex-monomod-debug-patcher]: https://thunderstore.io/c/lethal-company/p/BepInEx/BepInEx_MonoMod_Debug_Patcher/
[bepinexpack]: https://thunderstore.io/c/lethal-company/p/BepInEx/BepInExPack/
[imperium]: https://thunderstore.io/c/lethal-company/p/giosuel/Imperium/
[input-utils]: https://thunderstore.io/c/lethal-company/p/Rune580/LethalCompany_InputUtils/
[lethal-company]: https://store.steampowered.com/app/1966720/Lethal_Company/
[lethal-network-api]: https://thunderstore.io/c/lethal-company/p/xilophor/LethalNetworkAPI/
[odin-serializer]: https://thunderstore.io/c/lethal-company/p/Lordfirespeed/OdinSerializer/
[thunderstore]: https://thunderstore.io/
