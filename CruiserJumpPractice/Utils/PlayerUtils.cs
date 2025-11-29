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
}
