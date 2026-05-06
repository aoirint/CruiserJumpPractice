// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Validation;
using CruiserJumpPractice.Interop.Game.Behaviours;

using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

/// <summary>
/// Manages the NetworkBehaviour bridge used for practice save/load RPCs.
/// </summary>
/// <remarks>
/// The RPC surrogate lives on HUDManager because it is available on clients and
/// survives long enough to host the NetworkBehaviour bridge.
/// </remarks>
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
        try
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
        catch (System.Exception error)
        {
            logger.LogError($"Exception while spawning RPC surrogate: {error}");
            return RpcSurrogateSpawnResult.Error;
        }
    }

    public RpcSurrogateBehaviour GetRpcSurrogateBehaviour()
    {
        // Startup normally seeds the cache. The lookup fallback keeps input
        // handling resilient if Unity recreated the component or startup order
        // differs during validation.
        if (cachedRpcSurrogateBehaviour != null)
        {
            RecordResolved(
                source: ValidationLogRpcSurrogateResolveSource.Cache,
                result: ValidationLogRpcSurrogateResolveResult.Success
            );
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
            RecordResolved(
                source: ValidationLogRpcSurrogateResolveSource.Lookup,
                result: ValidationLogRpcSurrogateResolveResult.Success
            );
            return rpcSurrogateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            RecordResolved(
                source: ValidationLogRpcSurrogateResolveSource.Lookup,
                result: ValidationLogRpcSurrogateResolveResult.Error
            );
            logger.LogError($"Exception while getting RpcSurrogateBehaviour: {error}");
            throw new GameInteropException($"Exception while getting RpcSurrogateBehaviour: {error}");
        }
    }

    private void RecordResolved(
        ValidationLogRpcSurrogateResolveSource source,
        ValidationLogRpcSurrogateResolveResult result
    )
    {
        validationLogger.Record(
            ValidationLogRecord.RpcSurrogateResolved(source: source, result: result)
        );
    }
}
