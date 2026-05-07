#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.Handlers;

/// <summary>
/// Translates Harmony patch observation points into before/after validation events.
/// </summary>
/// <remarks>
/// Patch classes know where the base game applies state; this class decides
/// which observations are meaningful to CJP validation.
/// </remarks>
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

    /// <summary>
    /// Marks entry into the engine-oil ClientRpc receiver path.
    /// </summary>
    public void EnterEngineOilClientRpc()
    {
        stateStore.EnterEngineOilClientRpc();
    }

    /// <summary>
    /// Marks exit from the engine-oil ClientRpc receiver path.
    /// </summary>
    public void ExitEngineOilClientRpc()
    {
        stateStore.ExitEngineOilClientRpc();
    }

    /// <summary>
    /// Captures cruiser HP before the local engine-oil apply helper runs.
    /// </summary>
    public void HandleEngineOilLocalPreApply()
    {
        stateStore.SetPreEngineOilLocalApplyCarHP(gameInterop.GetCruiserCarHP());
    }

    /// <summary>
    /// Records receiver-side cruiser HP after the local engine-oil apply helper runs.
    /// </summary>
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
                beforeCarHP: stateStore.PreEngineOilLocalApplyCarHP,
                afterCarHP: gameInterop.GetCruiserCarHP(),
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            )
        );
    }

    /// <summary>
    /// Marks entry into the turbo ClientRpc receiver path.
    /// </summary>
    public void EnterTurboClientRpc()
    {
        stateStore.EnterTurboClientRpc();
    }

    /// <summary>
    /// Marks exit from the turbo ClientRpc receiver path.
    /// </summary>
    public void ExitTurboClientRpc()
    {
        stateStore.ExitTurboClientRpc();
    }

    /// <summary>
    /// Captures turbo count before the local turbo apply helper runs.
    /// </summary>
    public void HandleTurboLocalPreApply()
    {
        stateStore.SetPreTurboLocalApplyBoosts(gameInterop.GetCruiserTurboBoosts());
    }

    /// <summary>
    /// Records receiver-side turbo count after the local turbo apply helper runs.
    /// </summary>
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
                beforeTurbo: stateStore.PreTurboLocalApplyBoosts,
                afterTurbo: gameInterop.GetCruiserTurboBoosts(),
                source: ValidationLogBaseGameApplySource.ClientRpcApply
            )
        );
    }

    /// <summary>
    /// Captures ship-magnet state before the local magnet apply path runs.
    /// </summary>
    public void HandleShipMagnetLocalPreApply()
    {
        stateStore.SetPreMagnetLocalApplyState(gameInterop.IsShipMagnetOn());
    }

    /// <summary>
    /// Records ship-magnet state after the local magnet apply path runs.
    /// </summary>
    public void HandleShipMagnetLocalApplied()
    {
        HandleShipMagnetApplied(
            before: stateStore.PreMagnetLocalApplyState,
            after: gameInterop.IsShipMagnetOn(),
            source: ValidationLogBaseGameApplySource.LocalApply
        );
    }

    /// <summary>
    /// Captures ship-magnet state before the magnet ClientRpc apply path runs.
    /// </summary>
    public void HandleShipMagnetClientRpcPreApply()
    {
        stateStore.SetPreMagnetClientRpcApplyState(gameInterop.IsShipMagnetOn());
    }

    /// <summary>
    /// Records ship-magnet state after the magnet ClientRpc apply path runs.
    /// </summary>
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
