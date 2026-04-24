#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services.Client;

internal sealed class ClientRpcSurrogateService
{
    private readonly IGameInterop gameInterop;

    public ClientRpcSurrogateService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void EnsureSpawned()
    {
        gameInterop.SpawnRpcSurrogate();
    }
}
