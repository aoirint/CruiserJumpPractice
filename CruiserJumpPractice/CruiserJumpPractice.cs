#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application;
using CruiserJumpPractice.Application.Runtime;
using CruiserJumpPractice.Application.Services;
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

    private static ApplicationComposition? App { get; set; }

    internal static FrameHandler FrameHandler =>
        App!.FrameHandler;

    internal static StartupHandler StartupHandler =>
        App!.StartupHandler;

    internal static IGameInterop GameInterop =>
        App!.GameInterop;

    internal static ServerCruiserStateService ServerCruiserStateService =>
        App!.ServerCruiserStateService;

    internal static ClientCruiserStateService ClientCruiserStateService =>
        App!.ClientCruiserStateService;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        App = ApplicationComposition.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
