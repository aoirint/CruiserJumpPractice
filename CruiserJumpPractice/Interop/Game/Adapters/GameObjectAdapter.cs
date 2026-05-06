// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using LethalCompany;
using LethalCompany::GameNetcodeStuff;
using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Interop.Game;

namespace CruiserJumpPractice.Interop.Game.Adapters;

// Game object lookups stay centralized so Unity singleton/null handling has one
// policy: convert missing base-game objects into GameInteropException with a
// log entry at the Interop boundary.
internal sealed class GameObjectAdapter
{
    private readonly IPluginLogger logger;

    public GameObjectAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public HUDManager GetHUDManager()
    {
        try
        {
            var hudManager = HUDManager.Instance;
            if (hudManager == null)
            {
                throw new GameInteropException("HUDManager.Instance is null.");
            }

            return hudManager;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting HUDManager: {error}");
            throw new GameInteropException($"Exception while getting HUDManager: {error}");
        }
    }

    public NetworkManager GetNetworkManager()
    {
        try
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null)
            {
                throw new GameInteropException("NetworkManager.Singleton is null.");
            }

            return networkManager;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting NetworkManager: {error}");
            throw new GameInteropException($"Exception while getting NetworkManager: {error}");
        }
    }

    public PlayerControllerB GetLocalPlayer()
    {
        try
        {
            var gameNetworkManager = GameNetworkManager.Instance;
            if (gameNetworkManager == null)
            {
                throw new GameInteropException("GameNetworkManager.Instance is null.");
            }

            var localPlayer = gameNetworkManager.localPlayerController;
            if (localPlayer == null)
            {
                throw new GameInteropException("localPlayerController is null.");
            }

            return localPlayer;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting local player: {error}");
            throw new GameInteropException($"Exception while getting local player: {error}");
        }
    }

    public int GetLocalPlayerId()
    {
        try
        {
            // Netcode RPC helpers take the local player ID as an int even
            // though the base-game player controller stores it as an unsigned
            // client ID.
            return (int)GetLocalPlayer().playerClientId;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting local player ID: {error}");
            throw new GameInteropException($"Exception while getting local player ID: {error}");
        }
    }

    public StartOfRound GetStartOfRound()
    {
        try
        {
            var startOfRound = StartOfRound.Instance;
            if (startOfRound == null)
            {
                throw new GameInteropException("StartOfRound.Instance is null.");
            }

            return startOfRound;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting StartOfRound: {error}");
            throw new GameInteropException($"Exception while getting StartOfRound: {error}");
        }
    }
}
