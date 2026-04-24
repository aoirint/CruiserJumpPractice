#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.GameInterop.Features;

internal sealed class RpcSurrogateInterop
{
    private readonly ManualLogSource logger;
    private readonly GameObjectInterop gameObjects;

    private RpcSurrogateNetworkBehaviour? cachedRpcSurrogateNetworkBehaviour;

    public RpcSurrogateInterop(ManualLogSource logger, GameObjectInterop gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public void SpawnRpcSurrogate(HUDManager hudManager)
    {
        var gameObject = hudManager.gameObject;
        if (gameObject == null)
        {
            logger.LogError("HUDManager.gameObject is null.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameObject.GetComponent<RpcSurrogateNetworkBehaviour>();
        if (rpcSurrogateNetworkBehaviour != null)
        {
            cachedRpcSurrogateNetworkBehaviour = rpcSurrogateNetworkBehaviour;
            logger.LogDebug("RPC surrogate already exists on HUDManager.");
            return;
        }

        cachedRpcSurrogateNetworkBehaviour = gameObject.AddComponent<RpcSurrogateNetworkBehaviour>();
        logger.LogInfo("Spawned RPC surrogate on HUDManager.");
    }

    public RpcSurrogateNetworkBehaviour GetRpcSurrogateNetworkBehaviour()
    {
        if (cachedRpcSurrogateNetworkBehaviour != null)
        {
            return cachedRpcSurrogateNetworkBehaviour;
        }

        try
        {
            var rpcSurrogateNetworkBehaviour =
                gameObjects.GetHUDManager().GetComponent<RpcSurrogateNetworkBehaviour>();
            if (rpcSurrogateNetworkBehaviour == null)
            {
                throw new GameInteropException(
                    "RpcSurrogateNetworkBehaviour component not found on HUDManager instance."
                );
            }

            cachedRpcSurrogateNetworkBehaviour = rpcSurrogateNetworkBehaviour;
            return rpcSurrogateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting RpcSurrogateNetworkBehaviour: {error}");
            throw new GameInteropException($"Exception while getting RpcSurrogateNetworkBehaviour: {error}");
        }
    }
}
