// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Load result messages are emitted after the RPC returns to the requesting
// client. The server restores state and reports a result; this use case owns how
// that result is explained to players.
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
            logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(HudTipMessage message)
    {
        gameInterop.DisplayTip(message);
    }
}
