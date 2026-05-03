#nullable enable

namespace CruiserJumpPractice.Core.Ports;

// Use cases log decisions and failure details, but the Core layer should not depend on the
// BepInEx logging API. Interop supplies the adapter that chooses the actual log sink.
internal interface ICoreLogger
{
    void LogInfo(string message);

    void LogError(string message);
}
