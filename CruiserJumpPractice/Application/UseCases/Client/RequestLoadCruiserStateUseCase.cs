#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Application.UseCases.Client;

internal sealed class RequestLoadCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;

    public RequestLoadCruiserStateUseCase(IGameInterop gameInterop)
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
        rpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
        return HostGuardResult.Success;
    }
}