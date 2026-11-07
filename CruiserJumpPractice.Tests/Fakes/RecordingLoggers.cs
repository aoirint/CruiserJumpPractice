using System.Collections.Generic;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Tests.Fakes;

internal sealed class RecordingPluginLogger : IPluginLogger
{
    public List<string> Errors { get; } = new();
    public List<string> Infos { get; } = new();
    public void LogDebug(string message) { }
    public void LogInfo(string message) => Infos.Add(message);
    public void LogError(string message) => Errors.Add(message);
}

internal sealed class RecordingValidationLogger : IValidationLogger
{
    public List<ValidationLogRecord> Records { get; } = new();
    public void Record(ValidationLogRecord record) => Records.Add(record);
}
