// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;

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
                validationLogger.Record(
                    ValidationLogRecord.LoadNoCruiserFound(
                        cruiserStateStore.SavedCruiserState != null
                    )
                );
                return LoadCruiserStateResult.NoCruiserFound;
            }

            var savedCruiserState = cruiserStateStore.SavedCruiserState;
            if (savedCruiserState == null)
            {
                logger.LogInfo("No saved cruiser state found.");
                validationLogger.Record(ValidationLogRecord.LoadNoSavedState());
                return LoadCruiserStateResult.NoSavedState;
            }

            var isMagneted = gameInterop.IsCruiserMagnetedToShip();
            if (isMagneted)
            {
                logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
                validationLogger.Record(ValidationLogRecord.LoadMagnetedToShip());
                return LoadCruiserStateResult.MagnetedToShip;
            }

            // The restore observation is collected on the server path for validation logging; the
            // client RPC still receives only the enum result so restore details stay server-local.
            var restoreObservation = gameInterop.RestoreCruiser(savedCruiserState);
            RecordRestoreApplied(restoreObservation);
            // Success means the server restore completed, not just that preconditions passed.
            validationLogger.Record(ValidationLogRecord.LoadSuccess());
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while loading cruiser state: {error}");
            validationLogger.Record(ValidationLogRecord.LoadUnexpectedState());
            return LoadCruiserStateResult.UnexpectedState;
        }
    }

    private void RecordRestoreApplied(CruiserRestoreObservation observation)
    {
        validationLogger.Record(ValidationLogRecord.RestoreApplied(observation));
    }
}
