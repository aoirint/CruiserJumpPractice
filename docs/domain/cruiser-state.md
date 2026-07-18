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
| Turbo count | `private int turboBoosts` | Reflection target: `GetField("turboBoosts", BindingFlags.NonPublic | BindingFlags.Instance)`. |
| Magnet state | `public bool magnetedToShip` | Do not restore while true. |
| Oil apply | `public void AddEngineOilOnLocalClient(int setCarHP)` | Local health application. |
| Oil request | `public void AddEngineOilServerRpc(int playerId, int setHP)` | Network request for health application. |
| Turbo apply | `public void AddTurboBoostOnLocalClient(int setTurboBoosts)` | Local turbo-count application. |
| Turbo request | `public void AddTurboBoostServerRpc(int playerId, int setTurboBoosts)` | Network request for turbo application. |

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Magnet toggle | `public void SetMagnetOn(bool on)` | Patch target for ship-magnet transitions. |
| Magnet sync | `public void SetMagnetOnClientRpc(bool on)` | Client-side magnet-toggle application. |

## Implementation choices

| Decision | Options | Recommended approach | Why |
| --- | --- | --- | --- |
| Find the cruiser | Cache a previous instance; search `VehicleController` instances when needed; infer it from ship state | Resolve the live `VehicleController` instance. | The snapshot and restore targets are members of that instance; a stale reference can refer to a destroyed or replaced vehicle. |
| Restore transform and driving values | Assign public members directly; send a position RPC; wait for a later vehicle update | Assign the live transform, `moveInputVector.x`, and `EngineRPM` directly. | These are the current local values used by the vehicle update path; the documented health and turbo RPCs do not carry transform state. |
| Restore health and turbo | Assign fields only; call local helpers only; call local helper and matching server RPC | Call the local helper and matching server RPC. | The helper applies the local value immediately, while the RPC carries the corresponding network request; either one alone covers only one stage. |
| Read turbo count | Assume a public member; reflect `turboBoosts`; infer it from the HUD | Reflect the private `turboBoosts` field with non-public instance binding. | It is the stored base-game value; the HUD is a presentation of that value and does not provide the field identity needed for save/restore. |
| Restore near the ship magnet | Restore unconditionally; reject while `magnetedToShip`; wait for an arbitrary delay | Reject while `magnetedToShip` and observe the magnet methods. | The base controller moves a magneted vehicle, so an arbitrary delay does not establish that the attachment state has ended. |

## Snapshot and restore boundary

A durable snapshot can contain `transform.position`, `transform.eulerAngles`,
`moveInputVector.x`, `EngineRPM`, `carHP`, and `turboBoosts`. `magnetedToShip`
and the ship's attachment state are live
relationships, not values to restore.

Apply transform, steering, and engine speed to the live `VehicleController`.
Apply health and turbo count through both their local helper and server RPC,
using the current local player's ID for the RPC methods. The local helper is a
separate patch point from the RPC: patch it when observing the effective local
value, and patch the RPC when observing or guarding the network request.

`VehicleController` moves a magneted vehicle during its fixed update. Check
`magnetedToShip` before restore, and track `StartOfRound.SetMagnetOn(bool)` /
`SetMagnetOnClientRpc(bool)` when a restore must be delayed around magnet use.

## Change checklist

1. Bind reflection to the private instance `int turboBoosts` field.
2. Keep local-helper and server-RPC patches distinct; their signatures differ.
3. Do not write transform or snapshot values while `magnetedToShip` is true.
4. After applying values, read `carHP` and `turboBoosts` from the same live
   `VehicleController` instance.
