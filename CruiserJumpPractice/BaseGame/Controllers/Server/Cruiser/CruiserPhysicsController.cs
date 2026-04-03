#nullable enable

using BepInEx.Logging;
using UnityEngine;

namespace CruiserJumpPractice.BaseGame.Controllers.Server.Cruiser;

sealed class CruiserPhysics
{
    public Vector3 CarPosition { get; private set; }

    public Vector3 CarRotation { get; private set; }

    public float SteeringInput { get; private set; }

    public float EngineRPM { get; private set; }

    public CruiserPhysics(
        Vector3 carPosition,
        Vector3 carRotation,
        float steeringInput,
        float engineRPM
    )
    {
        CarPosition = carPosition;
        CarRotation = carRotation;
        SteeringInput = steeringInput;
        EngineRPM = engineRPM;
    }
}

class CruiserPhysicsControllerException : System.Exception
{
    public CruiserPhysicsControllerException(string message) : base(message) { }
}

class CruiserPhysicsController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected VehicleController cruiser;

    public CruiserPhysicsController(VehicleController cruiser)
    {
        this.cruiser = cruiser;
    }

    public CruiserPhysics GetCruiserPhysics()
    {
        try
        {
            return new CruiserPhysics(
                cruiser.transform.position,
                cruiser.transform.eulerAngles,
                cruiser.moveInputVector.x,
                cruiser.EngineRPM
            );
        } catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting cruiser physics: {error}");
            throw new CruiserPhysicsControllerException($"Exception while getting cruiser physics: {error}");
        }
    }

    public void SetCruiserPhysics(CruiserPhysics cruiserPhysics)
    {
        try
        {
            // NOTE: These values will be synced with vanilla VehicleController.Update and SyncCarPhysicsToOtherClients
            cruiser.transform.position = cruiserPhysics.CarPosition;
            cruiser.transform.eulerAngles = cruiserPhysics.CarRotation;
            cruiser.moveInputVector.x = cruiserPhysics.SteeringInput;
            cruiser.EngineRPM = cruiserPhysics.EngineRPM;
        } catch (System.Exception error)
        {
            Logger.LogError($"Exception while setting cruiser physics: {error}");
            throw new CruiserPhysicsControllerException($"Exception while setting cruiser physics: {error}");
        }
    }
}
