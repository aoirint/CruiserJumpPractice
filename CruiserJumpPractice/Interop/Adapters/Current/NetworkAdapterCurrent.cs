#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop.Adapters.Current;

internal sealed class NetworkAdapterCurrent
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterCurrent gameObjects;

    public NetworkAdapterCurrent(ManualLogSource logger, GameObjectAdapterCurrent gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
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
