#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop.Adapters.V73;

internal sealed class HudAdapterV73
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterV73 gameObjects;

    public HudAdapterV73(ManualLogSource logger, GameObjectAdapterV73 gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public void DisplayTip(HUDManager hudManager, string headerText, string bodyText)
    {
        try
        {
            hudManager.DisplayTip(headerText, bodyText);
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while displaying tip: {error}");
            throw new GameInteropException($"Exception while displaying tip: {error}");
        }
    }

    public void DisplayLocalTip(string headerText, string bodyText)
    {
        DisplayTip(gameObjects.GetHUDManager(), headerText, bodyText);
    }
}
