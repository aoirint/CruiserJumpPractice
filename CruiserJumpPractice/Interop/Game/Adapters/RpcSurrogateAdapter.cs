// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Interop.Game.Behaviours;

using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

internal sealed class RpcSurrogateAdapter
{
    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;
    private readonly IValidationLogger validationLogger;

    private RpcSurrogateBehaviour? cachedRpcSurrogateBehaviour;

    public RpcSurrogateAdapter(
        IPluginLogger logger,
        GameObjectAdapter gameObjects,
        IValidationLogger validationLogger
    )
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
        this.validationLogger = validationLogger;
    }

    public RpcSurrogateSpawnResult SpawnRpcSurrogate()
    {
        var hudManager = gameObjects.GetHUDManager();
        var gameObject = hudManager.gameObject;
        if (gameObject == null)
        {
            logger.LogError("HUDManager.gameObject is null.");
            return RpcSurrogateSpawnResult.Missing;
        }

        var rpcSurrogateNetworkBehaviour = gameObject.GetComponent<RpcSurrogateBehaviour>();
        if (rpcSurrogateNetworkBehaviour != null)
        {
            cachedRpcSurrogateBehaviour = rpcSurrogateNetworkBehaviour;
            logger.LogDebug("RPC surrogate already exists on HUDManager.");
            return RpcSurrogateSpawnResult.Reused;
        }

        cachedRpcSurrogateBehaviour = gameObject.AddComponent<RpcSurrogateBehaviour>();
        logger.LogInfo("Spawned RPC surrogate on HUDManager.");
        return RpcSurrogateSpawnResult.Added;
    }

    public RpcSurrogateBehaviour GetRpcSurrogateBehaviour()
    {
        if (cachedRpcSurrogateBehaviour != null)
        {
            RecordResolved("cache", "success");
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
            RecordResolved("lookup", "success");
            return rpcSurrogateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            RecordResolved("lookup", "error");
            logger.LogError($"Exception while getting RpcSurrogateBehaviour: {error}");
            throw new GameInteropException($"Exception while getting RpcSurrogateBehaviour: {error}");
        }
    }

    private void RecordResolved(string source, string result)
    {
        validationLogger.Record(
            "rpc_surrogate_resolved",
            new()
            {
                ["source"] = source,
                ["result"] = result
            }
        );
    }
}
