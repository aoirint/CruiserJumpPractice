#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.UseCases;

internal sealed class RequestSaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;

    public RequestSaveCruiserStateUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public HostGuardResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            return HostGuardResult.HostOnly;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateBehaviour();
        rpcSurrogateNetworkBehaviour.SaveCruiserStateServerRpc();
        return HostGuardResult.Success;
    }
}