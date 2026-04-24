#nullable enable

using BepInEx.Logging;
using Unity.Netcode;

namespace CruiserJumpPractice.BaseGame.Finders;

class NetworkManagerFinderException : System.Exception
{
    public NetworkManagerFinderException(string message) : base(message) { }
}

class NetworkManagerFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public NetworkManager GetNetworkManager()
    {
        try
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null)
            {
                throw new NetworkManagerFinderException("NetworkManager.Singleton is null.");
            }

            return networkManager;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting NetworkManager: {error}");
            throw new NetworkManagerFinderException($"Exception while getting NetworkManager: {error}");
        }
    }
}
