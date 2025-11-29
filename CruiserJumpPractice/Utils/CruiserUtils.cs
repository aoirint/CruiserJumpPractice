#nullable enable

using BepInEx.Logging;
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
}
