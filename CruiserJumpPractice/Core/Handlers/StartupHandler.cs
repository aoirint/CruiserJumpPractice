// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Validation;

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
        validationLogger.Record(ValidationLogRecord.HudStartup(surrogateResult));
    }
}
