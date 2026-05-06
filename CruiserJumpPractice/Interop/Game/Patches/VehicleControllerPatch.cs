// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System;
using CruiserJumpPractice.Interop.Game.Adapters;
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
        TryNotifyAppliedStateValidation(
            notify: static () =>
                CruiserJumpPractice.Controller.HandleBaseGameEngineOilClientRpcEntered()
        );
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddEngineOilClientRpcFinalizer()
    {
        TryNotifyAppliedStateValidation(
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameEngineOilClientRpcExited()
        );
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddEngineOilOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        TryNotifyAppliedStateValidation(
            notify: () =>
            {
                // Keep patch code limited to live game reads; filtering and record construction stay in Core.
                CruiserJumpPractice.Controller.HandleBaseGameEngineOilLocalApplied(
                    expectedHP: addedAmount,
                    observedHP: CruiserAdapter.GetCarHP(cruiser: __instance)
                );
            }
        );
    }

    // Turbo uses the same ClientRpc/local-apply split as engine oil, but the applied value is
    // private in VehicleController and has to be read through the adapter boundary.
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostClientRpcPrefix()
    {
        TryNotifyAppliedStateValidation(
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcEntered()
        );
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddTurboBoostClientRpcFinalizer()
    {
        TryNotifyAppliedStateValidation(
            notify: static () => CruiserJumpPractice.Controller.HandleBaseGameTurboClientRpcExited()
        );
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddTurboBoostOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        TryNotifyAppliedStateValidation(
            notify: () =>
            {
                CruiserJumpPractice.Controller.HandleBaseGameTurboLocalApplied(
                    expectedTurbo: addedAmount,
                    observedTurbo: CruiserAdapter.GetTurboBoosts(cruiser: __instance)
                );
            }
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
