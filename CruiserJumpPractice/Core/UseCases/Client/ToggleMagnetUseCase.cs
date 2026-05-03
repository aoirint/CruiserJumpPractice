#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Core.UseCases.Client;

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

        // This value is synchronized by the game's built-in server RPC flow.
        gameInterop.ToggleShipMagnet();

        var result = newMagnetState ? ToggleMagnetResult.MagnetOn : ToggleMagnetResult.MagnetOff;
        var magnetStateText = result == ToggleMagnetResult.MagnetOn ? "ON" : "OFF";
        gameInterop.DisplayTip("CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
        return result;
    }
}
