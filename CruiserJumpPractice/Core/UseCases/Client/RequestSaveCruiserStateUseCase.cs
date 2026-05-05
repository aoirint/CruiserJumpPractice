// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// A save key press is handled locally first so non-host players get immediate feedback. Only a
// host request crosses into the RPC path where the server captures the snapshot.
internal sealed class RequestSaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;

    public RequestSaveCruiserStateUseCase(
        IGameInterop gameInterop,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
    }

    public RequestSaveCruiserStateResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(HudTipMessage.SaveHostOnly);
            RecordResult("client", "host_only");
            return RequestSaveCruiserStateResult.HostOnly;
        }

        gameInterop.RequestSaveCruiserState();
        // Request success means the host dispatched the ServerRpc, not that the server saved.
        RecordResult("host", "success");
        return RequestSaveCruiserStateResult.Success;
    }

    private void RecordResult(string role, string result)
    {
        validationLogger.Record(
            "request_save_result",
            ValidationLogField.String("role", role),
            ValidationLogField.String("result", result)
        );
    }
}
