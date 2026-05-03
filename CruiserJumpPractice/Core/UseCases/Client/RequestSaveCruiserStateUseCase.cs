#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Client-side request use cases validate local intent before crossing into Netcode RPCs.
// The actual snapshot write happens on the server path so host authority stays explicit.
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
