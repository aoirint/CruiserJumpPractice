// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Interop.Game.Patches;

internal static class HarmonyCallbackTokens
{
    public const string HudManagerAwakePostfix = "hud_manager.awake.postfix";
    public const string HudManagerUpdatePostfix = "hud_manager.update.postfix";
    public const string StartOfRoundSetMagnetOnPrefix = "start_of_round.set_magnet_on.prefix";
    public const string StartOfRoundSetMagnetOnPostfix = "start_of_round.set_magnet_on.postfix";
    public const string StartOfRoundSetMagnetOnClientRpcPrefix =
        "start_of_round.set_magnet_on_client_rpc.prefix";
    public const string StartOfRoundSetMagnetOnClientRpcPostfix =
        "start_of_round.set_magnet_on_client_rpc.postfix";
    public const string VehicleControllerAddEngineOilClientRpcPrefix =
        "vehicle_controller.add_engine_oil_client_rpc.prefix";
    public const string VehicleControllerAddEngineOilClientRpcFinalizer =
        "vehicle_controller.add_engine_oil_client_rpc.finalizer";
    public const string VehicleControllerAddEngineOilOnLocalClientPrefix =
        "vehicle_controller.add_engine_oil_on_local_client.prefix";
    public const string VehicleControllerAddEngineOilOnLocalClientPostfix =
        "vehicle_controller.add_engine_oil_on_local_client.postfix";
    public const string VehicleControllerAddTurboBoostClientRpcPrefix =
        "vehicle_controller.add_turbo_boost_client_rpc.prefix";
    public const string VehicleControllerAddTurboBoostClientRpcFinalizer =
        "vehicle_controller.add_turbo_boost_client_rpc.finalizer";
    public const string VehicleControllerAddTurboBoostOnLocalClientPrefix =
        "vehicle_controller.add_turbo_boost_on_local_client.prefix";
    public const string VehicleControllerAddTurboBoostOnLocalClientPostfix =
        "vehicle_controller.add_turbo_boost_on_local_client.postfix";
}
