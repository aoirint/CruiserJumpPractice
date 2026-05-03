#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop;

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
