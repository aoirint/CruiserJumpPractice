#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Interop.Adapters.Current;

internal sealed class HudAdapterCurrent
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterCurrent gameObjects;

    public HudAdapterCurrent(ManualLogSource logger, GameObjectAdapterCurrent gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public void DisplayTip(string headerText, string bodyText)
    {
        var hudManager = gameObjects.GetHUDManager();

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
}
