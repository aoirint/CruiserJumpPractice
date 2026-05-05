// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.Handlers;

// On HUD startup, practice mode needs its RPC surrogate present before input can request server
// work. Interop detects the HUD lifecycle event; Core names the side effect to perform.
internal sealed class StartupHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;

    public StartupHandler(IGameInterop gameInterop, IValidationLogger validationLogger)
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
    }

    public void HandleStartup()
    {
        var surrogateResult = gameInterop.SpawnRpcSurrogate();
        validationLogger.Record(
            "hud_startup",
            ValidationLogField.String("surrogate", ToSurrogateResultToken(surrogateResult))
        );
    }

    private static string ToSurrogateResultToken(RpcSurrogateSpawnResult result)
    {
        return result switch
        {
            RpcSurrogateSpawnResult.Added => "added",
            RpcSurrogateSpawnResult.Reused => "reused",
            RpcSurrogateSpawnResult.Missing => "missing",
            RpcSurrogateSpawnResult.Error => "error",
            _ => "error"
        };
    }
}
