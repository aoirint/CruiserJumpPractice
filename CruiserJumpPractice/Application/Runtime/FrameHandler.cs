#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application.Services;

namespace CruiserJumpPractice.Application.Runtime;

internal sealed class FrameHandler
{
    private readonly IGameInterop gameInterop;
    private readonly ClientCruiserStateService clientCruiserStateService;
    private readonly ClientMagnetService clientMagnetService;

    public FrameHandler(
        IGameInterop gameInterop,
        ClientCruiserStateService clientCruiserStateService,
        ClientMagnetService clientMagnetService
    )
    {
        this.gameInterop = gameInterop;
        this.clientCruiserStateService = clientCruiserStateService;
        this.clientMagnetService = clientMagnetService;
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

        clientCruiserStateService.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateService.RequestLoadCruiserState();
    }

    private void UpdateToggleMagnet()
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        clientMagnetService.ToggleMagnet();
    }
}