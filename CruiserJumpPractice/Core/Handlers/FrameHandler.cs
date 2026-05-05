// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases.Client;

namespace CruiserJumpPractice.Core.Handlers;

// Frame handling is coordination rather than policy. It reads one-frame input, skips unsafe
// local-player states, and dispatches client-side commands; save/load rules remain in use cases.
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
            RecordSuppressedInput(saveTriggered, loadTriggered, toggleMagnetTriggered, busyState);
            return;
        }

        if (saveTriggered)
        {
            RecordTriggeredInput("save");
            requestSaveCruiserStateUseCase.Execute();
        }

        if (loadTriggered)
        {
            RecordTriggeredInput("load");
            requestLoadCruiserStateUseCase.Execute();
        }

        if (toggleMagnetTriggered)
        {
            RecordTriggeredInput("toggle_magnet");
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
            RecordSuppressedInput("save", busyState);
        }

        if (loadTriggered)
        {
            RecordSuppressedInput("load", busyState);
        }

        if (toggleMagnetTriggered)
        {
            RecordSuppressedInput("toggle_magnet", busyState);
        }
    }

    private void RecordTriggeredInput(string action)
    {
        validationLogger.Record(
            "input_triggered",
            ValidationLogField.String("action", action),
            ValidationLogField.String("role", GetRoleToken()),
            ValidationLogField.Bool("busy", false)
        );
    }

    private void RecordSuppressedInput(string action, LocalPlayerBusyState busyState)
    {
        validationLogger.Record(
            "input_suppressed",
            ValidationLogField.String("action", action),
            ValidationLogField.String("role", GetRoleToken()),
            ValidationLogField.String("reason", busyState.GetBusyReasonToken() ?? "unknown"),
            ValidationLogField.Bool("menu", busyState.IsMenuOpen),
            ValidationLogField.Bool("terminal", busyState.IsInTerminal),
            ValidationLogField.Bool("chat", busyState.IsTypingChat)
        );
    }

    private string GetRoleToken()
    {
        return gameInterop.IsHost() ? "host" : "client";
    }
}
