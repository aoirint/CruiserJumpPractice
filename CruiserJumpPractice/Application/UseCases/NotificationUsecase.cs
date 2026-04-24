#nullable enable

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.UseCases;

internal sealed class NotificationUsecase
{
    private readonly IGameInterop gameInterop;

    public NotificationUsecase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void DisplayTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}
