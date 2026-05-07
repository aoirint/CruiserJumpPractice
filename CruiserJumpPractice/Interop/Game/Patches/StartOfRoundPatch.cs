#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    /// <summary>
    /// Captures local ship-magnet state before the base-game lever apply path.
    /// </summary>
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

    /// <summary>
    /// Records local ship-magnet state after the base-game lever apply path.
    /// </summary>
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

    /// <summary>
    /// Captures ship-magnet state before the base-game ClientRpc apply path.
    /// </summary>
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

    /// <summary>
    /// Records ship-magnet state after the base-game ClientRpc apply path.
    /// </summary>
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
