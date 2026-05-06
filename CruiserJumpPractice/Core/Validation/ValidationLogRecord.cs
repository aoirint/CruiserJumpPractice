// SPDX-License-Identifier: MIT
#nullable enable

using System;
using System.Collections.Generic;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;

namespace CruiserJumpPractice.Core.Validation;

internal enum ValidationLogRole
{
    Host,
    Client
}

internal enum ValidationLogInputAction
{
    Save,
    Load,
    ToggleMagnet
}

internal enum ValidationLogRpcSurrogateResolveSource
{
    Cache,
    Lookup
}

internal enum ValidationLogRpcSurrogateResolveResult
{
    Success,
    Error
}

// The same base-game state can be applied by a local helper or by a ClientRpc receiver path.
// Preserve that source in validation logs so restore-side and sync-side observations are separable.
internal enum ValidationLogBaseGameApplySource
{
    LocalApply,
    ClientRpcApply,
    Unknown
}

internal sealed class ValidationLogRecord
{
    private ValidationLogRecord(string eventName, Dictionary<string, object?>? fields = null)
    {
        EventName = eventName;
        Fields = fields;
    }

    public string EventName { get; }

    public Dictionary<string, object?>? Fields { get; }

    public static ValidationLogRecord PluginLoaded(string version, bool validationLogging)
    {
        return new(
            "plugin_loaded",
            new()
            {
                ["version"] = version,
                ["validation_logging"] = validationLogging
            }
        );
    }

    public static ValidationLogRecord StateStoreCreated()
    {
        return new("state_store_created");
    }

    public static ValidationLogRecord ControllerCreated()
    {
        return new("controller_created");
    }

    public static ValidationLogRecord HudStartup(RpcSurrogateSpawnResult surrogateResult)
    {
        return new(
            "hud_startup",
            new() { ["surrogate"] = ToSurrogateResultToken(surrogateResult) }
        );
    }

    public static ValidationLogRecord InputTriggered(
        ValidationLogInputAction action,
        ValidationLogRole role
    )
    {
        return new(
            "input_triggered",
            new()
            {
                ["action"] = ToValidationActionToken(action),
                ["role"] = ToValidationRoleToken(role),
                ["busy"] = false
            }
        );
    }

    public static ValidationLogRecord InputSuppressed(
        ValidationLogInputAction action,
        ValidationLogRole role,
        LocalPlayerBusyState busyState
    )
    {
        return new(
            "input_suppressed",
            new()
            {
                ["action"] = ToValidationActionToken(action),
                ["role"] = ToValidationRoleToken(role),
                ["reason"] = busyState.GetBusyReasonToken() ?? "unknown",
                ["menu"] = busyState.IsMenuOpen,
                ["terminal"] = busyState.IsInTerminal,
                ["chat"] = busyState.IsTypingChat
            }
        );
    }

    public static ValidationLogRecord RequestSaveResult(
        ValidationLogRole role,
        RequestSaveCruiserStateResult result
    )
    {
        return Result(
            eventName: "request_save_result",
            role: role,
            result: ToValidationResultToken(result)
        );
    }

    public static ValidationLogRecord RequestLoadResult(
        ValidationLogRole role,
        RequestLoadCruiserStateResult result
    )
    {
        return Result(
            eventName: "request_load_result",
            role: role,
            result: ToValidationResultToken(result)
        );
    }

    public static ValidationLogRecord ToggleMagnetResultEvent(
        ValidationLogRole role,
        ToggleMagnetResult result
    )
    {
        return Result(
            eventName: "toggle_magnet_result",
            role: role,
            result: ToValidationResultToken(result)
        );
    }

    public static ValidationLogRecord MagnetToggle(MagnetToggleObservation observation)
    {
        return new(
            "magnet_toggle",
            new()
            {
                ["role"] = ToValidationRoleToken(ValidationLogRole.Host),
                ["before"] = ToValidationStateToken(observation.BeforeState),
                ["expected_after"] = ToValidationStateToken(observation.ExpectedAfterState),
                ["observed_after"] = ToValidationStateToken(observation.ObservedAfterState)
            }
        );
    }

