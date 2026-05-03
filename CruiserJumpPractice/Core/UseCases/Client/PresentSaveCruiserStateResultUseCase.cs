#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

internal sealed class PresentSaveCruiserStateResultUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly ICoreLogger logger;

    public PresentSaveCruiserStateResultUseCase(IGameInterop gameInterop, ICoreLogger logger)
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
