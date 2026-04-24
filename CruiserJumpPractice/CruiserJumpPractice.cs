#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Services;
using HarmonyLib;

namespace CruiserJumpPractice;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
[BepInProcess("Lethal Company.exe")]
public class CruiserJumpPractice : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony Harmony { get; } = new(MyPluginInfo.PLUGIN_GUID);

    internal static InputActions? InputActions { get; private set; }

    internal static CruiserStateClientService CruiserStateClientService { get; } = new();

    internal static CruiserStateServerService CruiserStateServerService { get; } = new();

    internal static CustomRpcSurrogateService CustomRpcSurrogateService { get; } = new();

    internal static MagnetClientService MagnetClientService { get; } = new();

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
