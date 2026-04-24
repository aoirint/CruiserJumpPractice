#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application.Services.Client;

namespace CruiserJumpPractice.Application.Runtime;

internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateOperationRequestService cruiserStateOperationRequestService;
    private readonly MagnetService magnetService;

    public FrameHandler(
        IGameInterop gameInterop,
        CruiserStateOperationRequestService cruiserStateOperationRequestService,
        MagnetService magnetService
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateOperationRequestService = cruiserStateOperationRequestService;
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

        cruiserStateOperationRequestService.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        cruiserStateOperationRequestService.RequestLoadCruiserState();
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