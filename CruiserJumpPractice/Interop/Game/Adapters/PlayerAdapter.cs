// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

internal sealed class PlayerAdapter
{
    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;

    public PlayerAdapter(IPluginLogger logger, GameObjectAdapter gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public LocalPlayerBusyState GetLocalPlayerBusyState()
    {
        var localPlayer = gameObjects.GetLocalPlayer();
        try
        {
            var quickMenuManager = localPlayer.quickMenuManager;
            if (quickMenuManager == null)
            {
                throw new GameInteropException("quickMenuManager is null.");
            }

            return new LocalPlayerBusyState(
                quickMenuManager.isMenuOpen,
                localPlayer.inTerminalMenu,
                localPlayer.isTypingChat
            );
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting local player status: {error}");
            throw new GameInteropException($"Exception while getting local player status: {error}");
        }
    }
}
