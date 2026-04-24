#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Controllers;
using CruiserJumpPractice.BaseGame.Controllers.Client;
using CruiserJumpPractice.BaseGame.Finders;

namespace CruiserJumpPractice.Services.Client;

internal class CruiserStateClientService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal void RequestSaveCruiserState(HUDManager hudManager)
    {
        if (!IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can save the cruiser state.");
            return;
        }

        var customRpcSurrogateNetworkBehaviourFinder = new CustomRpcSurrogateNetworkBehaviourFinder();
        var customRpcSurrogateNetworkBehaviour =
            customRpcSurrogateNetworkBehaviourFinder.GetCustomRpcSurrogateNetworkBehaviour();

        customRpcSurrogateNetworkBehaviour.SaveCruiserStateServerRpc();
    }

    internal void RequestLoadCruiserState(HUDManager hudManager)
    {
        if (!IsHost())
        {
            var tipController = new TipController(hudManager);
            tipController.DisplayTip("CruiserJumpPractice", "Only the host can load the cruiser state.");
            return;
        }

        var customRpcSurrogateNetworkBehaviourFinder = new CustomRpcSurrogateNetworkBehaviourFinder();
        var customRpcSurrogateNetworkBehaviour =
            customRpcSurrogateNetworkBehaviourFinder.GetCustomRpcSurrogateNetworkBehaviour();

        customRpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
    }

    private static bool IsHost()
    {
        var networkManagerFinder = new NetworkManagerFinder();
        var networkManager = networkManagerFinder.GetNetworkManager();
        var networkStateController = new NetworkStateController(networkManager);
        return networkStateController.IsHost();
    }
}
