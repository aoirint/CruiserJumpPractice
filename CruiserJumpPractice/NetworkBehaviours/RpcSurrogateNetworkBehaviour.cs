#nullable enable

using CruiserJumpPractice.Application;
using BepInEx.Logging;
using Unity.Netcode;

namespace CruiserJumpPractice.NetworkBehaviours;

internal class RpcSurrogateNetworkBehaviour : NetworkBehaviour
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

        var result = CruiserJumpPractice.ServerCruiserStateService.SaveCruiserState();
        SaveCruiserStateDoneClientRpc(result);
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

        var result = CruiserJumpPractice.ServerCruiserStateService.LoadCruiserState();
        LoadCruiserStateDoneClientRpc(result);
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
        CruiserJumpPractice.GameInterop.DisplayLocalTip("CruiserJumpPractice", bodyText);
    }

    private static bool HasServerRole()
    {
        return CruiserJumpPractice.GameInterop.IsServer();
    }

    private static bool HasClientRole()
    {
        return CruiserJumpPractice.GameInterop.IsClient();
    }
}
