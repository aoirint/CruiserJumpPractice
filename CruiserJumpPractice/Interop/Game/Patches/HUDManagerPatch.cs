// SPDX-License-Identifier: Unlicense
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

// HUDManager is patched only to find lifecycle moments the game already owns. Once those moments
// are found, work is delegated to PluginController rather than embedding practice logic here.
[HarmonyPatch(typeof(HUDManager))]
internal class HUDManagerPatch
{
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
