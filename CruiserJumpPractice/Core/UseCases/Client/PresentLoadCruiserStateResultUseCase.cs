// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

/// <summary>
/// Maps load results to player-facing HUD feedback.
/// </summary>
/// <remarks>
/// The server restores state and reports a result; this use case owns how that
/// result is explained after the RPC returns to the requesting client.
/// </remarks>
internal sealed class PresentLoadCruiserStateResultUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;

    public PresentLoadCruiserStateResultUseCase(IGameInterop gameInterop, IPluginLogger logger)
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
    }

    public void Execute(LoadCruiserStateResult result)
    {
        if (result == LoadCruiserStateResult.Success)
        {
            DisplayTip(HudTipMessage.LoadSuccess);
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            DisplayTip(HudTipMessage.LoadNoCruiser);
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            DisplayTip(HudTipMessage.LoadNoSavedState);
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            DisplayTip(HudTipMessage.LoadMagnetedToShip);
        }
        else
        {
            // Unexpected results are diagnostics, not player-facing practice
            // states. Known failures get HUD tips; unknown values stay in logs.
            logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(HudTipMessage message)
    {
        gameInterop.DisplayTip(message);
    }
}
