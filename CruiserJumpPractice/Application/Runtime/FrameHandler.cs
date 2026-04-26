#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application.Services.Client;

namespace CruiserJumpPractice.Application.Runtime;

internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly RequestCruiserStateService requestCruiserStateService;
    private readonly MagnetService magnetService;

    public FrameHandler(
        IGameInterop gameInterop,
        RequestCruiserStateService requestCruiserStateService,
        MagnetService magnetService
    )
    {
        this.gameInterop = gameInterop;
        this.requestCruiserStateService = requestCruiserStateService;
        this.magnetService = magnetService;
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
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        requestCruiserStateService.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        requestCruiserStateService.RequestLoadCruiserState();
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
