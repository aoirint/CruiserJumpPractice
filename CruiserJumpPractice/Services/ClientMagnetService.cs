#nullable enable

using CruiserJumpPractice.Domain;
using CruiserJumpPractice.UseCases;

namespace CruiserJumpPractice.Services;

internal sealed class ClientMagnetService
{
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;
    private readonly ClientNotificationService clientNotificationService;

    public ClientMagnetService(
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
