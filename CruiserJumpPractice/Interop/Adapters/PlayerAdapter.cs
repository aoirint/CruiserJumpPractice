#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Interop.Adapters;

internal sealed class PlayerAdapter
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapter gameObjects;

    public PlayerAdapter(ManualLogSource logger, GameObjectAdapter gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public bool IsLocalPlayerBusy()
    {
        var localPlayer = gameObjects.GetLocalPlayer();
        try
        {
            var quickMenuManager = localPlayer.quickMenuManager;
            if (quickMenuManager == null)
            {
                throw new GameInteropException("quickMenuManager is null.");
            }

            return quickMenuManager.isMenuOpen || localPlayer.inTerminalMenu || localPlayer.isTypingChat;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting local player status: {error}");
            throw new GameInteropException($"Exception while getting local player status: {error}");
        }
    }
}
