// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

/// <summary>
/// Harmony patches for HUD lifecycle moments that CJP uses as plugin hooks.
/// </summary>
/// <remarks>
/// Work is delegated to PluginController rather than embedding practice logic here.
/// </remarks>
[HarmonyPatch(typeof(HUDManager))]
internal static class HUDManagerPatch
{
    /// <summary>
    /// Runs plugin startup after the base-game HUD Awake setup finishes.
    /// </summary>
    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.HudManagerAwakePostfix,
            notify: static () => CruiserJumpPractice.Controller.HandleStartup()
        );
    }

    /// <summary>
    /// Runs plugin frame work after the base-game HUD frame update completes.
    /// </summary>
    [HarmonyPatch(nameof(HUDManager.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.HudManagerUpdatePostfix,
            notify: static () => CruiserJumpPractice.Controller.HandleFrame()
        );
    }

}
