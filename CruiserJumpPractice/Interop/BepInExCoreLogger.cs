#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop;

// Logging is intentionally a tiny adapter: Core can report what happened, while the BepInEx-facing
// layer decides how those messages are emitted in the mod loader environment.
internal sealed class BepInExCoreLogger : ICoreLogger
{
    private readonly ManualLogSource logger;

    public BepInExCoreLogger(ManualLogSource logger)
    {
        this.logger = logger;
    }

    public void LogInfo(string message)
    {
        logger.LogInfo(message);
    }

    public void LogError(string message)
    {
        logger.LogError(message);
    }
}
