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

    public void OnTick(HUDManager hudManager)
    {
        if (!gameInterop.IsClient())
        {
            return;
        }

        if (gameInterop.IsLocalPlayerBusy())
        {
            return;
        }

        UpdateSaveCruiser(hudManager);
        UpdateLoadCruiser(hudManager);
        UpdateToggleMagnet(hudManager);
    }

    private void UpdateSaveCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateService.RequestSaveCruiserState(hudManager);
    }

    private void UpdateLoadCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateService.RequestLoadCruiserState(hudManager);
    }

    private void UpdateToggleMagnet(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        clientMagnetService.ToggleMagnet(hudManager);
    }
}
