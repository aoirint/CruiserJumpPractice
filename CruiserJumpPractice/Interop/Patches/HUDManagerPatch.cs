#nullable enable

extern alias LethalCompany73;

using BepInEx.Logging;
using HarmonyLib;
using LethalCompany73;

namespace CruiserJumpPractice.Interop.Patches;

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
