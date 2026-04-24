#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Presentation;

internal sealed class StartupInitializer
{
    private readonly IGameInterop gameInterop;

    public StartupInitializer(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void HandleStartup()
    {
        gameInterop.SpawnRpcSurrogate();
    }
}