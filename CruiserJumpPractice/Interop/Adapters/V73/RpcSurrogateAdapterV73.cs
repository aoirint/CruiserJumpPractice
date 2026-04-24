#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Interop.Behaviours;

using CruiserJumpPractice.Interop.Domain;

namespace CruiserJumpPractice.Interop.Adapters.V73;

internal sealed class RpcSurrogateAdapterV73
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapterV73 gameObjects;

    private RpcSurrogateBehaviour? cachedRpcSurrogateBehaviour;

    public RpcSurrogateAdapterV73(ManualLogSource logger, GameObjectAdapterV73 gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public void SpawnRpcSurrogate()
    {
        var hudManager = gameObjects.GetHUDManager();
        var gameObject = hudManager.gameObject;
        if (gameObject == null)
        {
            logger.LogError("HUDManager.gameObject is null.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameObject.GetComponent<RpcSurrogateBehaviour>();
        if (rpcSurrogateNetworkBehaviour != null)
        {
            cachedRpcSurrogateBehaviour = rpcSurrogateNetworkBehaviour;
            logger.LogDebug("RPC surrogate already exists on HUDManager.");
            return;
        }

        cachedRpcSurrogateBehaviour = gameObject.AddComponent<RpcSurrogateBehaviour>();
        logger.LogInfo("Spawned RPC surrogate on HUDManager.");
    }

    public RpcSurrogateBehaviour GetRpcSurrogateBehaviour()
    {
        if (cachedRpcSurrogateBehaviour != null)
        {
            return cachedRpcSurrogateBehaviour;
        }

        try
        {
            var rpcSurrogateNetworkBehaviour =
                gameObjects.GetHUDManager().GetComponent<RpcSurrogateBehaviour>();
            if (rpcSurrogateNetworkBehaviour == null)
            {
                throw new GameInteropException(
                    "RpcSurrogateBehaviour component not found on HUDManager instance."
                );
            }

            cachedRpcSurrogateBehaviour = rpcSurrogateNetworkBehaviour;
            return rpcSurrogateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting RpcSurrogateBehaviour: {error}");
            throw new GameInteropException($"Exception while getting RpcSurrogateBehaviour: {error}");
        }
    }
}
