#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services.Client;

internal sealed class ClientTickService
{
    private readonly IGameInterop gameInterop;
    private readonly ClientCruiserStateService clientCruiserStateService;
    private readonly ClientMagnetService clientMagnetService;

    public ClientTickService(
        IGameInterop gameInterop,
        ClientCruiserStateService clientCruiserStateService,
        ClientMagnetService clientMagnetService
    )
    {
        this.gameInterop = gameInterop;
        this.clientCruiserStateService = clientCruiserStateService;
        this.clientMagnetService = clientMagnetService;
    }

    public void OnTick()
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
