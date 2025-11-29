#nullable enable

using BepInEx;
using BepInEx.Logging;
using CruiserJumpPractice.Managers;
using HarmonyLib;

namespace CruiserJumpPractice;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Lethal Company.exe")]
public class CruiserJumpPractice : BaseUnityPlugin
{
    internal static new ManualLogSource? Logger { get; private set; }

    internal static Harmony Harmony { get; } = new(MyPluginInfo.PLUGIN_GUID);

    internal static InputActions InputActions { get; } = new();

    internal static CruiserManager CruiserManager { get; } = new();

    private void Awake()
    {
        Logger = base.Logger;

        Harmony.PatchAll();

        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} v{MyPluginInfo.PLUGIN_VERSION} is loaded!");
    }
}
