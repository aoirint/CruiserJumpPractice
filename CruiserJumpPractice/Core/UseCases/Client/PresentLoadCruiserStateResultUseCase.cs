#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Load results are presented on the requesting client after the RPC returns. Server execution
// stays free of HUD concerns while player-facing messages remain centralized in Core.
internal sealed class PresentLoadCruiserStateResultUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly ICoreLogger logger;

    public PresentLoadCruiserStateResultUseCase(IGameInterop gameInterop, ICoreLogger logger)
    {
        this.gameInterop = gameInterop;
        this.logger = logger;
    }

    public void Execute(LoadCruiserStateResult result)
    {
        if (result == LoadCruiserStateResult.Success)
        {
            DisplayTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            DisplayTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            DisplayTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}
