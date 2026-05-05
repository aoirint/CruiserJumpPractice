// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.Validation;

internal sealed class DisabledValidationLogger : IValidationLogger
{
    public static DisabledValidationLogger Instance { get; } = new();

    private DisabledValidationLogger()
    {
    }

    public void Record(ValidationLogRecord record)
    {
    }
}
