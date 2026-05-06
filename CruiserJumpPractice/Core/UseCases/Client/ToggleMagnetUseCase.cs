// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Validation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Magnet toggling reuses the game's own synchronized lever behavior. The custom
// RPC surrogate is reserved for cruiser snapshot save/load, so this use case
// only guards host authority and feedback.
internal sealed class ToggleMagnetUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;

    public ToggleMagnetUseCase(IGameInterop gameInterop, IValidationLogger validationLogger)
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
    }

    public ToggleMagnetResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(HudTipMessage.MagnetHostOnly);
            RecordResult(role: ValidationLogRole.Client, result: ToggleMagnetResult.HostOnly);
            return ToggleMagnetResult.HostOnly;
        }

        var observation = MagnetToggleObservation.FromBeforeState(gameInterop.IsShipMagnetOn());

        // The game's built-in server RPC flow synchronizes this value.
        gameInterop.ToggleShipMagnet();
        validationLogger.Record(ValidationLogRecord.MagnetToggle(observation));

        var result = observation.ExpectedAfterState == MagnetState.On
            ? ToggleMagnetResult.MagnetOn
            : ToggleMagnetResult.MagnetOff;
        validationLogger.Record(
            ValidationLogRecord.ToggleMagnetResultEvent(
                role: ValidationLogRole.Host,
                result: result
            )
        );

        // Feedback uses the expected state because the vanilla lever/RPC path
        // may finish synchronization after this use case returns.
        var message = result == ToggleMagnetResult.MagnetOn
            ? HudTipMessage.MagnetOn
            : HudTipMessage.MagnetOff;
        gameInterop.DisplayTip(message);
        return result;
    }

    private void RecordResult(ValidationLogRole role, ToggleMagnetResult result)
    {
        validationLogger.Record(
            ValidationLogRecord.ToggleMagnetResultEvent(role: role, result: result)
        );
    }
}
