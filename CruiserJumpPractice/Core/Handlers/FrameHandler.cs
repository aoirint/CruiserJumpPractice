#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.Handlers;

/// <summary>
/// Coordinates one-frame practice input with client-side command dispatch.
/// </summary>
/// <remarks>
/// Save/load policy remains in use cases; this handler reads input, suppresses
/// unsafe local-player states, and dispatches accepted commands.
/// </remarks>
internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IPracticeInput practiceInput;
    private readonly IValidationLogger validationLogger;
    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;

    public FrameHandler(
        IGameInterop gameInterop,
        IPracticeInput practiceInput,
        IValidationLogger validationLogger,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.practiceInput = practiceInput;
        this.validationLogger = validationLogger;
        this.requestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        this.requestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        this.toggleMagnetUseCase = toggleMagnetUseCase;
    }

    public void HandleFrame()
    {
        // Snapshot one-frame triggers before suppression so later validation logging can
        // report the intended action without retaining raw input, chat, or terminal text.
        var saveTriggered = practiceInput.SaveCruiserTriggered;
        var loadTriggered = practiceInput.LoadCruiserTriggered;
        var toggleMagnetTriggered = practiceInput.ToggleMagnetTriggered;

        if (!saveTriggered && !loadTriggered && !toggleMagnetTriggered)
        {
            return;
        }

        var busyState = gameInterop.GetLocalPlayerBusyState();
        if (busyState.IsBusy)
        {
            RecordSuppressedInput(
                saveTriggered: saveTriggered,
                loadTriggered: loadTriggered,
                toggleMagnetTriggered: toggleMagnetTriggered,
                busyState: busyState
            );
            return;
        }

        if (saveTriggered)
        {
            RecordTriggeredInput(ValidationLogInputAction.Save);
            requestSaveCruiserStateUseCase.Execute();
        }

        if (loadTriggered)
        {
            RecordTriggeredInput(ValidationLogInputAction.Load);
            requestLoadCruiserStateUseCase.Execute();
        }

        if (toggleMagnetTriggered)
        {
            RecordTriggeredInput(ValidationLogInputAction.ToggleMagnet);
            toggleMagnetUseCase.Execute();
        }
    }

    private void RecordSuppressedInput(
        bool saveTriggered,
        bool loadTriggered,
        bool toggleMagnetTriggered,
        LocalPlayerBusyState busyState
    )
    {
        if (saveTriggered)
        {
            RecordSuppressedInput(action: ValidationLogInputAction.Save, busyState: busyState);
        }

        if (loadTriggered)
        {
            RecordSuppressedInput(action: ValidationLogInputAction.Load, busyState: busyState);
        }

        if (toggleMagnetTriggered)
        {
            RecordSuppressedInput(
                action: ValidationLogInputAction.ToggleMagnet,
                busyState: busyState
            );
        }
    }

    private void RecordTriggeredInput(ValidationLogInputAction action)
    {
        validationLogger.Record(
            ValidationLogRecord.InputTriggered(action: action, role: GetRole())
        );
    }

    private void RecordSuppressedInput(
        ValidationLogInputAction action,
        LocalPlayerBusyState busyState
    )
    {
        validationLogger.Record(
            ValidationLogRecord.InputSuppressed(
                action: action,
                role: GetRole(),
                busyState: busyState
            )
        );
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }
}
