// SPDX-License-Identifier: Unlicense
#nullable enable

extern alias LethalCompany;

using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Core.UseCases;

namespace CruiserJumpPractice.Interop.Game.Behaviours;

// Netcode RPC methods must live on a NetworkBehaviour, so this bridge stays in
// Interop. It crosses the client/server boundary, then hands execution and
// result presentation back to PluginController.
internal class RpcSurrogateBehaviour : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        CruiserJumpPractice.Controller.RecordSaveServerRpcReceived();
        var result = CruiserJumpPractice.Controller.SaveCruiserState();
        SaveCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        CruiserJumpPractice.Controller.RecordSaveClientRpcReceived(result);
        CruiserJumpPractice.Controller.PresentSaveCruiserStateResult(result);
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        CruiserJumpPractice.Controller.RecordLoadServerRpcReceived();
        var result = CruiserJumpPractice.Controller.LoadCruiserState();
        LoadCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        CruiserJumpPractice.Controller.RecordLoadClientRpcReceived(result);
        CruiserJumpPractice.Controller.PresentLoadCruiserStateResult(result);
    }
}
