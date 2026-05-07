#nullable enable

using BepInEx.Configuration;
using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop;

internal sealed class BepInExPluginConfig : IPluginConfig
{
    private readonly ConfigEntry<bool> validationLoggingConfig;

    private BepInExPluginConfig(ConfigEntry<bool> validationLoggingConfig)
    {
        this.validationLoggingConfig = validationLoggingConfig;
    }

    public bool ValidationLogging => validationLoggingConfig.Value;

    public static BepInExPluginConfig Bind(ConfigFile config)
    {
        var validationLoggingConfig = config.Bind(
            "Debug",
            "ValidationLogging",
            false,
            "Enable structured validation logs for release validation and troubleshooting."
        );

        return new BepInExPluginConfig(validationLoggingConfig: validationLoggingConfig);
    }
}
