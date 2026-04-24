#nullable enable

using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class MagnetService
{
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;
    private readonly NotificationUsecase notificationUsecase;

    public MagnetService(
        ToggleMagnetUseCase toggleMagnetUseCase,
        NotificationUsecase notificationUsecase
    )
    {
        this.toggleMagnetUseCase = toggleMagnetUseCase;
        this.notificationUsecase = notificationUsecase;
    }

    internal void ToggleMagnet()
    {
        var result = toggleMagnetUseCase.Execute();
        if (result == ToggleMagnetResult.HostOnly)
        {
            notificationUsecase.DisplayTip("Only the host can toggle the magnet.");
            return;
        }

        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        notificationUsecase.DisplayTip($"Magnet is now {magnetStateText}.");
    }
}
