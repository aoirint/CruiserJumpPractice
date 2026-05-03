#nullable enable

extern alias LethalCompany;

using BepInEx.Logging;
using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

// HUDManager hooks are kept as thin event bridges. They identify stable game lifecycle
// moments, then immediately delegate to the plugin controller so Core and construction details
// do not leak into attributes tied to Lethal Company types.
[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix()
    {
        CruiserJumpPractice.Controller.HandleStartup();
    }

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix()
    {
        CruiserJumpPractice.Controller.HandleFrame();
    }

}
