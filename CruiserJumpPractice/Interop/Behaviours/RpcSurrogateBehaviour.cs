#nullable enable

extern alias LethalCompany;

using BepInEx.Logging;
using LethalCompany::Unity.Netcode;

using CruiserJumpPractice.Core.UseCases;

namespace CruiserJumpPractice.Interop.Behaviours;

internal class RpcSurrogateBehaviour : NetworkBehaviour
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [ServerRpc(RequireOwnership = true)]
    public void SaveCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.SaveCruiserStateUseCase.Execute();
        SaveCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void SaveCruiserStateDoneClientRpc(SaveCruiserStateResult result)
    {
        CruiserJumpPractice.PresentSaveCruiserStateResultUseCase.Execute(result);
    }

    [ServerRpc(RequireOwnership = true)]
    public void LoadCruiserStateServerRpc()
    {
        var result = CruiserJumpPractice.LoadCruiserStateUseCase.Execute();
        LoadCruiserStateDoneClientRpc(result);
    }

    [ClientRpc]
    public void LoadCruiserStateDoneClientRpc(LoadCruiserStateResult result)
    {
        CruiserJumpPractice.PresentLoadCruiserStateResultUseCase.Execute(result);
    }
}
