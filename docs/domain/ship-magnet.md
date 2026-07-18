# Ship Magnet

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

### `StartOfRound`

| Member | Declaration | Role |
| --- | --- | --- |
| Magnet flag | `public bool magnetOn` | Current ship-magnet state. |
| Magnet lever | `public AnimatedObjectTrigger magnetLever` | The game-owned interaction surface for a magnet toggle. |
| Magnet toggle | `public void SetMagnetOn(bool on)` | Applies the local magnet-state transition and initiates synchronization. |
| Magnet sync | `public void SetMagnetOnClientRpc(bool on)` | Applies the synchronized magnet state on clients. |

### `AnimatedObjectTrigger`

| Member | Declaration | Role |
| --- | --- | --- |
| Lever interaction | `public void TriggerAnimation(PlayerControllerB playerWhoTriggered)` | Executes the lever interaction that reaches the magnet toggle and its network path. |

### `VehicleController`

| Member | Declaration | Role |
| --- | --- | --- |
| Ship attachment | `public bool magnetedToShip` | Indicates that the vehicle is currently attached to the ship magnet. |

## Behaviour and lifecycle

`StartOfRound.SetMagnetOn(bool)` changes `magnetOn` when the requested state is
different and sends the matching server RPC. The server reaches
`SetMagnetOnClientRpc(bool)` so connected clients apply the same value.

`AnimatedObjectTrigger.TriggerAnimation(PlayerControllerB)` is the game-owned
lever interaction path. Use it when a mod needs to perform the same action as
a player rather than duplicate the state and RPC sequence.

While `VehicleController.magnetedToShip` is true, the vehicle update path
continues to move the vehicle around the magnet. A system that writes vehicle
transform state must observe this attachment state and wait until it ends.

## Change checklist

1. Patch the exact `SetMagnetOn(bool)` and `SetMagnetOnClientRpc(bool)`
   signatures when observing local and synchronized transitions.
2. Resolve `StartOfRound.magnetLever` and the local `PlayerControllerB` before
   calling `TriggerAnimation`.
3. Treat a missing lever or player as an unavailable game interaction, not as a
   successful toggle.
4. Check `VehicleController.magnetedToShip` before changing vehicle transform
   state near the magnet.