    public static ValidationLogRecord SaveServerRpcReceived(ValidationLogRole role)
    {
        return Role(eventName: "save_server_rpc_received", role: role);
    }

    public static ValidationLogRecord SaveClientRpcReceived(
        ValidationLogRole role,
        SaveCruiserStateResult result
    )
    {
        return Result(
            eventName: "save_client_rpc_received",
            role: role,
            result: ToValidationResultToken(result)
        );
    }

    public static ValidationLogRecord LoadServerRpcReceived(ValidationLogRole role)
    {
        return Role(eventName: "load_server_rpc_received", role: role);
    }

    public static ValidationLogRecord LoadClientRpcReceived(
        ValidationLogRole role,
        LoadCruiserStateResult result
    )
    {
        return Result(
            eventName: "load_client_rpc_received",
            role: role,
            result: ToValidationResultToken(result)
        );
    }

    public static ValidationLogRecord SaveNoCruiserFound()
    {
        return new(
            "save_result",
            new()
            {
                ["role"] = ToValidationRoleToken(ValidationLogRole.Host),
                ["result"] = ToValidationResultToken(SaveCruiserStateResult.NoCruiserFound),
                ["cruiser_found"] = false
            }
        );
    }

    public static ValidationLogRecord SaveUnexpectedState()
    {
        return Result(
            eventName: "save_result",
            role: ValidationLogRole.Host,
            result: "unexpected_state"
        );
    }

    public static ValidationLogRecord SaveSuccess(CruiserSnapshot cruiserState)
    {
        return new(
            "save_result",
            new()
            {
                ["role"] = ToValidationRoleToken(ValidationLogRole.Host),
                ["result"] = ToValidationResultToken(SaveCruiserStateResult.Success),
                ["cruiser_found"] = true,
                ["pos"] = Vector3(cruiserState.CarPosition, decimalPlaces: 1),
                ["rot"] = Vector3(cruiserState.CarRotation, decimalPlaces: 1),
                ["hp"] = cruiserState.CarHP,
                ["turbo"] = cruiserState.TurboBoosts,
                ["steering"] = Number(cruiserState.SteeringInput, decimalPlaces: 2),
                ["rpm"] = Number(cruiserState.EngineRPM, decimalPlaces: 2)
            }
        );
    }

    public static ValidationLogRecord LoadNoCruiserFound(bool savedState)
    {
        return LoadResult(
            result: ToValidationResultToken(LoadCruiserStateResult.NoCruiserFound),
            cruiserFound: false,
            savedState: savedState,
            magneted: "unknown"
        );
    }

    public static ValidationLogRecord LoadNoSavedState()
    {
        return LoadResult(
            result: ToValidationResultToken(LoadCruiserStateResult.NoSavedState),
            cruiserFound: true,
            savedState: false,
            magneted: "unknown"
        );
    }

    public static ValidationLogRecord LoadMagnetedToShip()
    {
        return LoadResult(
            result: ToValidationResultToken(LoadCruiserStateResult.MagnetedToShip),
            cruiserFound: true,
            savedState: true,
            magneted: true
        );
    }

    public static ValidationLogRecord LoadSuccess()
    {
        return LoadResult(
            result: ToValidationResultToken(LoadCruiserStateResult.Success),
            cruiserFound: true,
            savedState: true,
            magneted: false
        );
    }

    public static ValidationLogRecord LoadUnexpectedState()
    {
        return Result(
            eventName: "load_result",
            role: ValidationLogRole.Host,
            result: "unexpected_state"
        );
    }

