#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Application.UseCases;

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

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();
        rpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
        return HostGuardResult.Success;
    }
}