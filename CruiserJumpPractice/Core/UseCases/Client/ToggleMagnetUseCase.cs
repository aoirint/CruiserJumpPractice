// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

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
            RecordResult("client", "host_only");
            return ToggleMagnetResult.HostOnly;
        }

        var observation = MagnetToggleObservation.FromBeforeState(gameInterop.IsShipMagnetOn());

        // The game's built-in server RPC flow synchronizes this value.
        gameInterop.ToggleShipMagnet();
        validationLogger.Record(
            "magnet_toggle",
            new()
            {
                ["role"] = "host",
                ["before"] = ToValidationStateToken(observation.BeforeState),
                ["expected_after"] = ToValidationStateToken(observation.ExpectedAfterState),
                ["observed_after"] = ToValidationStateToken(observation.ObservedAfterState)
            }
        );

        var result = observation.ExpectedAfterState == MagnetState.On
            ? ToggleMagnetResult.MagnetOn
            : ToggleMagnetResult.MagnetOff;
        RecordResult("host", ToValidationResultToken(result));
        var message = result == ToggleMagnetResult.MagnetOn
            ? HudTipMessage.MagnetOn
            : HudTipMessage.MagnetOff;
        gameInterop.DisplayTip(message);
        return result;
    }

    private void RecordResult(string role, string result)
    {
        validationLogger.Record(
            "toggle_magnet_result",
            new()
            {
                ["role"] = role,
                ["result"] = result
            }
        );
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
