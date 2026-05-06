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
    // Awake is the base HUD setup lifecycle point. CJP waits until that setup
    // finishes before running plugin startup work that depends on game UI state.
    [HarmonyPatch(nameof(HUDManager.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.HudManagerAwakePostfix,
            notify: static () => CruiserJumpPractice.Controller.HandleStartup()
        );
    }

    // Update is the base HUD frame tick. CJP observes after each tick so frame
    // work runs alongside the game loop without replacing the base HUD update.
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
