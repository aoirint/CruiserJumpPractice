#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;
using CruiserJumpPractice.Utils;
using UnityEngine;

namespace CruiserJumpPractice.Managers;

internal sealed class CruiserState
{
    public Vector3 CarPosition { get; private set; }

    public Vector3 CarRotation { get; private set; }

    public float SteeringInput { get; private set; }

    public float EngineRPM { get; private set; }

    public int CarHP { get; private set; }

    public int TurboBoosts { get; private set; }

    public CruiserState(
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

internal class CruiserManager
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private CruiserState? savedCruiserState;

    internal void SaveCruiserState()
    {
        var cruiserJumpPracticeNetworkBehaviour = NetworkBehaviourUtils.GetCruiserJumpPracticeNetworkBehaviour();
        if (cruiserJumpPracticeNetworkBehaviour == null)
        {
            Logger.LogError("CruiserJumpPracticeNetworkBehaviour is null.");
            return;
        }

        var cruiser = CruiserUtils.GetCruiser();
        if (cruiser == null)
        {
            cruiserJumpPracticeNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.NoCruiserFound);
            return;
        }

        savedCruiserState = new CruiserState(
            carPosition: cruiser.transform.position,
            carRotation: cruiser.transform.eulerAngles,
            steeringInput: cruiser.moveInputVector.x,
            engineRPM: cruiser.EngineRPM,
            carHP: cruiser.carHP,
            turboBoosts: cruiser.turboBoosts
        );

        cruiserJumpPracticeNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.Success);
    }

    internal void LoadCruiserState()
    {
        var cruiserJumpPracticeNetworkBehaviour = NetworkBehaviourUtils.GetCruiserJumpPracticeNetworkBehaviour();
        if (cruiserJumpPracticeNetworkBehaviour == null)
        {
            Logger.LogError("CruiserJumpPracticeNetworkBehaviour is null.");
            return;
        }

        var cruiser = CruiserUtils.GetCruiser();
        if (cruiser == null)
        {
            cruiserJumpPracticeNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoCruiserFound);
            return;
        }

        if (savedCruiserState == null)
        {
            cruiserJumpPracticeNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoSavedState);
            return;
        }

        cruiser.transform.position = savedCruiserState.CarPosition;
        cruiser.transform.eulerAngles = savedCruiserState.CarRotation;
        cruiser.steeringAnimValue = savedCruiserState.SteeringInput;
        cruiser.EngineRPM = savedCruiserState.EngineRPM;
        cruiser.carHP = savedCruiserState.CarHP;
        cruiser.turboBoosts = savedCruiserState.TurboBoosts;

        cruiserJumpPracticeNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.Success);
    }
}
