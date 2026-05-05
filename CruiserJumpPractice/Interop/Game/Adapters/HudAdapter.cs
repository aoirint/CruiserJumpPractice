// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

internal sealed class HudAdapter
{
    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;

    public HudAdapter(IPluginLogger logger, GameObjectAdapter gameObjects)
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
