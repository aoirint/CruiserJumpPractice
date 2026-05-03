#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Loading starts as a client-side input event, but restoring physics state is server work.
// Host-only feedback is handled before Interop sends the RPC.
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

        gameInterop.RequestLoadCruiserState();
        return RequestLoadCruiserStateResult.Success;
    }
}
