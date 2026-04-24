#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services.Client;

internal class ClientCruiserStateService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    public ClientCruiserStateService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    internal void RequestSaveCruiserState(HUDManager hudManager)
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(hudManager, "CruiserJumpPractice", "Only the host can save the cruiser state.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();
        rpcSurrogateNetworkBehaviour.SaveCruiserStateServerRpc();
    }

    internal void RequestLoadCruiserState(HUDManager hudManager)
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayTip(hudManager, "CruiserJumpPractice", "Only the host can load the cruiser state.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();
        rpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
    }
}
