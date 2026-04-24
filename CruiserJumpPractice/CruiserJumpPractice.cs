#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
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

    private static ServiceRegistry? Services { get; set; }

    internal static Services.Client.CruiserStateService CruiserStateService =>
        Services!.CruiserStateService;

    internal static Runtime.FrameHandler FrameHandler =>
        Services!.FrameHandler;

    internal static Runtime.StartupHandler StartupHandler =>
        Services!.StartupHandler;

    internal static IGameInterop GameInterop =>
        Services!.GameInterop;

    internal static Services.Server.ServerCruiserStateService ServerCruiserStateService =>
        Services!.ServerCruiserStateService;

    internal static Services.Client.MagnetService MagnetService =>
        Services!.MagnetService;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Services = ServiceRegistry.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
