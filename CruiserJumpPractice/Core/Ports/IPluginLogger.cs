// SPDX-License-Identifier: Unlicense
#nullable enable

namespace CruiserJumpPractice.Core.Ports;

// Core use cases and game interop adapters report decisions and caught failures
// through this port so diagnostics stay independent of the mod-loader logger.
internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogError(string message);
}
