#nullable enable

using BepInEx.Logging;
using UnityEngine;

namespace CruiserJumpPractice.BaseGame.Finders;

class CruiserFinderException : System.Exception
{
    public CruiserFinderException(string message) : base(message) { }
}

class CruiserFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public VehicleController? GetCruiser()
    {
        try
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
        } catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting cruiser: {error}");
            throw new CruiserFinderException($"Exception while getting cruiser: {error}");
        }
    }
}
