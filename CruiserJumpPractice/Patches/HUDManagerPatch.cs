#nullable enable

using BepInEx.Logging;
using HarmonyLib;
using CruiserJumpPractice.Utils;

namespace CruiserJumpPractice.Patches;

[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(HUDManager __instance)
    {
        if (!NetworkUtils.IsClient())
        {
            return;
        }

        if (CruiserJumpPractice.InputActions.SaveCruiserKey?.triggered ?? false)
        {
            CruiserJumpPractice.CruiserManager.SaveCruiser();
        }

        if (CruiserJumpPractice.InputActions.LoadCruiserKey?.triggered ?? false)
        {
            CruiserJumpPractice.CruiserManager.LoadCruiser();
        }
    }
}
