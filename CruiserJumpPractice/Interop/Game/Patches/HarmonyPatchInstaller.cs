// SPDX-License-Identifier: Unlicense
#nullable enable

using HarmonyLib;

namespace CruiserJumpPractice.Interop.Game.Patches;

internal static class HarmonyPatchInstaller
{
    private static readonly Harmony Harmony = new(MyPluginInfo.PLUGIN_GUID);

    public static void Install()
    {
        // Patch this assembly explicitly so Harmony ownership stays with the Interop patch layer.
        Harmony.PatchAll(typeof(HarmonyPatchInstaller).Assembly);
    }
}
