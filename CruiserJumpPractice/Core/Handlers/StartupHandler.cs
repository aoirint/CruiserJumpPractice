#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.Handlers;

// StartupHandler models the one game-start side effect Core needs: ensure the RPC surrogate
// exists before frame input can request server work. The Harmony patch that detects startup
// remains in Interop; Core only describes the action that should happen.
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
