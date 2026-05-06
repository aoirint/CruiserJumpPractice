// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Interop.Game.Patches;

internal static class HarmonyCallbackTokens
{
    // Tokens are stable validation identifiers, not display text. Use
    // class_method_patchkind naming so SDC and CJP diagnostics stay comparable.
    // Keep tokens grouped by patched game class so new callbacks land near
    // related patches.

    // HUDManager callbacks.
    public const string HudManagerAwakePostfix = "hud_manager_awake_postfix";
    public const string HudManagerUpdatePostfix = "hud_manager_update_postfix";

    // StartOfRound callbacks.
    public const string StartOfRoundSetMagnetOnPrefix = "start_of_round_set_magnet_on_prefix";
    public const string StartOfRoundSetMagnetOnPostfix = "start_of_round_set_magnet_on_postfix";
    public const string StartOfRoundSetMagnetOnClientRpcPrefix =
        "start_of_round_set_magnet_on_client_rpc_prefix";
    public const string StartOfRoundSetMagnetOnClientRpcPostfix =
        "start_of_round_set_magnet_on_client_rpc_postfix";

    // VehicleController callbacks.
    public const string VehicleControllerAddEngineOilClientRpcPrefix =
        "vehicle_controller_add_engine_oil_client_rpc_prefix";
    public const string VehicleControllerAddEngineOilClientRpcFinalizer =
        "vehicle_controller_add_engine_oil_client_rpc_finalizer";
    public const string VehicleControllerAddEngineOilOnLocalClientPrefix =
        "vehicle_controller_add_engine_oil_on_local_client_prefix";
    public const string VehicleControllerAddEngineOilOnLocalClientPostfix =
        "vehicle_controller_add_engine_oil_on_local_client_postfix";
    public const string VehicleControllerAddTurboBoostClientRpcPrefix =
        "vehicle_controller_add_turbo_boost_client_rpc_prefix";
    public const string VehicleControllerAddTurboBoostClientRpcFinalizer =
        "vehicle_controller_add_turbo_boost_client_rpc_finalizer";
    public const string VehicleControllerAddTurboBoostOnLocalClientPrefix =
        "vehicle_controller_add_turbo_boost_on_local_client_prefix";
    public const string VehicleControllerAddTurboBoostOnLocalClientPostfix =
        "vehicle_controller_add_turbo_boost_on_local_client_postfix";
}
