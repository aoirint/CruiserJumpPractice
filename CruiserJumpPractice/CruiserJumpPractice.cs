#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Composition;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Runtime;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.UseCases.Server;
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

    private static PluginComposition? Composition { get; set; }

    internal static FrameHandler FrameHandler =>
        Composition!.FrameHandler;

    internal static StartupHandler StartupHandler =>
        Composition!.StartupHandler;

    internal static IGameInterop GameInterop =>
        Composition!.GameInterop;

    internal static SaveCruiserStateUseCase SaveCruiserStateUseCase =>
        Composition!.SaveCruiserStateUseCase;

    internal static LoadCruiserStateUseCase LoadCruiserStateUseCase =>
        Composition!.LoadCruiserStateUseCase;

    internal static PresentSaveCruiserStateResultUseCase PresentSaveCruiserStateResultUseCase =>
        Composition!.PresentSaveCruiserStateResultUseCase;

    internal static PresentLoadCruiserStateResultUseCase PresentLoadCruiserStateResultUseCase =>
        Composition!.PresentLoadCruiserStateResultUseCase;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Composition = PluginComposition.Create(Logger, InputActions);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
