// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.Ports;

/// <summary>
/// Logger port used by Core use cases and game interop adapters.
/// </summary>
/// <remarks>
/// Diagnostics stay independent of the mod-loader logger implementation.
/// </remarks>
internal interface IPluginLogger
{
    void LogDebug(string message);

    void LogInfo(string message);

    void LogError(string message);
}
