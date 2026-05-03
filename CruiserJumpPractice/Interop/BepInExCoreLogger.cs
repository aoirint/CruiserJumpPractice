// SPDX-License-Identifier: Unlicense
#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop;

// Core log messages are routed through BepInEx only at the edge of the plugin. That keeps
// ManualLogSource out of use cases while preserving normal mod-loader output.
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
