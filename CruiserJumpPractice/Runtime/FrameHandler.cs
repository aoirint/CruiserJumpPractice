#nullable enable

using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Services.Client;

namespace CruiserJumpPractice.Runtime;

internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateService cruiserStateService;
    private readonly MagnetService magnetService;

    public FrameHandler(
        IGameInterop gameInterop,
        CruiserStateService cruiserStateService,
        MagnetService magnetService
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateService = cruiserStateService;
        this.magnetService = magnetService;
    }

    public void HandleFrame()
    {
        if (!gameInterop.IsClient())
        {
            return;
        }

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
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        cruiserStateService.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        cruiserStateService.RequestLoadCruiserState();
    }

    private void UpdateToggleMagnet()
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        magnetService.ToggleMagnet();
    }
}