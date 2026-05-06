// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal static class VehicleControllerPatch
{
    // For the base game, AddEngineOilClientRpc is the receiver-side RPC boundary
    // for synchronized cruiser HP restoration. The local apply method below is
    // where the HP value is applied.
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

    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddEngineOilClientRpcFinalizer()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilClientRpcFinalizer,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameEngineOilClientRpcExited()
        );
    }

    // For the base game, AddEngineOilOnLocalClient applies the cruiser HP value
    // on the local client after either local or RPC-driven oil restoration.
    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPrefix]
    public static void AddEngineOilOnLocalClientPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddEngineOilOnLocalClientPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameEngineOilLocalPreApply()
        );
    }

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

    // For the base game, AddTurboBoostClientRpc is the receiver-side RPC
    // boundary for synchronized turbo count restoration. The applied count is
    // stored inside VehicleController state.
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostClientRpcPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostClientRpcPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcEntered()
        );
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddTurboBoostClientRpcFinalizer()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostClientRpcFinalizer,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcExited()
        );
    }

    // For the base game, AddTurboBoostOnLocalClient applies the turbo count on
    // the local client after either local or RPC-driven turbo restoration.
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostOnLocalClientPrefix()
    {
        HarmonyCallbackGuard.TryNotifyHarmonyCallback(
            callback: HarmonyCallbackTokens.VehicleControllerAddTurboBoostOnLocalClientPrefix,
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboLocalPreApply()
        );
    }

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
