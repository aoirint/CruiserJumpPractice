#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Runtime;

internal sealed class ClientStartupHandler
{
    private readonly IGameInterop gameInterop;

    public ClientStartupHandler(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void HandleStartup()
    {
        gameInterop.SpawnRpcSurrogate();
    }
}