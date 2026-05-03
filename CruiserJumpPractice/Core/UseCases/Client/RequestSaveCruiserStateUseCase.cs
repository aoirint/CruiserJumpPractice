#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

internal sealed class RequestSaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;

    public RequestSaveCruiserStateUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public RequestSaveCruiserStateResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip("CruiserJumpPractice", "Only the host can save the cruiser state.");
            return RequestSaveCruiserStateResult.HostOnly;
        }

        gameInterop.RequestSaveCruiserState();
        return RequestSaveCruiserStateResult.Success;
    }
}
