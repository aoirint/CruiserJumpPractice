// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// A load key press is still local input, even though restoring transform and physics state is
// server work. Host validation happens here before Interop sends the RPC.
internal sealed class RequestLoadCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;

    public RequestLoadCruiserStateUseCase(
        IGameInterop gameInterop,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
    }

    public RequestLoadCruiserStateResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(HudTipMessage.LoadHostOnly);
            RecordResult("client", "host_only");
            return RequestLoadCruiserStateResult.HostOnly;
        }

        gameInterop.RequestLoadCruiserState();
        // Request success means the host dispatched the ServerRpc, not that the server restored.
        RecordResult("host", "success");
        return RequestLoadCruiserStateResult.Success;
    }

    private void RecordResult(string role, string result)
    {
        validationLogger.Record(
            "request_load_result",
            new()
            {
                ["role"] = role,
                ["result"] = result
            }
        );
    }
}
