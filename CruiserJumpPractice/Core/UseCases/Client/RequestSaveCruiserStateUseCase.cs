#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.UseCases.Client;

/// <summary>
/// Handles a local save key press before requesting the server save path.
/// </summary>
/// <remarks>
/// Non-host players get immediate feedback. Only a host request crosses into
/// the RPC path where the server captures the snapshot.
/// </remarks>
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
            RecordResult(
                role: ValidationLogRole.Client,
                result: RequestSaveCruiserStateResult.HostOnly
            );
            return RequestSaveCruiserStateResult.HostOnly;
        }

        // Record the local acceptance before crossing into the ServerRpc path; on a host the
        // server callback can run before this method returns.
        RecordResult(
            role: ValidationLogRole.Host,
            result: RequestSaveCruiserStateResult.Success
        );
        gameInterop.RequestSaveCruiserState();
        return RequestSaveCruiserStateResult.Success;
    }

    private void RecordResult(ValidationLogRole role, RequestSaveCruiserStateResult result)
    {
        validationLogger.Record(ValidationLogRecord.RequestSaveResult(role: role, result: result));
    }
}
