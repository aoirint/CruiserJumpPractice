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
    public static void AwakePostfix(HUDManager __instance)
    {
        CruiserJumpPractice.GameInterop.SpawnRpcSurrogate(__instance);
    }

    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix(HUDManager __instance)
    {
        if (!CruiserJumpPractice.GameInterop.IsClient())
        {
            return;
        }

        if (CruiserJumpPractice.GameInterop.IsLocalPlayerBusy())
        {
            return;
        }

        UpdateSaveCruiser(__instance);
        UpdateLoadCruiser(__instance);
        UpdateToggleMagnet(__instance);
    }

    internal static void UpdateSaveCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        CruiserJumpPractice.ClientCruiserStateService.RequestSaveCruiserState(hudManager);
    }

    internal static void UpdateLoadCruiser(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        CruiserJumpPractice.ClientCruiserStateService.RequestLoadCruiserState(hudManager);
    }

    internal static void UpdateToggleMagnet(HUDManager hudManager)
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        CruiserJumpPractice.ClientMagnetService.ToggleMagnet(hudManager);
    }

}
