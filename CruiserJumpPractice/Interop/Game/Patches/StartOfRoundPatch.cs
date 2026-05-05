// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

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
        RecordAppliedState(
            instance: __instance,
            expectedAfter: on,
            source: ValidationLogBaseGameApplySource.LocalApply
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnClientRpcPostfix(StartOfRound __instance, bool on)
    {
        RecordAppliedState(
            instance: __instance,
            expectedAfter: on,
            source: ValidationLogBaseGameApplySource.ClientRpcApply
        );
    }

    private static void RecordAppliedState(
        StartOfRound instance,
        bool expectedAfter,
        ValidationLogBaseGameApplySource source
    )
    {
        try
        {
            CruiserJumpPractice.Controller.RecordBaseGameShipMagnetApplied(
                expectedAfter: expectedAfter,
                observedAfter: instance.magnetOn,
                source: source
            );
        }
        catch (System.Exception error)
        {
            LogPatchError(nameof(RecordAppliedState), error);
        }
    }

    private static void LogPatchError(string hookName, System.Exception error)
    {
        try
        {
            CruiserJumpPractice.Controller.LogValidationPatchError(
                $"{nameof(StartOfRoundPatch)}.{hookName}",
                error
            );
        }
        catch
        {
            // Patch diagnostics are best-effort because this code runs inside base-game callbacks.
        }
    }
}
