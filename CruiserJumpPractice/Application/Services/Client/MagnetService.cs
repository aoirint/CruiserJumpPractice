#nullable enable

using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases.Client;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class MagnetService
{
    private readonly ToggleMagnetUseCase toggleMagnetUseCase;
    private readonly NotificationUseCase notificationUseCase;

    public MagnetService(
        ToggleMagnetUseCase toggleMagnetUseCase,
        NotificationUseCase notificationUseCase
    )
    {
        this.toggleMagnetUseCase = toggleMagnetUseCase;
        this.notificationUseCase = notificationUseCase;
    }

    internal void ToggleMagnet()
    {
        var result = toggleMagnetUseCase.Execute();
        if (result == ToggleMagnetResult.HostOnly)
        {
            notificationUseCase.DisplayTip("Only the host can toggle the magnet.");
            return;
        }

        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        notificationUseCase.DisplayTip($"Magnet is now {magnetStateText}.");
    }
}
