#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services;

internal class MagnetService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    public MagnetService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    internal void ToggleMagnet()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayLocalTip("CruiserJumpPractice", "Only the host can toggle the magnet.");
            return;
        }

        var newMagnetState = !gameInterop.IsShipMagnetOn();

        // NOTE: This value will be synced with vanilla Server RPC
        gameInterop.ToggleShipMagnet();

        var magnetStateText = newMagnetState ? "ON" : "OFF";
        gameInterop.DisplayLocalTip("CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
    }
}
