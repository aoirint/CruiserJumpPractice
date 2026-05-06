// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System;
using CruiserJumpPractice.Interop.Game.Adapters;
using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // SetMagnetOn is the local apply callback behind the lever path, while
    // SetMagnetOnClientRpc is the receiver-side synchronization boundary.
    [HarmonyPatch(nameof(StartOfRound.SetMagnetOn), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnPostfix(StartOfRound __instance, bool on)
    {
        TryNotifyAppliedStateValidation(
            notify: () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetLocalApplied(
                    expectedAfter: on,
                    observedAfter: ShipMagnetAdapter.GetMagnetOn(startOfRound: __instance)
                )
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnClientRpcPostfix(StartOfRound __instance, bool on)
    {
        TryNotifyAppliedStateValidation(
            notify: () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetClientRpcApplied(
                    expectedAfter: on,
                    observedAfter: ShipMagnetAdapter.GetMagnetOn(startOfRound: __instance)
                )
        );
    }

    private static void TryNotifyAppliedStateValidation(Action notify)
    {
        try
        {
            notify();
        }
        catch
        {
            // Validation logging must never interrupt the base-game apply path.
        }
    }
}
