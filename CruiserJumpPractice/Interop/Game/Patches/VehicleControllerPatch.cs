// SPDX-License-Identifier: MIT
#nullable enable

extern alias LethalCompany;

using System.Reflection;
using CruiserJumpPractice.Core.Validation;
using HarmonyLib;
using LethalCompany;

namespace CruiserJumpPractice.Interop.Game.Patches;

[HarmonyPatch(typeof(VehicleController))]
internal static class VehicleControllerPatch
{
    private static readonly FieldInfo? turboBoostsField = typeof(VehicleController).GetField(
        "turboBoosts",
        BindingFlags.NonPublic | BindingFlags.Instance
    );

    private static int engineOilClientRpcDepth;
    private static int turboClientRpcDepth;
    private static bool turboBoostsFieldMissingLogged;

    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddEngineOilClientRpcPrefix()
    {
        engineOilClientRpcDepth++;
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddEngineOilClientRpcFinalizer()
    {
        if (engineOilClientRpcDepth > 0)
        {
            engineOilClientRpcDepth--;
        }
    }

    [HarmonyPatch(nameof(VehicleController.AddEngineOilOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddEngineOilOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        if (engineOilClientRpcDepth <= 0)
        {
            return;
        }

        try
        {
            CruiserJumpPractice.Controller.RecordBaseGameEngineOilApplied(
                expectedHP: addedAmount,
                observedHP: __instance.carHP,
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            );
        }
        catch (System.Exception error)
        {
            LogPatchError(nameof(AddEngineOilOnLocalClientPostfix), error);
        }
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyPrefix]
    public static void AddTurboBoostClientRpcPrefix()
    {
        turboClientRpcDepth++;
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostClientRpc), typeof(int), typeof(int))]
    [HarmonyFinalizer]
    public static void AddTurboBoostClientRpcFinalizer()
    {
        if (turboClientRpcDepth > 0)
        {
            turboClientRpcDepth--;
        }
    }

    [HarmonyPatch(nameof(VehicleController.AddTurboBoostOnLocalClient), typeof(int))]
    [HarmonyPostfix]
    public static void AddTurboBoostOnLocalClientPostfix(VehicleController __instance, int addedAmount)
    {
        if (turboClientRpcDepth <= 0)
        {
            return;
        }

        if (turboBoostsField == null)
        {
            LogTurboBoostsFieldMissing();
            return;
        }

        try
        {
            var observedTurbo = turboBoostsField.GetValue(__instance);
            if (observedTurbo is not int turboBoosts)
            {
                return;
            }

            CruiserJumpPractice.Controller.RecordBaseGameTurboApplied(
                expectedTurbo: addedAmount,
                observedTurbo: turboBoosts,
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            );
        }
        catch (System.Exception error)
        {
            LogPatchError(nameof(AddTurboBoostOnLocalClientPostfix), error);
        }
    }

    private static void LogTurboBoostsFieldMissing()
    {
        if (turboBoostsFieldMissingLogged)
        {
            return;
        }

        turboBoostsFieldMissingLogged = true;
        LogPatchError(
            nameof(AddTurboBoostOnLocalClientPostfix),
            new System.MissingFieldException(nameof(VehicleController), "turboBoosts")
        );
    }

    private static void LogPatchError(string hookName, System.Exception error)
    {
        try
        {
            CruiserJumpPractice.Controller.LogValidationPatchError(
                $"{nameof(VehicleControllerPatch)}.{hookName}",
                error
            );
        }
        catch
        {
            // Validation logging must never interrupt the base-game apply path.
        }
    }
}
