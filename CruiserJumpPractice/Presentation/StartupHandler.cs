#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Presentation;

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