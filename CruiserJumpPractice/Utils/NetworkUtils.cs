#nullable enable

using BepInEx.Logging;
using Unity.Netcode;

namespace CruiserJumpPractice.Utils;

internal static class NetworkUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public static bool IsServer()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsServer;
    }

    public static bool IsHost()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsHost;
    }

    public static bool IsClient()
    {
        var networkManager = NetworkManager.Singleton;
        if (networkManager == null)
        {
            Logger.LogError("NetworkManager.Singleton is null.");
            return false;
        }

        return networkManager.IsClient;
    }
}