    public static ValidationLogRecord RestoreApplied(CruiserRestoreObservation observation)
    {
        return new(
            "restore_applied",
            new()
            {
                ["role"] = ToValidationRoleToken(ValidationLogRole.Host),
                ["saved_pos"] = Vector3(observation.SavedCarPosition, decimalPlaces: 1),
                ["saved_rot"] = Vector3(observation.SavedCarRotation, decimalPlaces: 1),
                ["before_pos"] = Vector3(observation.BeforeCarPosition, decimalPlaces: 1),
                ["after_pos"] = Vector3(observation.AfterCarPosition, decimalPlaces: 1),
                ["saved_hp"] = observation.SavedCarHP,
                ["before_hp"] = observation.BeforeCarHP,
                ["after_hp"] = observation.AfterCarHP,
                ["saved_turbo"] = observation.SavedTurboBoosts,
                ["before_turbo"] = observation.BeforeTurboBoosts,
                ["after_turbo"] = observation.AfterTurboBoosts
            }
        );
    }

    public static ValidationLogRecord BaseGameEngineOilApplied(
        ValidationLogRole role,
        int? beforeCarHP,
        int? afterCarHP,
        ValidationLogBaseGameApplySource source
    )
    {
        return new(
            eventName: "base_game_engine_oil_applied",
            fields: new()
            {
                ["role"] = ToValidationRoleToken(role: role),
                ["before_hp"] = beforeCarHP,
                ["after_hp"] = afterCarHP,
                ["source"] = ToBaseGameApplySourceToken(source: source)
            }
        );
    }

    public static ValidationLogRecord BaseGameTurboApplied(
        ValidationLogRole role,
        int? beforeTurbo,
        int? afterTurbo,
        ValidationLogBaseGameApplySource source
    )
    {
        return new(
            eventName: "base_game_turbo_applied",
            fields: new()
            {
                ["role"] = ToValidationRoleToken(role: role),
                ["before_turbo"] = beforeTurbo,
                ["after_turbo"] = afterTurbo,
                ["source"] = ToBaseGameApplySourceToken(source: source)
            }
        );
    }

    public static ValidationLogRecord BaseGameShipMagnetApplied(
        ValidationLogRole role,
        bool? before,
        bool after,
        ValidationLogBaseGameApplySource source
    )
    {
        return new(
            eventName: "base_game_ship_magnet_applied",
            fields: new()
            {
                ["role"] = ToValidationRoleToken(role: role),
                ["before"] = before,
                ["after"] = after,
                ["source"] = ToBaseGameApplySourceToken(source: source)
            }
        );
    }

