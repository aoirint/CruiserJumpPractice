// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

// ShipMagnetAdapter exposes the ship magnet state and toggle action used by
// practice mode. Toggling goes through the game's lever animation because that
// path already sends the needed RPC.
internal sealed class ShipMagnetAdapter
{
    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;

    public ShipMagnetAdapter(IPluginLogger logger, GameObjectAdapter gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public bool IsShipMagnetOn()
    {
        try
        {
            return gameObjects.GetStartOfRound().magnetOn;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'magnetOn': {error}");
            throw new GameInteropException($"Exception while getting 'magnetOn': {error}");
        }
    }

    public void ToggleShipMagnet()
    {
        try
        {
            var magnetLever = gameObjects.GetStartOfRound().magnetLever;
            if (magnetLever == null)
            {
                throw new GameInteropException("StartOfRound.magnetLever is null.");
            }

            // NOTE: AnimatedObjectTrigger calls StartOfRound.SetMagnetOn and
            // sends a ServerRpc internally.
            magnetLever.TriggerAnimation(gameObjects.GetLocalPlayer());
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while toggling magnet: {error}");
            throw new GameInteropException($"Exception while toggling magnet: {error}");
        }
    }
}
