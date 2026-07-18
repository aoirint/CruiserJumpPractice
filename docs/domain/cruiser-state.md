# Cruiser State

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

| Type | Member | Declaration | Use |
| --- | --- | --- | --- |
| `VehicleController` | Physics body | `public Rigidbody mainRigidbody` | Source of the live vehicle position, rotation, and velocity. |
| `VehicleController` | Steering | `public Vector2 moveInputVector` | Its `x` component is the stored steering input. |
| `VehicleController` | Engine speed | `public float EngineRPM` | Stored engine-speed value. |
| `VehicleController` | Health | `public int carHP` | Stored and restored through the oil helpers. |
| `VehicleController` | Turbo count | `private int turboBoosts` | Reflection target: `GetField("turboBoosts", BindingFlags.NonPublic | BindingFlags.Instance)`. |
| `VehicleController` | Magnet state | `public bool magnetedToShip` | Do not restore while true. |
| `VehicleController` | Oil apply | `public void AddEngineOilOnLocalClient(int setCarHP)` | Local health application. |
| `VehicleController` | Oil request | `public void AddEngineOilServerRpc(int playerId, int setHP)` | Network request for health application. |
| `VehicleController` | Turbo apply | `public void AddTurboBoostOnLocalClient(int setTurboBoosts)` | Local turbo-count application. |
| `VehicleController` | Turbo request | `public void AddTurboBoostServerRpc(int playerId, int setTurboBoosts)` | Network request for turbo application. |
| `StartOfRound` | Magnet toggle | `public void SetMagnetOn(bool on)` | Patch target for ship-magnet transitions. |
| `StartOfRound` | Magnet sync | `public void SetMagnetOnClientRpc(bool on)` | Client-side magnet-toggle application. |

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
