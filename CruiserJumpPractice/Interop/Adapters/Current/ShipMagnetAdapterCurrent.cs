#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Interop.Adapters.Current;

internal sealed class ShipMagnetAdapterCurrent
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterCurrent gameObjects;

    public ShipMagnetAdapterCurrent(ManualLogSource logger, GameObjectAdapterCurrent gameObjects)
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
