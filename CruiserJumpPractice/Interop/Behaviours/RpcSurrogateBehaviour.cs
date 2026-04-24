#nullable enable

using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using Unity.Netcode;

namespace CruiserJumpPractice.Interop.Behaviours;

internal class RpcSurrogateBehaviour : NetworkBehaviour
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

        var result = CruiserJumpPractice.CruiserStateOperationService.SaveCruiserState();
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

        CruiserJumpPractice.CruiserStateOperationRequestService.PresentSaveResult(result);
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        if (!HasServerRole())
        {
            Logger.LogError("LoadCruiserStateServerRpc called on client. Ignoring.");
            return;
        }

        var result = CruiserJumpPractice.CruiserStateOperationService.LoadCruiserState();
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

        CruiserJumpPractice.CruiserStateOperationRequestService.PresentLoadResult(result);
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
