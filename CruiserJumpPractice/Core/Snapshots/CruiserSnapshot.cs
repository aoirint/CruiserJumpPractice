#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

// CruiserSnapshot is a Core value object, so it stores only the fields practice mode needs and
// avoids Unity component references. Interop is responsible for translating this data to and
// from the live VehicleController instance.
internal sealed class CruiserSnapshot
{
    public Vector3Value CarPosition { get; }

    public Vector3Value CarRotation { get; }

    public float SteeringInput { get; }

    public float EngineRPM { get; }

    public int CarHP { get; }

    public int TurboBoosts { get; }

    public CruiserSnapshot(
        Vector3Value carPosition,
        Vector3Value carRotation,
        float steeringInput,
        float engineRPM,
        int carHP,
        int turboBoosts
    )
    {
        CarPosition = carPosition;
        CarRotation = carRotation;
        SteeringInput = steeringInput;
        EngineRPM = engineRPM;
        CarHP = carHP;
        TurboBoosts = turboBoosts;
    }
}
