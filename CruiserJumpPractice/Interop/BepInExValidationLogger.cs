// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using CruiserJumpPractice.Core.Ports;
using Newtonsoft.Json;

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

    public void Record(string eventName, Dictionary<string, object?>? fields = null)
    {
        var payload = new Dictionary<string, object?>
        {
            ["schema"] = SchemaVersion,
            ["ts"] = FormatTimestamp(DateTime.UtcNow),
            ["run"] = runId,
            ["seq"] = ++sequence,
            ["event"] = eventName
        };

        if (fields != null)
        {
            foreach (var field in fields)
            {
                payload[field.Key] = field.Value;
            }
        }

        logger.LogInfo(Prefix + JsonConvert.SerializeObject(payload, Formatting.None));
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

}
