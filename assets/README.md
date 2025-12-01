# CruiserJumpPractice

A Lethal Company mod that saves/loads cruiser position, rotation, and condition, and lets you toggle the magnet remotely.

This mod helps you practice cruiser jumps repeatedly without having to manually reset the cruiser every attempt.

This mod works for v73+. Maybe works for the earlier versions, but not tested.

## What it does

- Keybind to save the current cruiser state (position, rotation, HP, turbo boosts).
- Keybind to load the saved cruiser state.
- Keybind to toggle the magnet on/off remotely.

Only the host can use all features of this mod. Clients will still receive the synced cruiser state even without this mod installed.

[giosuel/Imperium](https://thunderstore.io/c/lethal-company/p/giosuel/Imperium/) is practically required.
This mod does not provide any way to instantly spawn cruiser, teleport player, or **restore destroyed cruiser**.

## Keybinds

You can change the keybinds from the [Rune580/LethalCompany_InputUtils](https://thunderstore.io/c/lethal-company/p/Rune580/LethalCompany_InputUtils/) settings menu.

- Load Cruiser State: `[` (US: `[`, JP109: `@`)
- Save Cruiser State: `]` (US: `]`, JP109: `[`)
- Toggle Magnet: `\` (US: `\`, JP109: `]`)

## Current Limitations

- Exploded cruisers cannot be restored. Use Imperium to respawn the cruiser and then load the saved state.

## Who needs to install

Host only.

Clients cannot use all features even if they install this mod.
