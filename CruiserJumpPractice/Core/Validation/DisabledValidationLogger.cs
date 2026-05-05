// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.Validation;

internal sealed class DisabledValidationLogger : IValidationLogger
{
    public static DisabledValidationLogger Instance { get; } = new();

    private DisabledValidationLogger()
    {
    }

    public void Record(string eventName, params ValidationLogField[] fields)
    {
    }
}
