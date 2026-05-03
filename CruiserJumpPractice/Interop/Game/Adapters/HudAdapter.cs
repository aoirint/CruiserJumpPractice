// SPDX-License-Identifier: Unlicense
#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

internal sealed class HudAdapter
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapter gameObjects;

    public HudAdapter(ManualLogSource logger, GameObjectAdapter gameObjects)
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
