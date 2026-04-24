#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Controllers.Server.Cruiser;
using CruiserJumpPractice.BaseGame.Finders;
using CruiserJumpPractice.NetworkBehaviours;
using UnityEngine;

namespace CruiserJumpPractice.Services;

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

class NoCruiserFoundException : System.Exception
{
    public NoCruiserFoundException() : base() { }
}

class NoSavedStateException : System.Exception
{
    public NoSavedStateException() : base() { }
}

class MagnetedToShipException : System.Exception
{
    public MagnetedToShipException() : base() { }
}

internal class CruiserStateService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private CruiserState? savedCruiserState;

    internal void SaveCruiserState()
    {
        var cruiserStateNetworkBehaviourFinder = new CruiserStateNetworkBehaviourFinder();
        var cruiserStateNetworkBehaviour = cruiserStateNetworkBehaviourFinder.GetCruiserStateNetworkBehaviour();

        try
        {
            var cruiserFinder = new CruiserFinder();
            var cruiser = cruiserFinder.GetCruiser();
            if (cruiser == null)
            {
                throw new NoCruiserFoundException();
            }

            var localPlayerIdFinder = new LocalPlayerIdFinder();
            var localPlayerId = localPlayerIdFinder.GetLocalPlayerId();

            var cruiserPhysicsController = new CruiserPhysicsController(cruiser);
            var cruiserHpController = new CruiserHpController(cruiser, localPlayerId);
            var cruiserTurboBoostController = new CruiserTurboBoostController(cruiser, localPlayerId);

            var cruiserPhysics = cruiserPhysicsController.GetCruiserPhysics();
            var cruiserHP = cruiserHpController.GetCruiserHP();
            var turboBoosts = cruiserTurboBoostController.GetCruiserTurboBoosts();

            savedCruiserState = new CruiserState(
                carPosition: cruiserPhysics.CarPosition,
                carRotation: cruiserPhysics.CarRotation,
                steeringInput: cruiserPhysics.SteeringInput,
                engineRPM: cruiserPhysics.EngineRPM,
                carHP: cruiserHP,
                turboBoosts: turboBoosts
            );

            cruiserStateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.Success);
        }
        catch (NoCruiserFoundException)
        {
            Logger.LogInfo("No cruiser found.");
            cruiserStateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.NoCruiserFound);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while saving cruiser state: {error}");
            cruiserStateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.UnexpectedState);
        }
    }

    internal void LoadCruiserState()
    {
        var cruiserStateNetworkBehaviourFinder = new CruiserStateNetworkBehaviourFinder();
        var cruiserStateNetworkBehaviour = cruiserStateNetworkBehaviourFinder.GetCruiserStateNetworkBehaviour();

        try
        {
            var cruiserFinder = new CruiserFinder();
            var cruiser = cruiserFinder.GetCruiser();
            if (cruiser == null)
            {
                throw new NoCruiserFoundException();
            }

            if (savedCruiserState == null)
            {
                throw new NoSavedStateException();
            }

            var localPlayerIdFinder = new LocalPlayerIdFinder();
            var localPlayerId = localPlayerIdFinder.GetLocalPlayerId();

            var cruiserMagnetController = new CruiserMagnetController(cruiser);
            var magnetedToShip = cruiserMagnetController.GetMagnetedToShip();
            if (magnetedToShip)
            {
                throw new MagnetedToShipException();
            }

            var cruiserPhysicsController = new CruiserPhysicsController(cruiser);
            var cruiserHpController = new CruiserHpController(cruiser, localPlayerId);
            var cruiserTurboBoostController = new CruiserTurboBoostController(cruiser, localPlayerId);
            cruiserPhysicsController.SetCruiserPhysics(
                new CruiserPhysics(
                    carPosition: savedCruiserState.CarPosition,
                    carRotation: savedCruiserState.CarRotation,
                    steeringInput: savedCruiserState.SteeringInput,
                    engineRPM: savedCruiserState.EngineRPM
                )
            );
            cruiserHpController.SetCruiserHP(savedCruiserState.CarHP);
            cruiserTurboBoostController.SetCruiserTurboBoosts(savedCruiserState.TurboBoosts);

            cruiserStateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.Success);
        }
        catch (NoCruiserFoundException)
        {
            Logger.LogInfo("No cruiser found.");
            cruiserStateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoCruiserFound);
        }
        catch (NoSavedStateException)
        {
            Logger.LogInfo("No saved cruiser state found.");
            cruiserStateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoSavedState);
        }
        catch (MagnetedToShipException)
        {
            Logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
            cruiserStateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.MagnetedToShip);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while loading cruiser state: {error}");
            cruiserStateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.UnexpectedState);
        }
    }
}
