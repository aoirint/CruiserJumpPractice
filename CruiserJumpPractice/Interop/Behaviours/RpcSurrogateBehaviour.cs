#nullable enable

extern alias LethalCompany;

using BepInEx.Logging;
using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop.Behaviours;

internal class RpcSurrogateBehaviour : NetworkBehaviour
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.CruiserStateService.SaveCruiserState();
        SaveCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        CruiserJumpPractice.RequestCruiserStateService.PresentSaveResult(result);
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.CruiserStateService.LoadCruiserState();
        LoadCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        CruiserJumpPractice.RequestCruiserStateService.PresentLoadResult(result);
    }
}
