#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop.Domain;

namespace CruiserJumpPractice.Interop.Adapters.V73;

internal sealed class ShipMagnetAdapterV73
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterV73 gameObjects;

    public ShipMagnetAdapterV73(ManualLogSource logger, GameObjectAdapterV73 gameObjects)
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

            // NOTE: This AnimatedObjectTrigger method calls StartOfRound.SetMagnetOn and sends a ServerRpc internally.
            magnetLever.TriggerAnimation(gameObjects.GetLocalPlayer());
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while toggling magnet: {error}");
            throw new GameInteropException($"Exception while toggling magnet: {error}");
        }
    }
}
