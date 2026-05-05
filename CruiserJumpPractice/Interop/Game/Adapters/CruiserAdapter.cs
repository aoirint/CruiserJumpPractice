// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;
extern alias UnityEngine;

using System.Reflection;
using LethalCompany;
using UnityEngine::UnityEngine;

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Interop.Game.Adapters;

// CruiserAdapter finds live cruisers and turns them into the snapshots used by save/load.
// Unity vector conversion and reflection for private cruiser fields stay here, away from Core
// practice rules.
internal sealed class CruiserAdapter
{
    // The field identity belongs to the VehicleController type, not to each cruiser instance.
    // Cache it once and use GetValue only for the per-instance read.
    private static readonly FieldInfo? turboBoostsField = typeof(VehicleController).GetField(
        name: "turboBoosts",
        bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance
    );

    private readonly IPluginLogger logger;
    private readonly GameObjectAdapter gameObjects;

    public CruiserAdapter(IPluginLogger logger, GameObjectAdapter gameObjects)
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
                turboBoosts: GetTurboBoosts(cruiser: cruiser)
            );
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while capturing cruiser state: {error}");
            throw new GameInteropException($"Exception while capturing cruiser state: {error}");
        }
    }

    public CruiserRestoreObservation RestoreCruiser(VehicleController cruiser, CruiserSnapshot snapshot)
    {
        var localPlayerId = gameObjects.GetLocalPlayerId();
        try
        {
            // Capture observation values from this live cruiser instance only; restore validation
            // should not add another scene search or polling loop when logging is wired later.
            var beforeCarPosition = FromUnityVector3(cruiser.transform.position);
            var beforeCarHP = cruiser.carHP;
            var beforeTurboBoosts = GetTurboBoosts(cruiser: cruiser);

            // VehicleController already syncs transform and driving fields during its vanilla
            // update flow, while oil and turbo counts need the game's RPC helpers below.
            cruiser.transform.position = ToUnityVector3(snapshot.CarPosition);
            cruiser.transform.eulerAngles = ToUnityVector3(snapshot.CarRotation);
            cruiser.moveInputVector.x = snapshot.SteeringInput;
            cruiser.EngineRPM = snapshot.EngineRPM;

            cruiser.AddEngineOilOnLocalClient(snapshot.CarHP);
            cruiser.AddEngineOilServerRpc(localPlayerId, snapshot.CarHP);

            cruiser.AddTurboBoostOnLocalClient(snapshot.TurboBoosts);
            cruiser.AddTurboBoostServerRpc(localPlayerId, snapshot.TurboBoosts);

            return new CruiserRestoreObservation(
                savedCarPosition: snapshot.CarPosition,
                savedCarRotation: snapshot.CarRotation,
                beforeCarPosition: beforeCarPosition,
                afterCarPosition: FromUnityVector3(cruiser.transform.position),
                savedCarHP: snapshot.CarHP,
                beforeCarHP: beforeCarHP,
                afterCarHP: cruiser.carHP,
                savedTurboBoosts: snapshot.TurboBoosts,
                beforeTurboBoosts: beforeTurboBoosts,
                afterTurboBoosts: GetTurboBoosts(cruiser: cruiser)
            );
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

    internal static int GetTurboBoosts(VehicleController cruiser)
    {
        // This remains static so Harmony patches can reuse the same interop read without owning an
        // adapter instance; CruiserAdapter still owns the private-field knowledge.
        if (turboBoostsField == null)
        {
            throw new GameInteropException(
                message: "Failed to get 'turboBoosts' field from VehicleController."
            );
        }

        var turboBoostsValue = turboBoostsField.GetValue(obj: cruiser);
        if (turboBoostsValue is int turboBoosts)
        {
            return turboBoosts;
        }

        throw new GameInteropException(message: "'turboBoosts' field is not of type int.");
    }

    private static Vector3Value FromUnityVector3(Vector3 value)
    {
        return new Vector3Value(x: value.x, y: value.y, z: value.z);
    }

    private static Vector3 ToUnityVector3(Vector3Value value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }
}
