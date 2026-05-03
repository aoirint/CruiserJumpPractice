#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Core;
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

    private static CoreComposition? Core { get; set; }

    internal static FrameHandler FrameHandler =>
        Core!.FrameHandler;

    internal static StartupHandler StartupHandler =>
        Core!.StartupHandler;

    internal static IGameInterop GameInterop =>
        Core!.GameInterop;

    internal static SaveCruiserStateUseCase SaveCruiserStateUseCase =>
        Core!.SaveCruiserStateUseCase;

    internal static LoadCruiserStateUseCase LoadCruiserStateUseCase =>
        Core!.LoadCruiserStateUseCase;

    internal static PresentSaveCruiserStateResultUseCase PresentSaveCruiserStateResultUseCase =>
        Core!.PresentSaveCruiserStateResultUseCase;

    internal static PresentLoadCruiserStateResultUseCase PresentLoadCruiserStateResultUseCase =>
        Core!.PresentLoadCruiserStateResultUseCase;

    private void Awake()
    {
        Logger = base.Logger;

        InputActions = new InputActions();
        Core = CoreComposition.Create(Logger);

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
