#nullable enable

using BepInEx.Logging;
using GameNetcodeStuff;

namespace CruiserJumpPractice.Utils;

internal static class PlayerUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal static GameNetworkManager? GetGameNetworkManager()
    {
        var gameNetworkManager = GameNetworkManager.Instance;
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager.Instance is null.");
            return null;
        }

        return gameNetworkManager;
    }

    internal static PlayerControllerB? GetLocalPlayer()
    {
        var gameNetworkManager = GetGameNetworkManager();
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager is null.");
            return null;
        }

        var localPlayer = gameNetworkManager.localPlayerController;
        if (localPlayer == null)
        {
            Logger.LogError("localPlayerController is null.");
            return null;
        }

        return localPlayer;
    }

    internal static bool IsLocalPlayerBusy()
    {
        if (IsLocalPlayerMenuOpen() == true)
        {
            return true;
        }

        if (IsLocalPlayerInTerminalMenu() == true)
        {
            return true;
        }

        if (IsLocalPlayerTypingChat() == true)
        {
            return true;
        }

        return false;
    }

    internal static bool? IsLocalPlayerInTerminalMenu()
    {
        var localPlayer = GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return null;
        }

        return localPlayer.inTerminalMenu;
    }

    internal static bool? IsLocalPlayerTypingChat()
    {
        var localPlayer = GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return null;
        }

        return localPlayer.isTypingChat;
    }

    internal static QuickMenuManager? GetLocalPlayerQuickMenuManager()
    {
        var localPlayer = GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return null;
        }

        var quickMenuManager = localPlayer.quickMenuManager;
        if (quickMenuManager == null)
        {
            Logger.LogError("quickMenuManager is null.");
            return null;
        }

        return quickMenuManager;
    }

    internal static bool? IsLocalPlayerMenuOpen()
    {
        var quickMenuManager = GetLocalPlayerQuickMenuManager();
        if (quickMenuManager == null)
        {
            Logger.LogError("QuickMenuManager is null.");
            return null;
        }

        return quickMenuManager.isMenuOpen;
    }
}
