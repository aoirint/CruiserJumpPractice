#nullable enable

using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class MagnetService
{
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;
    private readonly NotificationService notificationService;

    public MagnetService(
        ToggleMagnetUseCase toggleMagnetUseCase,
        NotificationService notificationService
    )
    {
        this.toggleMagnetUseCase = toggleMagnetUseCase;
        this.notificationService = notificationService;
    }

    internal void ToggleMagnet()
    {
        var result = toggleMagnetUseCase.Execute();
        if (result == ToggleMagnetResult.HostOnly)
        {
            notificationService.ShowCruiserTip("Only the host can toggle the magnet.");
            return;
        }

        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        notificationService.ShowCruiserTip($"Magnet is now {magnetStateText}.");
    }
}
