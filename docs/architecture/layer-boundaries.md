# Layer Boundaries

## Core

`Core` owns snapshot values, session stores, input and server use cases,
callback handlers, presentation messages, validation records, and port
interfaces. It defines busy-state suppression and the ordered load
preconditions.

Core depends on interfaces such as `IGameInterop`, `IPracticeInput`,
`IPluginConfig`, `IPluginLogger`, and `IValidationLogger`; it does not
reference Unity, Harmony, BepInEx, or Lethal Company types.

## Interop

`Interop` owns BepInEx configuration and logging, InputUtils bindings,
Harmony patch declarations, game and HUD adapters, Netcode surrogate behaviour,
and exception guards. It translates framework callbacks into
`PluginController` calls.

## Composition

`PluginController.Create()` is the composition root. It constructs adapters,
stores, server use cases, client request and presentation use cases, then
handlers. New game/framework dependencies should be represented by a Core port
with an Interop implementation; new practice policy belongs in a Core use case.

