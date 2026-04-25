#nullable enable

extern alias LethalCompany;

using BepInEx.Logging;
using LethalCompany;
using LethalCompany::GameNetcodeStuff;
using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop.Adapters.V73;

internal sealed class GameObjectAdapterV73
{
    private readonly ManualLogSource logger;

    public GameObjectAdapterV73(ManualLogSource logger)
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
