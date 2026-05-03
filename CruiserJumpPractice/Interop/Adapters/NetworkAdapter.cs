#nullable enable

using BepInEx.Logging;

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Interop.Adapters;

internal sealed class NetworkAdapter
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapter gameObjects;

    public NetworkAdapter(ManualLogSource logger, GameObjectAdapter gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
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
