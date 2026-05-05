// SPDX-License-Identifier: MIT
#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop;

// Plugin log messages are routed through BepInEx only at the edge of the plugin.
// That keeps ManualLogSource out of Core and game interop composition.
internal sealed class BepInExPluginLogger : IPluginLogger
{
    private readonly ManualLogSource logger;

    public BepInExPluginLogger(ManualLogSource logger)
    {
        this.logger = logger;
    }

    public void LogDebug(string message)
    {
        logger.LogDebug(message);
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
