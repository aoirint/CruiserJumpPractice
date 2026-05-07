#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.UseCases.Server;

/// <summary>
/// Captures cruiser state on the server and stores it for later restore.
/// </summary>
/// <remarks>
/// This is the only writer to CruiserStateStore. It returns a small result
/// value because the NetworkBehaviour only needs to report the outcome.
/// </remarks>
internal sealed class SaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;
    private readonly IPluginLogger logger;
    private readonly IValidationLogger validationLogger;

    public SaveCruiserStateUseCase(
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

    public SaveCruiserStateResult Execute()
    {
        try
        {
            var cruiserState = gameInterop.CaptureCruiser();
            if (cruiserState == null)
            {
                logger.LogInfo("No cruiser found.");
                validationLogger.Record(ValidationLogRecord.SaveNoCruiserFound());
                return SaveCruiserStateResult.NoCruiserFound;
            }

            cruiserStateStore.SavedCruiserState = cruiserState;
            RecordSaveSuccess(cruiserState);
            return SaveCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while saving cruiser state: {error}");
            validationLogger.Record(ValidationLogRecord.SaveUnexpectedState());
            return SaveCruiserStateResult.UnexpectedState;
        }
    }

    private void RecordSaveSuccess(CruiserSnapshot cruiserState)
    {
        validationLogger.Record(ValidationLogRecord.SaveSuccess(cruiserState));
    }
}
