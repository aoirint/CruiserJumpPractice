#nullable enable

extern alias LethalCompany;
extern alias UnityEngine;

using BepInEx.Logging;
using LethalCompany;
using System.Reflection;
using UnityEngine::UnityEngine;

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Interop.Adapters;

internal sealed class CruiserAdapter
{
    private readonly ManualLogSource logger;
    private readonly GameObjectAdapter gameObjects;

    public CruiserAdapter(ManualLogSource logger, GameObjectAdapter gameObjects)
    {
        this.logger = logger;
        this.gameObjects = gameObjects;
    }

    public VehicleController? FindCruiser()
    {
        try
        {
            var vehicleControllers = Object.FindObjectsOfType<VehicleController>();
            if (vehicleControllers == null)
            {
                logger.LogError("Failed to find VehicleController objects.");
                return null;
            }

            if (vehicleControllers.Length == 0)
            {
                logger.LogInfo("No VehicleController objects found.");
                return null;
            }

            return vehicleControllers[0];
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting cruiser: {error}");
            throw new GameInteropException($"Exception while getting cruiser: {error}");
        }
    }

    public CruiserSnapshot CaptureCruiser(VehicleController cruiser)
    {
        try
        {
            return new CruiserSnapshot(
                carPosition: FromUnityVector3(cruiser.transform.position),
                carRotation: FromUnityVector3(cruiser.transform.eulerAngles),
                steeringInput: cruiser.moveInputVector.x,
                engineRPM: cruiser.EngineRPM,
                carHP: cruiser.carHP,
                turboBoosts: GetCruiserTurboBoosts(cruiser)
            );
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while capturing cruiser state: {error}");
            throw new GameInteropException($"Exception while capturing cruiser state: {error}");
        }
    }

    public void RestoreCruiser(VehicleController cruiser, CruiserSnapshot snapshot)
    {
        var localPlayerId = gameObjects.GetLocalPlayerId();
        try
        {
            // NOTE: These values will be synced with vanilla VehicleController.Update and SyncCarPhysicsToOtherClients.
            cruiser.transform.position = ToUnityVector3(snapshot.CarPosition);
            cruiser.transform.eulerAngles = ToUnityVector3(snapshot.CarRotation);
            cruiser.moveInputVector.x = snapshot.SteeringInput;
            cruiser.EngineRPM = snapshot.EngineRPM;

            cruiser.AddEngineOilOnLocalClient(snapshot.CarHP);
            cruiser.AddEngineOilServerRpc(localPlayerId, snapshot.CarHP);

            cruiser.AddTurboBoostOnLocalClient(snapshot.TurboBoosts);
            cruiser.AddTurboBoostServerRpc(localPlayerId, snapshot.TurboBoosts);
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while restoring cruiser state: {error}");
            throw new GameInteropException($"Exception while restoring cruiser state: {error}");
        }
    }

    public bool IsCruiserMagnetedToShip(VehicleController cruiser)
    {
        try
        {
            return cruiser.magnetedToShip;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'magnetedToShip': {error}");
            throw new GameInteropException($"Exception while getting 'magnetedToShip': {error}");
        }
    }

    private int GetCruiserTurboBoosts(VehicleController cruiser)
    {
        try
        {
            var turboBoostsField = typeof(VehicleController).GetField(
                "turboBoosts",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            if (turboBoostsField == null)
            {
                throw new GameInteropException("Failed to get 'turboBoosts' field from VehicleController.");
            }

            var turboBoostsValue = turboBoostsField.GetValue(cruiser);
            if (turboBoostsValue is int turboBoosts)
            {
                return turboBoosts;
            }

            throw new GameInteropException("'turboBoosts' field is not of type int.");
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while getting 'turboBoosts': {error}");
            throw new GameInteropException($"Exception while getting 'turboBoosts': {error}");
        }
    }

    private static Vector3Value FromUnityVector3(Vector3 value)
    {
        return new Vector3Value(value.x, value.y, value.z);
    }

    private static Vector3 ToUnityVector3(Vector3Value value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }
}
