#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services.Client;

internal class ClientMagnetService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    public ClientMagnetService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    internal void ToggleMagnet(HUDManager hudManager)
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(hudManager, "CruiserJumpPractice", "Only the host can toggle the magnet.");
            return;
        }

        var newMagnetState = !gameInterop.IsShipMagnetOn();

        // NOTE: This value will be synced with vanilla Server RPC
        gameInterop.ToggleShipMagnet();

        var magnetStateText = newMagnetState ? "ON" : "OFF";
        gameInterop.DisplayTip(hudManager, "CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
    }
}
