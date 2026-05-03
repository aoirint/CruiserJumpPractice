#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

// Magnet toggling reuses the game's own synchronized lever behavior. The custom RPC surrogate is
// reserved for cruiser snapshot save/load, so this use case only guards host authority and feedback.
internal sealed class ToggleMagnetUseCase
{
    private readonly IGameInterop gameInterop;

    public ToggleMagnetUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public ToggleMagnetResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip("CruiserJumpPractice", "Only the host can toggle the magnet.");
            return ToggleMagnetResult.HostOnly;
        }

        var newMagnetState = !gameInterop.IsShipMagnetOn();

        // The game's built-in server RPC flow synchronizes this value.
        gameInterop.ToggleShipMagnet();

        var result = newMagnetState ? ToggleMagnetResult.MagnetOn : ToggleMagnetResult.MagnetOff;
        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        gameInterop.DisplayTip("CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
        return result;
    }
}
