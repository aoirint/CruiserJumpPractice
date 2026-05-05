// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System;
using CruiserJumpPractice.Core.Validation;
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
                HandleAppliedState(
                    instance: __instance,
                    expectedAfter: on,
                    source: ValidationLogBaseGameApplySource.LocalApply
                )
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnClientRpcPostfix(StartOfRound __instance, bool on)
    {
        TryNotifyAppliedStateValidation(
            notify: () =>
                HandleAppliedState(
                    instance: __instance,
                    expectedAfter: on,
                    source: ValidationLogBaseGameApplySource.ClientRpcApply
                )
        );
    }

    private static void HandleAppliedState(
        StartOfRound instance,
        bool expectedAfter,
        ValidationLogBaseGameApplySource source
    )
    {
        CruiserJumpPractice.Controller.HandleBaseGameShipMagnetApplied(
            expectedAfter: expectedAfter,
            observedAfter: instance.magnetOn,
            source: source
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
