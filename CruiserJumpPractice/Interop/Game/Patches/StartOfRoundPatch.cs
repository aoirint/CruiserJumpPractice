// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // SetMagnetOn is the local/originating ship-magnet apply path behind the
    // lever flow. It is useful local-state evidence, but not receiver-side RPC
    // receipt evidence.
    [HarmonyPatch(nameof(StartOfRound.SetMagnetOn), typeof(bool))]
    [HarmonyPrefix]
    public static void SetMagnetOnPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundSetMagnetOnPrefix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetLocalPreApply()
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOn), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundSetMagnetOnPostfix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetLocalApplied()
        );
    }

    // SetMagnetOnClientRpc is the receiver-side synchronization boundary for
    // ship magnet state. Its Postfix is the primary receiver-side final-state
    // observation point identified by the base-game investigation.
    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPrefix]
    public static void SetMagnetOnClientRpcPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundSetMagnetOnClientRpcPrefix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetClientRpcPreApply()
        );
    }

    [HarmonyPatch(nameof(StartOfRound.SetMagnetOnClientRpc), typeof(bool))]
    [HarmonyPostfix]
    public static void SetMagnetOnClientRpcPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.StartOfRoundSetMagnetOnClientRpcPostfix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameShipMagnetClientRpcApplied()
        );
    }
}
