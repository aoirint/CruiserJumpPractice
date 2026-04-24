#nullable enable

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Services;

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