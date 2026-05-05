// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Validation;

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
            RecordResult(
                role: ValidationLogRole.Client,
                result: RequestLoadCruiserStateResult.HostOnly
            );
            return RequestLoadCruiserStateResult.HostOnly;
        }

        // Record the local acceptance before crossing into the ServerRpc path; on a host the
        // server callback can run before this method returns.
        RecordResult(
            role: ValidationLogRole.Host,
            result: RequestLoadCruiserStateResult.Success
        );
        gameInterop.RequestLoadCruiserState();
        return RequestLoadCruiserStateResult.Success;
    }

    private void RecordResult(ValidationLogRole role, RequestLoadCruiserStateResult result)
    {
        validationLogger.Record(ValidationLogRecord.RequestLoadResult(role: role, result: result));
    }
}
