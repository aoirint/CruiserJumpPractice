// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

// PlayerAdapter exposes only local UI/input-blocking state to Core. Keeping the
// checks here avoids passing PlayerControllerB or menu objects across the port.
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
            // These three flags cover the base-game states where a practice key
            // press would conflict with text entry or menu navigation.
            var quickMenuManager = localPlayer.quickMenuManager;
            if (quickMenuManager == null)
            {
                throw new GameInteropException("quickMenuManager is null.");
            }

            return new LocalPlayerBusyState(
                isMenuOpen: quickMenuManager.isMenuOpen,
                isInTerminal: localPlayer.inTerminalMenu,
                isTypingChat: localPlayer.isTypingChat
            );
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting local player status: {error}");
            throw new GameInteropException($"Exception while getting local player status: {error}");
        }
    }
}
