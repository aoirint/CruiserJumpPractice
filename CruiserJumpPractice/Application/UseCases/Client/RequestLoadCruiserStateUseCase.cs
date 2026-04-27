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

    public RequestLoadCruiserStateResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip("CruiserJumpPractice", "Only the host can load the cruiser state.");
            return RequestLoadCruiserStateResult.HostOnly;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateBehaviour();
        rpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
        return RequestLoadCruiserStateResult.Success;
    }
}
