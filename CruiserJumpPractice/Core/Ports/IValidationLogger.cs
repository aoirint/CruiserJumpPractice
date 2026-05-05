// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.Ports;

internal interface IValidationLogger
{
    void Record(ValidationLogRecord record);
}
