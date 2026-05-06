// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System;
using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // SetMagnetOn is the local apply callback behind the lever path, while
    // SetMagnetOnClientRpc is the receiver-side synchronization boundary.
    [HarmonyPatch(nameof(StartOfRound.SetMagnetOn), typeof(bool))]
    [HarmonyPrefix]
    public static void SetMagnetOnPrefix()
    {
        TryNotifyAppliedStateValidation(
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetLocalPreApply()
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOn), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnPostfix()
    {
        TryNotifyAppliedStateValidation(
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetLocalApplied()
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPrefix]
    public static void SetMagnetOnClientRpcPrefix()
    {
        TryNotifyAppliedStateValidation(
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetClientRpcPreApply()
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnClientRpcPostfix()
    {
        TryNotifyAppliedStateValidation(
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetClientRpcApplied()
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
