// SPDX-License-Identifier: MIT
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
            new()
            {
                ["action"] = action,
                ["role"] = GetRoleToken(),
                ["busy"] = false
            }
        );
    }

    private void RecordSuppressedInput(string action, LocalPlayerBusyState busyState)
    {
        validationLogger.Record(
            "input_suppressed",
            new()
            {
                ["action"] = action,
                ["role"] = GetRoleToken(),
                ["reason"] = busyState.GetBusyReasonToken() ?? "unknown",
                ["menu"] = busyState.IsMenuOpen,
                ["terminal"] = busyState.IsInTerminal,
                ["chat"] = busyState.IsTypingChat
            }
        );
    }

    private string GetRoleToken()
    {
        return gameInterop.IsHost() ? "host" : "client";
    }
}
