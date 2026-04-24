#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.GameInterop.Features;

internal sealed class NetworkInterop
{
    private readonly ManualLogSource logger;
    private readonly GameObjectInterop gameObjects;

    public NetworkInterop(ManualLogSource logger, GameObjectInterop gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public bool IsServer()
    {
        try
        {
            return gameObjects.GetNetworkManager().IsServer;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'IsServer': {error}");
            throw new GameInteropException($"Exception while getting 'IsServer': {error}");
        }
    }

    public bool IsClient()
    {
        try
        {
            return gameObjects.GetNetworkManager().IsClient;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'IsClient': {error}");
            throw new GameInteropException($"Exception while getting 'IsClient': {error}");
        }
    }

    public bool IsHost()
    {
        try
        {
            return gameObjects.GetNetworkManager().IsHost;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'IsHost': {error}");
            throw new GameInteropException($"Exception while getting 'IsHost': {error}");
        }
    }
}
