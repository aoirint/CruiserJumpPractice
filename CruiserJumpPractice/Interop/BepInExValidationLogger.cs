// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Globalization;
using CruiserJumpPractice.Core.Ports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CruiserJumpPractice.Interop;

internal sealed class BepInExValidationLogger : IValidationLogger
{
    private const int SchemaVersion = 1;
    private const string Prefix = "[CJP_VALIDATION] ";

    private readonly IPluginLogger logger;
    private readonly string runId;
    private int sequence;

    public BepInExValidationLogger(IPluginLogger logger, DateTime startupTimeUtc)
    {
        this.logger = logger;
        runId = CreateRunId(startupTimeUtc);
    }

    public void Record(string eventName, params ValidationLogField[] fields)
    {
        var payload = new JObject
        {
            ["schema"] = SchemaVersion,
            ["ts"] = FormatTimestamp(DateTime.UtcNow),
            ["run"] = runId,
            ["seq"] = ++sequence,
            ["event"] = eventName
        };

        foreach (var field in fields)
        {
            payload[field.Name] = CreateFieldValue(field);
        }

        logger.LogInfo(Prefix + payload.ToString(Formatting.None));
    }

    private static string CreateRunId(DateTime startupTimeUtc)
    {
        var timestamp = startupTimeUtc.ToString("yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
        var suffix = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture).Substring(0, 6);
        return timestamp + "-" + suffix;
    }

    private static string FormatTimestamp(DateTime timestampUtc)
    {
        return timestampUtc.ToUniversalTime().ToString(
            "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
            CultureInfo.InvariantCulture
        );
    }

    private static JToken CreateFieldValue(ValidationLogField field)
    {
        return field.Kind switch
        {
            ValidationLogFieldKind.String => field.StringValue ?? string.Empty,
            ValidationLogFieldKind.Bool => field.BoolValue,
            ValidationLogFieldKind.Int => field.IntValue,
            ValidationLogFieldKind.Number => CreateNumberValue(field.FloatValue, field.DecimalPlaces),
            ValidationLogFieldKind.Vector3 => CreateVector3Value(field.VectorValue, field.DecimalPlaces),
            _ => throw new InvalidOperationException($"Unsupported validation log field kind: {field.Kind}")
        };
    }

    private static JToken CreateNumberValue(float value, int decimalPlaces)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return JValue.CreateNull();
        }

        return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
    }

    private static JArray CreateVector3Value(
        Core.Snapshots.Vector3Value value,
        int decimalPlaces
    )
    {
        return new JArray(
            CreateNumberValue(value.X, decimalPlaces),
            CreateNumberValue(value.Y, decimalPlaces),
            CreateNumberValue(value.Z, decimalPlaces)
        );
    }
}
