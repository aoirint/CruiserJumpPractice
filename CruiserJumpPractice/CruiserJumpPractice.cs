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

    // Patched game methods and NetworkBehaviour callbacks are constructed by the game, not by us.
    // The static access point is deliberately narrow so those objects can reach plugin actions
    // without exposing Core use cases or interop adapters one by one.
    internal static PluginController Controller => controller!;

    private void Awake()
    {
        Logger = base.Logger;

        controller = PluginController.Create(Logger);

        // Harmony is still invoked at the BepInEx entrypoint so startup order is obvious:
        // build the plugin controller first, then let patched game callbacks enter through it.
        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
