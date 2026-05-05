// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.UseCases.Server;

// Loading can move a live vehicle, so the preconditions stay next to the server restore call.
// The client gets only the result enum that describes why the restore did or did not happen.
internal sealed class LoadCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public LoadCruiserStateUseCase(
        IGameInterop gameInterop,
        CruiserStateStore cruiserStateStore,
        IPluginLogger logger,
        IValidationLogger validationLogger
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateStore = cruiserStateStore;
        this.logger = logger;
        this.validationLogger = validationLogger;
    }

    public LoadCruiserStateResult Execute()
    {
        try
        {
            if (!gameInterop.CruiserExists())
            {
                logger.LogInfo("No cruiser found.");
                RecordLoadResult(
                    result: "no_cruiser_found",
                    cruiserFound: false,
                    savedState: cruiserStateStore.SavedCruiserState != null,
                    magneted: "unknown"
                );
                return LoadCruiserStateResult.NoCruiserFound;
            }

            var savedCruiserState = cruiserStateStore.SavedCruiserState;
            if (savedCruiserState == null)
            {
                logger.LogInfo("No saved cruiser state found.");
                RecordLoadResult(
                    result: "no_saved_state",
                    cruiserFound: true,
                    savedState: false,
                    magneted: "unknown"
                );
                return LoadCruiserStateResult.NoSavedState;
            }

            var isMagneted = gameInterop.IsCruiserMagnetedToShip();
            if (isMagneted)
            {
                logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
                RecordLoadResult(
                    result: "magneted_to_ship",
                    cruiserFound: true,
                    savedState: true,
                    magneted: true
                );
                return LoadCruiserStateResult.MagnetedToShip;
            }

            // The restore observation is collected on the server path for validation logging; the
            // client RPC still receives only the enum result so restore details stay server-local.
            var restoreObservation = gameInterop.RestoreCruiser(savedCruiserState);
            RecordRestoreApplied(restoreObservation);
            // Success means the server restore completed, not just that preconditions passed.
            RecordLoadResult(
                result: "success",
                cruiserFound: true,
                savedState: true,
                magneted: false
            );
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while loading cruiser state: {error}");
            validationLogger.Record(
                "load_result",
                new()
                {
                    ["role"] = "host",
                    ["result"] = "unexpected_state"
                }
            );
            return LoadCruiserStateResult.UnexpectedState;
        }
    }

    private void RecordLoadResult(
        string result,
        bool cruiserFound,
        bool savedState,
        bool magneted
    )
    {
        validationLogger.Record(
            "load_result",
            new()
            {
                ["role"] = "host",
                ["result"] = result,
                ["cruiser_found"] = cruiserFound,
                ["saved_state"] = savedState,
                ["magneted"] = magneted
            }
        );
    }

    private void RecordLoadResult(
        string result,
        bool cruiserFound,
        bool savedState,
        string magneted
    )
    {
        validationLogger.Record(
            "load_result",
            new()
            {
                ["role"] = "host",
                ["result"] = result,
                ["cruiser_found"] = cruiserFound,
                ["saved_state"] = savedState,
                ["magneted"] = magneted
            }
        );
    }

    private void RecordRestoreApplied(CruiserRestoreObservation observation)
    {
        validationLogger.Record(
            "restore_applied",
            new()
            {
                ["role"] = "host",
                ["saved_pos"] = ValidationLogData.Vector3(observation.SavedCarPosition, decimalPlaces: 1),
                ["saved_rot"] = ValidationLogData.Vector3(observation.SavedCarRotation, decimalPlaces: 1),
                ["before_pos"] = ValidationLogData.Vector3(observation.BeforeCarPosition, decimalPlaces: 1),
                ["after_pos"] = ValidationLogData.Vector3(observation.AfterCarPosition, decimalPlaces: 1),
                ["saved_hp"] = observation.SavedCarHP,
                ["before_hp"] = observation.BeforeCarHP,
                ["after_hp"] = observation.AfterCarHP,
                ["saved_turbo"] = observation.SavedTurboBoosts,
                ["before_turbo"] = observation.BeforeTurboBoosts,
                ["after_turbo"] = observation.AfterTurboBoosts
            }
        );
    }
}
