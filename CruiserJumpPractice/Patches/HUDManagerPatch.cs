#nullable enable

using BepInEx.Logging;
using HarmonyLib;

namespace CruiserJumpPractice.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix()
    {
        CruiserJumpPractice.StartupHandler.HandleStartup();
    }

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix()
    {
        CruiserJumpPractice.FrameHandler.HandleFrame();
    }

}
