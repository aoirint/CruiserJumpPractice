// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.Ports;

internal interface IValidationLogger
{
    void Record(string eventName, Dictionary<string, object?>? fields = null);
}

internal static class ValidationLogData
{
    public static object? Number(float value, int decimalPlaces)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return null;
        }

        return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
    }

    public static object?[] Vector3(Vector3Value value, int decimalPlaces)
    {
        return
        [
            Number(value.X, decimalPlaces),
            Number(value.Y, decimalPlaces),
            Number(value.Z, decimalPlaces)
        ];
    }
}
