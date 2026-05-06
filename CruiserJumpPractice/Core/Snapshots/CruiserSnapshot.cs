// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

/// <summary>
/// Plain saved cruiser state used by the practice save/load rules.
/// </summary>
/// <remarks>
/// Core needs these values, not a reference to VehicleController. Interop
/// translates between this snapshot and the live Unity component.
/// </remarks>
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
