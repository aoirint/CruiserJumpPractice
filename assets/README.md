<!-- SPDX-License-Identifier: MIT -->

# CruiserJumpPractice

A Lethal Company mod that saves and loads cruiser position, rotation, and
condition, and lets you toggle the magnet remotely.

This mod helps you practice cruiser jumps repeatedly without having to manually
reset the cruiser every attempt.

## Compatibility

- Lethal Company v81.5 (2026-04-17 UTC, Manifest ID:
  `6423525044216269478`)
    - Test environment
        - BepInExPack v5.4.2305 (2026-03-17 UTC)
        - Imperium v1.3.0 (2026-04-08 UTC)
        - LethalCompany_InputUtils v0.7.13 (2026-03-31 UTC)
        - LethalNetworkAPI v3.3.3 (2026-04-02 UTC)
        - OdinSerializer v2024.2.2700 (2025-05-18 UTC)
        - BepInEx_MonoMod_Debug_Patcher v1.1.1 (2025-04-03 UTC)
    - NOTE: Imperium v1.3.0 appears to have some cruiser-related issues. See
      the [Imperium issue comment][imperium-cruiser-workaround] for a
      workaround.

## What it does

- ✅ Keybind to save the current cruiser state (position, rotation, HP, turbo boosts).
- ✅ Keybind to load the saved cruiser state.
- ✅ Keybind to toggle the magnet on/off remotely.

Only the host can use all features of this mod. Clients will still receive the
synced cruiser state even without this mod installed.

[giosuel/Imperium](https://thunderstore.io/c/lethal-company/p/giosuel/Imperium/)
is practically required.

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

## Current Limitations

- Exploded cruisers cannot be restored. Use Imperium to respawn the cruiser and
  then load the saved state.

## Who needs to install

Host only.

Clients cannot use all features even if they install this mod.

## AI Disclosure

Some parts of this project were developed with AI tools based on large language
models (LLMs), including agent-based tools.
The project maintainer reviews the code.
This disclosure is made in compliance with Thunderstore policies.

[imperium-cruiser-workaround]: https://github.com/giosuel/imperium/issues/153#issuecomment-4317402735
[input-utils]: https://thunderstore.io/c/lethal-company/p/Rune580/LethalCompany_InputUtils/
