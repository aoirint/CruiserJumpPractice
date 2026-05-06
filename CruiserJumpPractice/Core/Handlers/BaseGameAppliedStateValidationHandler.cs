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

    public void HandleEngineOilPreApply()
    {
        stateStore.SetPreEngineOilApplyCarHP(gameInterop.GetCruiserCarHP());
    }

    public void HandleEngineOilLocalApplied()
    {
        // The same vanilla local helper runs during host-initiated restore; only log it when the
        // ClientRpc wrapper marked this call as receiver-side synchronization.
        if (!stateStore.IsEngineOilClientRpcApplyActive)
        {
            return;
        }

        validationLogger.Record(
            record: ValidationLogRecord.BaseGameEngineOilApplied(
                role: GetRole(),
                beforeCarHP: stateStore.PreEngineOilApplyCarHP,
                afterCarHP: gameInterop.GetCruiserCarHP(),
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

    public void HandleTurboPreApply()
    {
        stateStore.SetPreTurboApplyBoosts(gameInterop.GetCruiserTurboBoosts());
    }

    public void HandleTurboLocalApplied()
    {
        // Turbo restore also calls the local helper directly, so this keeps #100 logs scoped to
        // vanilla ClientRpc application instead of duplicating restore observations.
        if (!stateStore.IsTurboClientRpcApplyActive)
        {
            return;
        }

        validationLogger.Record(
            record: ValidationLogRecord.BaseGameTurboApplied(
                role: GetRole(),
                beforeTurbo: stateStore.PreTurboApplyBoosts,
                afterTurbo: gameInterop.GetCruiserTurboBoosts(),
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            )
        );
    }

    public void HandleShipMagnetLocalPreApply()
    {
        stateStore.SetPreMagnetLocalApplyState(gameInterop.IsShipMagnetOn());
    }

    public void HandleShipMagnetLocalApplied()
    {
        HandleShipMagnetApplied(
            before: stateStore.PreMagnetLocalApplyState,
            after: gameInterop.IsShipMagnetOn(),
            source: ValidationLogBaseGameApplySource.LocalApply
        );
    }

    public void HandleShipMagnetClientRpcPreApply()
    {
        stateStore.SetPreMagnetClientRpcApplyState(gameInterop.IsShipMagnetOn());
    }

    public void HandleShipMagnetClientRpcApplied()
    {
        HandleShipMagnetApplied(
            before: stateStore.PreMagnetClientRpcApplyState,
            after: gameInterop.IsShipMagnetOn(),
            source: ValidationLogBaseGameApplySource.ClientRpcApply
        );
    }

    private void HandleShipMagnetApplied(
        bool? before,
        bool after,
        ValidationLogBaseGameApplySource source
    )
    {
        validationLogger.Record(
            record: ValidationLogRecord.BaseGameShipMagnetApplied(
                role: GetRole(),
                before: before,
                after: after,
                source: source
            )
        );
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }
}
