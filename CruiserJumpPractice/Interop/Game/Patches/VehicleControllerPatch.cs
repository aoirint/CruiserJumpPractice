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
        name: "turboBoosts",
        bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance
    );

    // The local apply helpers are also used by the initiating restore path. These depth markers
    // identify calls made from inside the base-game ClientRpc path so #100 logs receiver-side
    // applied state without duplicating #97 sender-side restore observations.
    private static int engineOilClientRpcDepth;
    private static int turboClientRpcDepth;

    // Avoid log spam if a future base-game update renames the private turbo field.
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
            LogPatchError(hookName: nameof(AddEngineOilOnLocalClientPostfix), error: error);
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
            var observedTurbo = turboBoostsField.GetValue(obj: __instance);
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
            LogPatchError(hookName: nameof(AddTurboBoostOnLocalClientPostfix), error: error);
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
            hookName: nameof(AddTurboBoostOnLocalClientPostfix),
            error: new System.MissingFieldException(
                className: nameof(VehicleController),
                fieldName: "turboBoosts"
            )
        );
    }

    private static void LogPatchError(string hookName, System.Exception error)
    {
        try
        {
            CruiserJumpPractice.Controller.LogValidationPatchError(
                hookName: $"{nameof(VehicleControllerPatch)}.{hookName}",
                error: error
            );
        }
        catch
        {
            // Patch diagnostics are best-effort because this code runs inside base-game callbacks.
        }
    }
}
