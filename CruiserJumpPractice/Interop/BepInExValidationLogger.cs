// SPDX-License-Identifier: Unlicense
#nullable enable

using System;
using System.Globalization;
using System.Text;
using CruiserJumpPractice.Core.Ports;

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
        var payload = new StringBuilder();
        payload.Append('{');
        AppendInt(payload, "schema", SchemaVersion, isFirst: true);
        AppendString(payload, "ts", FormatTimestamp(DateTime.UtcNow));
        AppendString(payload, "run", runId);
        AppendInt(payload, "seq", ++sequence);
        AppendString(payload, "event", eventName);

        foreach (var field in fields)
        {
            AppendField(payload, field);
        }

        payload.Append('}');
        logger.LogInfo(Prefix + payload);
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

    private static void AppendField(StringBuilder payload, ValidationLogField field)
    {
        switch (field.Kind)
        {
            case ValidationLogFieldKind.String:
                AppendString(payload, field.Name, field.StringValue ?? string.Empty);
                break;
            case ValidationLogFieldKind.Bool:
                AppendBool(payload, field.Name, field.BoolValue);
                break;
            case ValidationLogFieldKind.Int:
                AppendInt(payload, field.Name, field.IntValue);
                break;
            case ValidationLogFieldKind.Number:
                AppendNumber(payload, field.Name, field.FloatValue, field.DecimalPlaces);
                break;
            case ValidationLogFieldKind.Vector3:
                AppendVector3(payload, field.Name, field.VectorValue, field.DecimalPlaces);
                break;
            default:
                throw new InvalidOperationException($"Unsupported validation log field kind: {field.Kind}");
        }
    }

    private static void AppendString(
        StringBuilder payload,
        string name,
        string value,
        bool isFirst = false
    )
    {
        AppendName(payload, name, isFirst);
        payload.Append('"');
        foreach (var character in value)
        {
            AppendEscapedJsonCharacter(payload, character);
        }

        payload.Append('"');
    }

    private static void AppendBool(
        StringBuilder payload,
        string name,
        bool value,
        bool isFirst = false
    )
    {
        AppendName(payload, name, isFirst);
        payload.Append(value ? "true" : "false");
    }

    private static void AppendInt(
        StringBuilder payload,
        string name,
        int value,
        bool isFirst = false
    )
    {
        AppendName(payload, name, isFirst);
        payload.Append(value.ToString(CultureInfo.InvariantCulture));
    }

    private static void AppendNumber(
        StringBuilder payload,
        string name,
        float value,
        int decimalPlaces,
        bool isFirst = false
    )
    {
        AppendName(payload, name, isFirst);
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            // JSON has no NaN or Infinity literals; keep malformed Unity state parseable.
            payload.Append("null");
            return;
        }

        payload.Append(FormatNumber(value, decimalPlaces));
    }

    private static void AppendVector3(
        StringBuilder payload,
        string name,
        Core.Snapshots.Vector3Value value,
        int decimalPlaces
    )
    {
        AppendName(payload, name, isFirst: false);
        payload.Append('[');
        payload.Append(FormatNumber(value.X, decimalPlaces));
        payload.Append(',');
        payload.Append(FormatNumber(value.Y, decimalPlaces));
        payload.Append(',');
        payload.Append(FormatNumber(value.Z, decimalPlaces));
        payload.Append(']');
    }

    private static string FormatNumber(float value, int decimalPlaces)
    {
        var rounded = Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
        return rounded.ToString("F" + decimalPlaces.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
    }

    private static void AppendName(StringBuilder payload, string name, bool isFirst)
    {
        if (!isFirst)
        {
            payload.Append(',');
        }

        payload.Append('"');
        payload.Append(name);
        payload.Append("\":");
    }

    private static void AppendEscapedJsonCharacter(StringBuilder payload, char character)
    {
        switch (character)
        {
            case '"':
                payload.Append("\\\"");
                break;
            case '\\':
                payload.Append("\\\\");
                break;
            case '\b':
                payload.Append("\\b");
                break;
            case '\f':
                payload.Append("\\f");
                break;
            case '\n':
                payload.Append("\\n");
                break;
            case '\r':
                payload.Append("\\r");
                break;
            case '\t':
                payload.Append("\\t");
                break;
            default:
                if (char.IsControl(character))
                {
                    payload.Append("\\u");
                    payload.Append(((int)character).ToString("x4", CultureInfo.InvariantCulture));
                }
                else
                {
                    payload.Append(character);
                }

                break;
        }
    }
}
