// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    // For the base game, SetMagnetOn applies the local/originating ship-magnet
    // state used by the lever flow. It is not the receiver-side RPC receipt
    // boundary.
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

    // For the base game, SetMagnetOnClientRpc applies ship-magnet state on RPC
    // receivers. Its Postfix is the receiver-side final-state observation point.
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
