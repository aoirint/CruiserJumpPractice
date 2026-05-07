#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

internal sealed class NetworkAdapter
{
    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;

    public NetworkAdapter(IPluginLogger logger, GameObjectAdapter gameObjects)
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
