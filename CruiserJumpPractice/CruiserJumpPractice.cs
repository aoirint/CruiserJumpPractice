#nullable enable

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CruiserJumpPractice;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.HardDependency)]
[BepInProcess("Lethal Company.exe")]
public class CruiserJumpPractice : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony Harmony { get; } = new(MyPluginInfo.PLUGIN_GUID);

    private static PluginController? controller;

    // Harmony and Netcode construct their callback objects outside our construction path. This
    // static entry exposes one plugin-level controller instead of scattering use cases across
    // patch and NetworkBehaviour classes.
    internal static PluginController Controller => controller!;

    private void Awake()
    {
        Logger = base.Logger;

        controller = PluginController.Create(Logger);

        // Startup order matters: construct the controller before patching so the first game
        // callback can enter a fully wired plugin boundary.
        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
