// SPDX-License-Identifier: Unlicense
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

            RecordLoadResult(
                result: "success",
                cruiserFound: true,
                savedState: true,
                magneted: false
            );
            var restoreObservation = gameInterop.RestoreCruiser(savedCruiserState);
            RecordRestoreApplied(restoreObservation);
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while loading cruiser state: {error}");
            validationLogger.Record(
                "load_result",
                ValidationLogField.String("role", "host"),
                ValidationLogField.String("result", "unexpected_state")
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
            ValidationLogField.String("role", "host"),
            ValidationLogField.String("result", result),
            ValidationLogField.Bool("cruiser_found", cruiserFound),
            ValidationLogField.Bool("saved_state", savedState),
            ValidationLogField.Bool("magneted", magneted)
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
            ValidationLogField.String("role", "host"),
            ValidationLogField.String("result", result),
            ValidationLogField.Bool("cruiser_found", cruiserFound),
            ValidationLogField.Bool("saved_state", savedState),
            ValidationLogField.String("magneted", magneted)
        );
    }

    private void RecordRestoreApplied(CruiserRestoreObservation observation)
    {
        validationLogger.Record(
            "restore_applied",
            ValidationLogField.String("role", "host"),
            ValidationLogField.Vector3("saved_pos", observation.SavedCarPosition, decimalPlaces: 1),
            ValidationLogField.Vector3("saved_rot", observation.SavedCarRotation, decimalPlaces: 1),
            ValidationLogField.Vector3("before_pos", observation.BeforeCarPosition, decimalPlaces: 1),
            ValidationLogField.Vector3("after_pos", observation.AfterCarPosition, decimalPlaces: 1),
            ValidationLogField.Int("saved_hp", observation.SavedCarHP),
            ValidationLogField.Int("before_hp", observation.BeforeCarHP),
            ValidationLogField.Int("after_hp", observation.AfterCarHP),
            ValidationLogField.Int("saved_turbo", observation.SavedTurboBoosts),
            ValidationLogField.Int("before_turbo", observation.BeforeTurboBoosts),
            ValidationLogField.Int("after_turbo", observation.AfterTurboBoosts)
        );
    }
}
