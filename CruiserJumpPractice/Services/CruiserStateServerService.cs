#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.Services;

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

internal class CruiserStateServerService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    private CruiserSnapshot? savedCruiserState;

    public CruiserStateServerService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    internal void SaveCruiserState()
    {
        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();

        try
        {
            var cruiser = gameInterop.FindCruiser();
            if (cruiser == null)
            {
                throw new NoCruiserFoundException();
            }

            savedCruiserState = gameInterop.CaptureCruiser(cruiser);

            rpcSurrogateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.Success);
        }
        catch (NoCruiserFoundException)
        {
            Logger.LogInfo("No cruiser found.");
            rpcSurrogateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.NoCruiserFound);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while saving cruiser state: {error}");
            rpcSurrogateNetworkBehaviour.SaveCruiserStateDoneClientRpc(SaveCruiserStateResult.UnexpectedState);
        }
    }

    internal void LoadCruiserState()
    {
        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();

        try
        {
            var cruiser = gameInterop.FindCruiser();
            if (cruiser == null)
            {
                throw new NoCruiserFoundException();
            }

            if (savedCruiserState == null)
            {
                throw new NoSavedStateException();
            }

            var magnetedToShip = gameInterop.IsCruiserMagnetedToShip(cruiser);
            if (magnetedToShip)
            {
                throw new MagnetedToShipException();
            }

            gameInterop.RestoreCruiser(cruiser, savedCruiserState);

            rpcSurrogateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.Success);
        }
        catch (NoCruiserFoundException)
        {
            Logger.LogInfo("No cruiser found.");
            rpcSurrogateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoCruiserFound);
        }
        catch (NoSavedStateException)
        {
            Logger.LogInfo("No saved cruiser state found.");
            rpcSurrogateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.NoSavedState);
        }
        catch (MagnetedToShipException)
        {
            Logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
            rpcSurrogateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.MagnetedToShip);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while loading cruiser state: {error}");
            rpcSurrogateNetworkBehaviour.LoadCruiserStateDoneClientRpc(LoadCruiserStateResult.UnexpectedState);
        }
    }

}
