#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Controllers;
using CruiserJumpPractice.BaseGame.Controllers.Client;
using CruiserJumpPractice.BaseGame.Controllers.Server;
using CruiserJumpPractice.BaseGame.Finders;

namespace CruiserJumpPractice.Services.Client;

internal class MagnetClientService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal void ToggleMagnet(HUDManager hudManager)
    {
        if (!IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can toggle the magnet.");
            return;
        }

        var startOfRoundFinder = new StartOfRoundFinder();
        var startOfRound = startOfRoundFinder.GetStartOfRound();
        var magnetController = new MagnetController(startOfRound);
        var newMagnetState = !magnetController.IsMagnetOn();

        // NOTE: This value will be synced with vanilla Server RPC
        magnetController.ToggleMagnet();

        var magnetStateText = newMagnetState ? "ON" : "OFF";
        var localTipController = new TipController(hudManager);
        localTipController.DisplayTip("CruiserJumpPractice", $"Magnet is now {magnetStateText}.");
    }

    private static bool IsHost()
    {
        var networkManagerFinder = new NetworkManagerFinder();
        var networkManager = networkManagerFinder.GetNetworkManager();
        var networkStateController = new NetworkStateController(networkManager);
        return networkStateController.IsHost();
    }
}
