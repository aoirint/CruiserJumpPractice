#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Application.UseCases;
using CruiserJumpPractice.Composition;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Presentation;
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

    internal static FrameInputCoordinator FrameInputCoordinator =>
        Root!.FrameInputCoordinator;

    internal static StartupInitializer StartupInitializer =>
        Root!.StartupInitializer;

    internal static IGameInterop GameInterop =>
        Root!.GameInterop;

    internal static SaveCruiserStateUseCase SaveCruiserStateUseCase =>
        Root!.SaveCruiserStateUseCase;

    internal static LoadCruiserStateUseCase LoadCruiserStateUseCase =>
        Root!.LoadCruiserStateUseCase;

    internal static ClientCruiserResultPresenter ClientCruiserResultPresenter =>
        Root!.ClientCruiserResultPresenter;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Root = CompositionRoot.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
