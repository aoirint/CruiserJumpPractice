#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.UseCases.Client;

namespace CruiserJumpPractice.Core.Handlers;

// FrameHandler owns only per-frame coordination: read current practice input, ignore unsafe
// player states, and dispatch client-side requests. Server-side save/load rules stay in use
// cases so this class does not become a second policy layer.
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
