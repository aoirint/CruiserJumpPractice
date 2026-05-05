// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

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
        CruiserJumpPractice.Controller.EnterBaseGameEngineOilClientRpc();
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddEngineOilClientRpcFinalizer()
    {
        CruiserJumpPractice.Controller.ExitBaseGameEngineOilClientRpc();
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddEngineOilOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        try
        {
            // Keep patch code limited to live game reads; filtering and record construction stay in Core.
            CruiserJumpPractice.Controller.HandleBaseGameEngineOilLocalApplied(
                expectedHP: addedAmount,
                observedHP: __instance.carHP
            );
        }
        catch
        {
            // Validation logging must never interrupt the base-game apply path.
        }
    }

    // Turbo uses the same ClientRpc/local-apply split as engine oil, but the applied value is
    // private in VehicleController and has to be read through the adapter boundary.
    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostClientRpcPrefix()
    {
        CruiserJumpPractice.Controller.EnterBaseGameTurboClientRpc();
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddTurboBoostClientRpcFinalizer()
    {
        CruiserJumpPractice.Controller.ExitBaseGameTurboClientRpc();
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddTurboBoostOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        try
        {
            CruiserJumpPractice.Controller.HandleBaseGameTurboLocalApplied(
                expectedTurbo: addedAmount,
                observedTurbo: CruiserAdapter.GetTurboBoosts(cruiser: __instance)
            );
        }
        catch
        {
            // Validation logging must never interrupt the base-game apply path.
        }
    }
}
