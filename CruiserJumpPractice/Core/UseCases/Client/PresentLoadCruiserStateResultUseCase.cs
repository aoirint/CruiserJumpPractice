#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Load result messages are emitted after the RPC returns to the requesting client. The server
// restores state and reports a result; this use case owns how that result is explained to players.
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
