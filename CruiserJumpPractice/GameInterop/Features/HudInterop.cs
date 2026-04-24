#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.GameInterop.Features;

internal sealed class HudInterop
{
    private readonly ManualLogSource logger;
    private readonly GameObjectInterop gameObjects;

    public HudInterop(ManualLogSource logger, GameObjectInterop gameObjects)
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
