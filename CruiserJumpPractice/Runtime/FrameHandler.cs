#nullable enable

using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Services;

namespace CruiserJumpPractice.Runtime;

internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateClientService cruiserStateClientService;
    private readonly MagnetService magnetService;

    public FrameHandler(
        IGameInterop gameInterop,
        CruiserStateClientService cruiserStateClientService,
        MagnetService magnetService
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateClientService = cruiserStateClientService;
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

        cruiserStateClientService.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        cruiserStateClientService.RequestLoadCruiserState();
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