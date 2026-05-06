// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Save result wording belongs with practice behavior, but displaying the tip is still a game
// operation. This keeps the message mapping in Core and the HUD call behind IGameInterop.
internal sealed class PresentSaveCruiserStateResultUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly IPluginLogger logger;

    public PresentSaveCruiserStateResultUseCase(IGameInterop gameInterop, IPluginLogger logger)
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
    }

    public void Execute(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            DisplayTip(HudTipMessage.SaveSuccess);
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            DisplayTip(HudTipMessage.SaveNoCruiser);
        }
        else
        {
            // Unexpected results are diagnostics, not player-facing practice
            // states. Known failures get HUD tips; unknown values stay in logs.
            logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(HudTipMessage message)
    {
        gameInterop.DisplayTip(message);
    }
}
