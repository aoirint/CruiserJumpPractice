#nullable enable

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.UseCases.Client;

internal sealed class NotificationUseCase
{
    private readonly IGameInterop gameInterop;

    public NotificationUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void DisplayTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}
