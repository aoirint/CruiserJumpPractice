// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal static class VehicleControllerPatch
{
    /// <summary>
    /// Marks entry into the receiver-side engine-oil RPC boundary.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddEngineOilClientRpcPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilClientRpcPrefix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameEngineOilClientRpcEntered()
        );
    }

    /// <summary>
    /// Marks exit from the receiver-side engine-oil RPC boundary.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddEngineOilClientRpcFinalizer()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilClientRpcFinalizer,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameEngineOilClientRpcExited()
        );
    }

    /// <summary>
    /// Captures cruiser HP before the base-game local engine-oil apply helper runs.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPrefix]
    public static void AddEngineOilOnLocalClientPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilOnLocalClientPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameEngineOilLocalPreApply()
        );
    }

    /// <summary>
    /// Records cruiser HP after the base-game local engine-oil apply helper runs.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddEngineOilOnLocalClientPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilOnLocalClientPostfix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameEngineOilLocalApplied()
        );
    }

    /// <summary>
    /// Marks entry into the receiver-side turbo RPC boundary.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostClientRpcPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostClientRpcPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcEntered()
        );
    }

    /// <summary>
    /// Marks exit from the receiver-side turbo RPC boundary.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddTurboBoostClientRpcFinalizer()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostClientRpcFinalizer,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcExited()
        );
    }

    /// <summary>
    /// Captures turbo count before the base-game local turbo apply helper runs.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostOnLocalClientPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostOnLocalClientPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboLocalPreApply()
        );
    }

    /// <summary>
    /// Records turbo count after the base-game local turbo apply helper runs.
    /// </summary>
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddTurboBoostOnLocalClientPostfix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostOnLocalClientPostfix,
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameTurboLocalApplied()
        );
    }
}
