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

    internal static Services.Client.ClientCruiserStateService ClientCruiserStateService =>
        Services!.ClientCruiserStateService;

    internal static Services.Runtime.ClientFrameHandler ClientFrameHandler =>
        Services!.ClientFrameHandler;

    internal static Services.Runtime.ClientStartupHandler ClientStartupHandler =>
        Services!.ClientStartupHandler;

    internal static IGameInterop GameInterop =>
        Services!.GameInterop;

    internal static Services.Server.ServerCruiserStateService ServerCruiserStateService =>
        Services!.ServerCruiserStateService;

    internal static Services.Client.ClientMagnetService ClientMagnetService =>
        Services!.ClientMagnetService;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Services = ServiceRegistry.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
