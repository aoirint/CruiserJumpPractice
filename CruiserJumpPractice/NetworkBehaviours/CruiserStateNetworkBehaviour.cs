#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Controllers;
using CruiserJumpPractice.BaseGame.Controllers.Client;
using CruiserJumpPractice.BaseGame.Finders;
using Unity.Netcode;

namespace CruiserJumpPractice.NetworkBehaviours;

internal enum SaveCruiserStateResult
{
    Success,
    NoCruiserFound,
    UnexpectedState
}

internal enum LoadCruiserStateResult
{
    Success,
    NoCruiserFound,
    NoSavedState,
    MagnetedToShip,
    UnexpectedState
}

internal class CruiserStateNetworkBehaviour : NetworkBehaviour
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        if (!HasServerRole())
        {
            Logger.LogError("SaveCruiserStateServerRpc called on client. Ignoring.");
            return;
        }

        CruiserJumpPractice.CruiserStateService.SaveCruiserState();
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        if (!HasClientRole())
        {
            Logger.LogError("SaveCruiserStateDoneClientRpc called on server. Ignoring.");
            return;
        }

        if (result == SaveCruiserStateResult.Success)
        {
            DisplayTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to save.");
        }
        else
        {
            Logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        if (!HasServerRole())
        {
            Logger.LogError("LoadCruiserStateServerRpc called on client. Ignoring.");
            return;
        }

        CruiserJumpPractice.CruiserStateService.LoadCruiserState();
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        if (!HasClientRole())
        {
            Logger.LogError("LoadCruiserStateDoneClientRpc called on server. Ignoring.");
            return;
        }

        if (result == LoadCruiserStateResult.Success)
        {
            DisplayTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            DisplayTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            DisplayTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }

    private static void DisplayTip(string bodyText)
    {
        var hudManagerFinder = new HUDManagerFinder();
        var hudManager = hudManagerFinder.GetHUDManager();
        var tipController = new TipController(hudManager);
        tipController.DisplayTip("CruiserJumpPractice", bodyText);
    }

    private static bool HasServerRole()
    {
        var networkManagerFinder = new NetworkManagerFinder();
        var networkManager = networkManagerFinder.GetNetworkManager();
        var networkStateController = new NetworkStateController(networkManager);
        return networkStateController.IsServer();
    }

    private static bool HasClientRole()
    {
        var networkManagerFinder = new NetworkManagerFinder();
        var networkManager = networkManagerFinder.GetNetworkManager();
        var networkStateController = new NetworkStateController(networkManager);
        return networkStateController.IsClient();
    }
}
