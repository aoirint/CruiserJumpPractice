// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// A load key press is still local input, even though restoring transform and physics state is
// server work. Host validation happens here before Interop sends the RPC.
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
