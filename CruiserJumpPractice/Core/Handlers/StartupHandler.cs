#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.Handlers;

internal sealed class StartupHandler
{
    private readonly IGameInterop gameInterop;

    public StartupHandler(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void HandleStartup()
    {
        gameInterop.SpawnRpcSurrogate();
    }
}
