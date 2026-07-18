# Cruiser State

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

### `VehicleController`

| Member | Declaration | Role |
| --- | --- | --- |
| Physics body | `public Rigidbody mainRigidbody` | Source of the live vehicle position, rotation, and velocity. |
| Steering | `public Vector2 moveInputVector` | Its `x` component is the stored steering input. |
| Engine speed | `public float EngineRPM` | Stored engine-speed value. |
| Health | `public int carHP` | Stored and restored through the oil helpers. |
| Turbo count | `private int turboBoosts` | Reflection target: `GetField("turboBoosts", BindingFlags.NonPublic \| BindingFlags.Instance)`. |
| Magnet state | `public bool magnetedToShip` | Do not restore while true. |
| Oil apply | `public void AddEngineOilOnLocalClient(int setCarHP)` | Local health application. |
| Oil request | `public void AddEngineOilServerRpc(int playerId, int setHP)` | Network request for health application. |
| Oil receive | `public void AddEngineOilClientRpc(int playerId, int setHP)` | Receiver-side health application; patch target for client observation. |
| Turbo apply | `public void AddTurboBoostOnLocalClient(int setTurboBoosts)` | Local turbo-count application. |
| Turbo request | `public void AddTurboBoostServerRpc(int playerId, int setTurboBoosts)` | Network request for turbo application. |
| Turbo receive | `public void AddTurboBoostClientRpc(int playerId, int setTurboBoosts)` | Receiver-side turbo application; patch target for client observation. |
| Position request | `public void SyncCarPositionServerRpc(Vector3 carPosition, Vector3 carRotation, float steeringInput, float EngineRPM)` | Owner-gated request for vehicle transform and driving values. |
| Position receive | `public void SyncCarPositionClientRpc(Vector3 carPosition, Vector3 carRotation, float steeringInput, float engineSpeed)` | Stores non-owner synchronization targets; `FixedUpdate` moves toward the received transform later. |

## Implementation choices

### Find the cruiser

#### Resolve the live `VehicleController` instance — recommended

Snapshot and restore targets are members of that instance. Resolving the live
instance avoids using a stale reference to a destroyed or replaced vehicle.

#### Cache a previous instance or infer the vehicle from ship state

Neither approach establishes that the reference is the current
`VehicleController` being updated by the game.

### Restore transform and driving values

#### Assign the live transform, `moveInputVector.x`, and `EngineRPM` directly — recommended

These are the current local values used by the vehicle update path. The
base-game owner path later synchronizes transform targets, steering animation,
and engine speed through the `SyncCarPosition` RPC pair; direct assignment
restores the captured state immediately on the live instance. This normal
follow-up applies only while the local restoring client owns the
`VehicleController`; `SyncCarPositionServerRpc` rejects a non-owner, so a
non-owner restore requires a supported ownership or synchronization strategy.

#### Send `SyncCarPositionServerRpc(...)` or wait for a later vehicle update

The base-game RPC represents this snapshot state but is owner-gated and follows
the vehicle's normal update cadence. Delaying does not itself apply saved
values, and an explicit request is not the same as restoring the current local
instance immediately.

### Restore health and turbo

#### Call the local helper and matching server RPC — recommended

The helper applies the local value immediately, while the RPC carries the
corresponding network request. Both stages are represented by separate base
methods.

#### Assign fields only

Direct assignment bypasses the base helper and its network request.

#### Call only the local helper or only the server RPC

Either alternative covers only one of the local-application and network-request
stages.

### Read turbo count

#### Reflect the private `turboBoosts` field with non-public instance binding — recommended

It is the stored base-game value and gives the field identity required for
save and restore.

#### Assume a public member or infer the value from the HUD

The count is private, and HUD state is a presentation rather than the stored
field required by the implementation.

### Restore near the ship magnet

#### Reject while `magnetedToShip` and observe magnet methods — recommended

The base controller moves a magneted vehicle, so restoration must not write
snapshot values during that attachment state.

#### Wait for an arbitrary delay

A delay does not establish that the attachment state has ended.

## Snapshot and restore boundary

A durable snapshot can contain `transform.position`, `transform.eulerAngles`,
`moveInputVector.x`, `EngineRPM`, `carHP`, and `turboBoosts`. `magnetedToShip`
and the ship's attachment state are live
relationships, not values to restore.

Apply transform, steering, and engine speed to the live `VehicleController`.
Apply health and turbo count through both their local helper and server RPC,
using the current local player's ID for the request. The local helper, outgoing
`ServerRpc`, and incoming `ClientRpc` are distinct boundaries: patch the helper
for local application, the server RPC for the request, and the client RPC for
receiver-side observation.

`VehicleController` moves a magneted vehicle during its fixed update. Check
`magnetedToShip` before restore, and track `StartOfRound.SetMagnetOn(bool)` /
`SetMagnetOnClientRpc(bool)` when a restore must be delayed around magnet use.

## Change checklist

1. Bind reflection to the private instance `int turboBoosts` field.
2. Keep local-helper, server-RPC, and client-RPC patches distinct; their
   signatures and roles differ.
3. Do not write transform or snapshot values while `magnetedToShip` is true.
4. After applying values, read `carHP` and `turboBoosts` from the same live
   `VehicleController` instance.
