#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Presentation;

internal sealed class ClientNotificationService
{
    private readonly IGameInterop gameInterop;

    public ClientNotificationService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void ShowCruiserTip(string message)
    {
        gameInterop.DisplayLocalTip("CruiserJumpPractice", message);
    }
}