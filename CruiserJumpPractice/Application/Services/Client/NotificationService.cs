#nullable enable

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class NotificationService
{
    private readonly IGameInterop gameInterop;

    public NotificationService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void ShowCruiserTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}