    public static ValidationLogRecord HudTip(ValidationLogRole role, HudTipMessage message)
    {
        return new(
            "hud_tip",
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["message"] = message.Token
            }
        );
    }

    public static ValidationLogRecord RpcSurrogateResolved(
        ValidationLogRpcSurrogateResolveSource source,
        ValidationLogRpcSurrogateResolveResult result
    )
    {
        return new(
            "rpc_surrogate_resolved",
            new()
            {
                ["source"] = ToRpcSurrogateResolveSourceToken(source),
                ["result"] = ToRpcSurrogateResolveResultToken(result)
            }
        );
    }

    private static ValidationLogRecord LoadResult(
        string result,
        bool cruiserFound,
        bool savedState,
        object? magneted
    )
    {
        return new(
            "load_result",
            new()
            {
                ["role"] = ToValidationRoleToken(ValidationLogRole.Host),
                ["result"] = result,
                ["cruiser_found"] = cruiserFound,
                ["saved_state"] = savedState,
                ["magneted"] = magneted
            }
        );
    }

    private static ValidationLogRecord Result(
        string eventName,
        ValidationLogRole role,
        string result
    )
    {
        return new(
            eventName,
            new()
            {
                ["role"] = ToValidationRoleToken(role),
                ["result"] = result
            }
        );
    }

    private static ValidationLogRecord Role(string eventName, ValidationLogRole role)
    {
        return new(eventName, new() { ["role"] = ToValidationRoleToken(role) });
    }

    private static object? Number(float value, int decimalPlaces)
    {
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return null;
        }

        return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
    }

    private static object?[] Vector3(Vector3Value value, int decimalPlaces)
    {
        return
        [
            Number(value.X, decimalPlaces),
            Number(value.Y, decimalPlaces),
            Number(value.Z, decimalPlaces)
        ];
    }

    private static string ToSurrogateResultToken(RpcSurrogateSpawnResult result)
    {
        return result switch
        {
            RpcSurrogateSpawnResult.Added => "added",
            RpcSurrogateSpawnResult.Reused => "reused",
            RpcSurrogateSpawnResult.Missing => "missing",
            RpcSurrogateSpawnResult.Error => "error",
            _ => "error"
        };
    }

    private static string ToValidationRoleToken(ValidationLogRole role)
    {
        return role switch
        {
            ValidationLogRole.Host => "host",
            ValidationLogRole.Client => "client",
            _ => "client"
        };
    }

    private static string ToValidationActionToken(ValidationLogInputAction action)
    {
        return action switch
        {
            ValidationLogInputAction.Save => "save",
            ValidationLogInputAction.Load => "load",
            ValidationLogInputAction.ToggleMagnet => "toggle_magnet",
            _ => "toggle_magnet"
        };
    }

    private static string ToRpcSurrogateResolveSourceToken(
        ValidationLogRpcSurrogateResolveSource source
    )
    {
        return source switch
        {
            ValidationLogRpcSurrogateResolveSource.Cache => "cache",
            ValidationLogRpcSurrogateResolveSource.Lookup => "lookup",
            _ => "lookup"
        };
    }

    private static string ToRpcSurrogateResolveResultToken(
        ValidationLogRpcSurrogateResolveResult result
    )
    {
        return result switch
        {
            ValidationLogRpcSurrogateResolveResult.Success => "success",
            ValidationLogRpcSurrogateResolveResult.Error => "error",
            _ => "error"
        };
    }

    private static string ToBaseGameApplySourceToken(ValidationLogBaseGameApplySource source)
    {
        return source switch
        {
            ValidationLogBaseGameApplySource.LocalApply => "local_apply",
            ValidationLogBaseGameApplySource.ClientRpcApply => "client_rpc_apply",
            ValidationLogBaseGameApplySource.Unknown => "unknown",
            _ => "unknown"
        };
    }

    private static string ToValidationResultToken(SaveCruiserStateResult result)
    {
        return result switch
        {
            SaveCruiserStateResult.Success => "success",
            SaveCruiserStateResult.NoCruiserFound => "no_cruiser_found",
            SaveCruiserStateResult.UnexpectedState => "unexpected_state",
            _ => "unexpected_state"
        };
    }

    private static string ToValidationResultToken(LoadCruiserStateResult result)
    {
        return result switch
        {
            LoadCruiserStateResult.Success => "success",
            LoadCruiserStateResult.NoCruiserFound => "no_cruiser_found",
            LoadCruiserStateResult.NoSavedState => "no_saved_state",
            LoadCruiserStateResult.MagnetedToShip => "magneted_to_ship",
            LoadCruiserStateResult.UnexpectedState => "unexpected_state",
            _ => "unexpected_state"
        };
    }

    private static string ToValidationResultToken(RequestSaveCruiserStateResult result)
    {
        return result switch
        {
            RequestSaveCruiserStateResult.Success => "success",
            RequestSaveCruiserStateResult.HostOnly => "host_only",
            _ => "host_only"
        };
    }

    private static string ToValidationResultToken(RequestLoadCruiserStateResult result)
    {
        return result switch
        {
            RequestLoadCruiserStateResult.Success => "success",
            RequestLoadCruiserStateResult.HostOnly => "host_only",
            _ => "host_only"
        };
    }

    private static string ToValidationResultToken(ToggleMagnetResult result)
    {
        return result switch
        {
            ToggleMagnetResult.MagnetOn => "magnet_on",
            ToggleMagnetResult.MagnetOff => "magnet_off",
            ToggleMagnetResult.HostOnly => "host_only",
            _ => "host_only"
        };
    }

    private static string ToValidationStateToken(MagnetState state)
    {
        return state switch
        {
            MagnetState.On => "on",
            MagnetState.Off => "off",
            MagnetState.Unknown => "unknown",
            _ => "unknown"
        };
    }
}
