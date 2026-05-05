// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.Handlers;

internal sealed class BaseGameAppliedStateValidationHandler
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;
    private readonly BaseGameAppliedStateValidationStore stateStore;

    public BaseGameAppliedStateValidationHandler(
        IGameInterop gameInterop,
        IValidationLogger validationLogger,
        BaseGameAppliedStateValidationStore stateStore
    )
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
        this.stateStore = stateStore;
    }

    public void EnterEngineOilClientRpc()
    {
        stateStore.EnterEngineOilClientRpc();
    }

    public void ExitEngineOilClientRpc()
    {
        stateStore.ExitEngineOilClientRpc();
    }

    public void HandleEngineOilLocalApplied(int expectedHP, int observedHP)
    {
        if (!stateStore.IsEngineOilClientRpcApplyActive)
        {
            return;
        }

        validationLogger.Record(
            record: ValidationLogRecord.BaseGameEngineOilApplied(
                role: GetRole(),
                expectedHP: expectedHP,
                observedHP: observedHP,
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            )
        );
    }

    public void EnterTurboClientRpc()
    {
        stateStore.EnterTurboClientRpc();
    }

    public void ExitTurboClientRpc()
    {
        stateStore.ExitTurboClientRpc();
    }

    public void HandleTurboLocalApplied(int expectedTurbo, int observedTurbo)
    {
        if (!stateStore.IsTurboClientRpcApplyActive)
        {
            return;
        }

        validationLogger.Record(
            record: ValidationLogRecord.BaseGameTurboApplied(
                role: GetRole(),
                expectedTurbo: expectedTurbo,
                observedTurbo: observedTurbo,
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            )
        );
    }

    public void HandleShipMagnetApplied(
        bool expectedAfter,
        bool observedAfter,
        ValidationLogBaseGameApplySource source
    )
    {
        validationLogger.Record(
            record: ValidationLogRecord.BaseGameShipMagnetApplied(
                role: GetRole(),
                expectedAfter: expectedAfter,
                observedAfter: observedAfter,
                source: source
            )
        );
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }
}
