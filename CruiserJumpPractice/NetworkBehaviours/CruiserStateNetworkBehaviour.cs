#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Utils;
using Unity.Netcode;

namespace CruiserJumpPractice.NetworkBehaviours;

internal enum SaveCruiserStateResult
{
    Success,
    NoCruiserFound
}

internal enum LoadCruiserStateResult
{
    Success,
    NoCruiserFound,
    NoSavedState,
    MagnetedToShip
}

internal class CruiserStateNetworkBehaviour : NetworkBehaviour
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogError("SaveCruiserStateServerRpc called on client. Ignoring.");
            return;
        }

        CruiserJumpPractice.CruiserManager.SaveCruiserState();
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        if (!NetworkUtils.IsClient())
        {
            Logger.LogError("SaveCruiserStateDoneClientRpc called on server. Ignoring.");
            return;
        }

        if (result == SaveCruiserStateResult.Success)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "No cruiser found to save.");
        }
        else
        {
            Logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        if (!NetworkUtils.IsServer())
        {
            Logger.LogError("LoadCruiserStateServerRpc called on client. Ignoring.");
            return;
        }

        CruiserJumpPractice.CruiserManager.LoadCruiserState();
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        if (!NetworkUtils.IsClient())
        {
            Logger.LogError("LoadCruiserStateDoneClientRpc called on server. Ignoring.");
            return;
        }

        if (result == LoadCruiserStateResult.Success)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            HUDManagerUtils.DisplayTip("CruiserJumpPractice", "Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }
}
