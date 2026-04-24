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

    private static CompositionRoot? Services { get; set; }

    internal static Services.Client.ClientCruiserStateCoordinator ClientCruiserStateCoordinator =>
        Services!.ClientCruiserStateCoordinator;

    internal static Presentation.FrameInputCoordinator FrameInputCoordinator =>
        Services!.FrameInputCoordinator;

    internal static Presentation.StartupInitializer StartupInitializer =>
        Services!.StartupInitializer;

    internal static IGameInterop GameInterop =>
        Services!.GameInterop;

    internal static Services.Server.ServerCruiserStateCoordinator ServerCruiserStateCoordinator =>
        Services!.ServerCruiserStateCoordinator;

    internal static Services.Client.ClientCruiserResultPresenter ClientCruiserResultPresenter =>
        Services!.ClientCruiserResultPresenter;

    internal static Services.Client.ClientMagnetCoordinator ClientMagnetCoordinator =>
        Services!.ClientMagnetCoordinator;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Services = CompositionRoot.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
