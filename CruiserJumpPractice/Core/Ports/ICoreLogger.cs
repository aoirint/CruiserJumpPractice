#nullable enable

namespace CruiserJumpPractice.Core.Ports;

internal interface ICoreLogger
{
    void LogInfo(string message);

    void LogError(string message);
}
