#nullable enable

using UnityEngine;

namespace CruiserJumpPractice.Interop.Domain;

internal sealed class CruiserSnapshot
{
    public Vector3 CarPosition { get; private set; }

    public Vector3 CarRotation { get; private set; }

    public float SteeringInput { get; private set; }

    public float EngineRPM { get; private set; }

    public int CarHP { get; private set; }

    public int TurboBoosts { get; private set; }

    public CruiserSnapshot(
        Vector3 carPosition,
        Vector3 carRotation,
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
