// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

// HUDManager is patched only to find lifecycle moments the game already owns. Once those moments
// are found, work is delegated to PluginController rather than embedding practice logic here.
[HarmonyPatch(typeof(HUDManager))]
internal static class HUDManagerPatch
{
    // For the base game, Awake initializes HUD state and UI references. The
    // Postfix waits until that setup finishes before plugin startup work runs.
    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.HudManagerAwakePostfix,
            notify: static () => CruiserJumpPractice.Controller.HandleStartup()
        );
    }

    // For the base game, Update is the HUD's per-frame UI loop. The Postfix
    // runs plugin frame work after the base HUD frame update has completed.
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
