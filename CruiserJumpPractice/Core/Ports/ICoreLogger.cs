#nullable enable

namespace CruiserJumpPractice.Core.Ports;

// Use cases report decisions and caught failures through this port so Core can be explicit about
// diagnostics without referencing ManualLogSource.
internal interface ICoreLogger
{
    void LogInfo(string message);

    void LogError(string message);
}
