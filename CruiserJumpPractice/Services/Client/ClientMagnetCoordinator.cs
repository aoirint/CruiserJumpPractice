#nullable enable

using CruiserJumpPractice.Application;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Services.Client;

internal sealed class ClientMagnetCoordinator
{
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;
    private readonly ClientNotificationService clientNotificationService;

    public ClientMagnetCoordinator(
        ToggleMagnetUseCase toggleMagnetUseCase,
        ClientNotificationService clientNotificationService
    )
    {
        this.toggleMagnetUseCase = toggleMagnetUseCase;
        this.clientNotificationService = clientNotificationService;
    }

    internal void ToggleMagnet()
    {
        var result = toggleMagnetUseCase.Execute();
        if (result == ToggleMagnetResult.HostOnly)
        {
            clientNotificationService.ShowCruiserTip("Only the host can toggle the magnet.");
            return;
        }

        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        clientNotificationService.ShowCruiserTip($"Magnet is now {magnetStateText}.");
    }
}
