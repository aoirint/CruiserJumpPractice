#nullable enable

using BepInEx.Logging;
using GameNetcodeStuff;
using UnityEngine;

namespace CruiserJumpPractice.Utils;

internal static class CruiserUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal static VehicleController? GetCruiser()
    {
        var vehicleControllers = Object.FindObjectsOfType<VehicleController>();
        if (vehicleControllers == null)
        {
            Logger.LogError("Failed to find VehicleController objects.");
            return null;
        }

        if (vehicleControllers.Length == 0)
        {
            Logger.LogInfo("No VehicleController objects found.");
            return null;
        }

        return vehicleControllers[0];
    }

    internal static GameNetworkManager? GetGameNetworkManager()
    {
        var gameNetworkManager = GameNetworkManager.Instance;
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager.Instance is null.");
            return null;
        }

        return gameNetworkManager;
    }

    internal static PlayerControllerB? GetLocalPlayer()
    {
        var gameNetworkManager = GetGameNetworkManager();
        if (gameNetworkManager == null)
        {
            Logger.LogError("GameNetworkManager is null.");
            return null;
        }

        var localPlayer = gameNetworkManager.localPlayerController;
        if (localPlayer == null)
        {
            Logger.LogError("localPlayerController is null.");
            return null;
        }

        return localPlayer;
    }

    internal static bool SetCarHP(VehicleController cruiser, int carHP)
    {
        var localPlayer = GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return false;
        }

        cruiser.AddEngineOilOnLocalClient(carHP);
        cruiser.AddEngineOilServerRpc((int)localPlayer.playerClientId, carHP);
        return true;
    }

    internal static int? GetTurboBoosts(VehicleController cruiser)
    {
        try
        {
            var turboBoostsField = typeof(VehicleController).GetField("turboBoosts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (turboBoostsField == null)
            {
                Logger.LogError("Failed to get 'turboBoosts' field from VehicleController.");
                return null;
            }

            var turboBoostsValue = turboBoostsField.GetValue(cruiser);
            if (turboBoostsValue is int turboBoosts)
            {
                return turboBoosts;
            }
            else
            {
                Logger.LogError("'turboBoosts' field is not of type int.");
                return null;
            }
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'turboBoosts': {error}");
            return null;
        }
    }

    internal static bool SetTurboBoosts(VehicleController cruiser, int turboBoosts)
    {
        var localPlayer = GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return false;
        }

        cruiser.AddTurboBoostOnLocalClient(turboBoosts);
        cruiser.AddTurboBoostServerRpc((int)localPlayer.playerClientId, turboBoosts);
        return true;
    }
}
