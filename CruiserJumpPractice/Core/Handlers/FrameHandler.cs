// SPDX-License-Identifier: Unlicense
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
        if (gameInterop.IsLocalPlayerBusy())
        {
            return;
        }

        UpdateSaveCruiser();
        UpdateLoadCruiser();
        UpdateToggleMagnet();
    }

    private void UpdateSaveCruiser()
    {
        if (!practiceInput.SaveCruiserTriggered)
        {
            return;
        }

        requestSaveCruiserStateUseCase.Execute();
    }

    private void UpdateLoadCruiser()
    {
        if (!practiceInput.LoadCruiserTriggered)
        {
            return;
        }

        requestLoadCruiserStateUseCase.Execute();
    }

    private void UpdateToggleMagnet()
    {
        if (!practiceInput.ToggleMagnetTriggered)
        {
            return;
        }

        toggleMagnetUseCase.Execute();
    }
}
