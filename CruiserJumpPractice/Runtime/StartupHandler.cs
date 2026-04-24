#nullable enable

using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Runtime;

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