// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal static class VehicleControllerPatch
{
    // The local apply method is also used by the host restore path, so the ClientRpc wrapper
    // marks only receiver-side vanilla synchronization before the shared local helper runs.
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

    // Turbo uses the same ClientRpc/local-apply split as engine oil, but the applied value is
    // private in VehicleController and has to be read through the adapter boundary.
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
