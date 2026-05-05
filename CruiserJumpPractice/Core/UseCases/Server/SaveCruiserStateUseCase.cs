// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.UseCases.Server;

// The server save path is the only writer to CruiserStateStore. It returns a small result value
// because the NetworkBehaviour only needs to report the outcome, not the captured snapshot.
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
                validationLogger.Record(
                    "save_result",
                    new()
                    {
                        ["role"] = "host",
                        ["result"] = "no_cruiser_found",
                        ["cruiser_found"] = false
                    }
                );
                return SaveCruiserStateResult.NoCruiserFound;
            }

            cruiserStateStore.SavedCruiserState = cruiserState;
            RecordSaveSuccess(cruiserState);
            return SaveCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while saving cruiser state: {error}");
            validationLogger.Record(
                "save_result",
                new()
                {
                    ["role"] = "host",
                    ["result"] = "unexpected_state"
                }
            );
            return SaveCruiserStateResult.UnexpectedState;
        }
    }

    private void RecordSaveSuccess(CruiserSnapshot cruiserState)
    {
        validationLogger.Record(
            "save_result",
            new()
            {
                ["role"] = "host",
                ["result"] = "success",
                ["cruiser_found"] = true,
                ["pos"] = ValidationLogData.Vector3(cruiserState.CarPosition, decimalPlaces: 1),
                ["rot"] = ValidationLogData.Vector3(cruiserState.CarRotation, decimalPlaces: 1),
                ["hp"] = cruiserState.CarHP,
                ["turbo"] = cruiserState.TurboBoosts,
                ["steering"] = ValidationLogData.Number(cruiserState.SteeringInput, decimalPlaces: 2),
                ["rpm"] = ValidationLogData.Number(cruiserState.EngineRPM, decimalPlaces: 2)
            }
        );
    }
}
