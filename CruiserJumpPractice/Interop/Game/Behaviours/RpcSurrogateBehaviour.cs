#nullable enable

extern alias LethalCompany;

using BepInEx.Logging;
using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Core.UseCases;

namespace CruiserJumpPractice.Interop.Game.Behaviours;

// Netcode RPC methods must live on a NetworkBehaviour, which is why this bridge exists. The
// actual save/load decisions are still owned by Core use cases through PluginController; this
// class only crosses the client/server boundary and returns the result to the requesting client.
internal class RpcSurrogateBehaviour : NetworkBehaviour
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.Controller.SaveCruiserState();
        SaveCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        CruiserJumpPractice.Controller.PresentSaveCruiserStateResult(result);
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.Controller.LoadCruiserState();
        LoadCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        CruiserJumpPractice.Controller.PresentLoadCruiserStateResult(result);
    }
}
