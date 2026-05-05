// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;

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
            DisplayTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to save.");
        }
        else
        {
            logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}
