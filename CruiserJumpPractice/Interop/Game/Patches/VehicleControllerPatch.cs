// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal static class VehicleControllerPatch
{
    // AddEngineOilClientRpc marks the receiver-side vanilla synchronization
    // boundary. The local apply method below is the final-state hook used after
    // both initiating and receiver-side paths update the cruiser HP.
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

    // AddEngineOilOnLocalClient is the local apply point identified as the
    // first HP final-state observation hook.
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

    // AddTurboBoostClientRpc uses the same receiver-side synchronization shape
    // as engine oil. The applied turbo count is private, so final-state reads
    // stay behind the adapter boundary.
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

    // AddTurboBoostOnLocalClient is the local apply point identified as the
    // first turbo-count final-state observation hook.
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
