#nullable enable

using BepInEx.Logging;
using Unity.Netcode;

namespace CruiserJumpPractice.BaseGame.Controllers;

class NetworkStateControllerException : System.Exception
{
    public NetworkStateControllerException(string message) : base(message) { }
}

class NetworkStateController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected NetworkManager networkManager;

    public NetworkStateController(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    public bool IsServer()
    {
        try
        {
            return networkManager.IsServer;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'IsServer': {error}");
            throw new NetworkStateControllerException($"Exception while getting 'IsServer': {error}");
        }
    }

    public bool IsHost()
    {
        try
        {
            return networkManager.IsHost;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'IsHost': {error}");
            throw new NetworkStateControllerException($"Exception while getting 'IsHost': {error}");
        }
    }

    public bool IsClient()
    {
        try
        {
            return networkManager.IsClient;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'IsClient': {error}");
            throw new NetworkStateControllerException($"Exception while getting 'IsClient': {error}");
        }
    }
}
