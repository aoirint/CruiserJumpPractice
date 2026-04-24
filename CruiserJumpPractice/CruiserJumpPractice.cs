#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Composition;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Presentation;
using CruiserJumpPractice.Application.Coordinators;
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

    private static CompositionRoot? Root { get; set; }

    internal static ClientCruiserStateCoordinator ClientCruiserStateCoordinator =>
        Root!.ClientCruiserStateCoordinator;

    internal static FrameInputCoordinator FrameInputCoordinator =>
        Root!.FrameInputCoordinator;

    internal static StartupInitializer StartupInitializer =>
        Root!.StartupInitializer;

    internal static IGameInterop GameInterop =>
        Root!.GameInterop;

    internal static ServerCruiserStateCoordinator ServerCruiserStateCoordinator =>
        Root!.ServerCruiserStateCoordinator;

    internal static ClientCruiserResultPresenter ClientCruiserResultPresenter =>
        Root!.ClientCruiserResultPresenter;

    internal static ClientMagnetCoordinator ClientMagnetCoordinator =>
        Root!.ClientMagnetCoordinator;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Root = CompositionRoot.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
