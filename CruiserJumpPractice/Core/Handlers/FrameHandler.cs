// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.UseCases.Client;

namespace CruiserJumpPractice.Core.Handlers;

// Frame handling is coordination rather than policy. It reads one-frame input, skips unsafe
// local-player states, and dispatches client-side commands; save/load rules remain in use cases.
internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IPracticeInput practiceInput;
    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;

    public FrameHandler(
        IGameInterop gameInterop,
        IPracticeInput practiceInput,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.practiceInput = practiceInput;
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

        if (gameInterop.GetLocalPlayerBusyState().IsBusy)
        {
            return;
        }

        if (saveTriggered)
        {
            requestSaveCruiserStateUseCase.Execute();
        }

        if (loadTriggered)
        {
            requestLoadCruiserStateUseCase.Execute();
        }

        if (toggleMagnetTriggered)
        {
            toggleMagnetUseCase.Execute();
        }
    }
}
