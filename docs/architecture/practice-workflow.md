# Practice Workflow

## Model

`CruiserSnapshot` is an immutable value for the saved vehicle state.
`CruiserStateStore` holds only the latest session-scoped snapshot;
it is neither persistent storage nor a multi-slot save system.
`CruiserRestoreObservation` records values around a restore for validation.

The base-game vehicle members and synchronization methods are documented in
[../domain/cruiser-state.md](../domain/cruiser-state.md). The ship magnet's
lever and synchronization path are documented in
[../domain/ship-magnet.md](../domain/ship-magnet.md).
The HUD hooks and transient result presentation are documented in
[../domain/hud-feedback.md](../domain/hud-feedback.md).

## Input and command flow

`FrameHandler` samples the three one-frame actions—save, load, and magnet
toggle—before checking `LocalPlayerBusyState`. A busy player suppresses each
requested action rather than dispatching a partial command.

Save and load are host-only. A non-host request is rejected locally with HUD
feedback. Accepted host input uses client request use cases to cross the
surrogate ServerRpc boundary; the server runs `SaveCruiserStateUseCase` or
`LoadCruiserStateUseCase`, then a result ClientRpc returns a compact result for
client-side presentation.

## Save and load policy

Saving captures a live cruiser and replaces the store's previous snapshot.
Loading rejects, in order: no cruiser, no saved snapshot, and a cruiser
magneted to the ship. Only after these checks does it restore the saved state.

The restore operation records a `CruiserRestoreObservation`; the client is
shown a result enum rather than server-side restore details. This keeps
presentation separate from authority and validation diagnostics.

## Magnet control

The magnet command is a separate client use case. Base-game magnet patches
feed validation observations but do not own the mod's input policy.
