#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Services.Client;

internal class CruiserStateService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    public CruiserStateService(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    internal void RequestSaveCruiserState()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayLocalTip("CruiserJumpPractice", "Only the host can save the cruiser state.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();
        rpcSurrogateNetworkBehaviour.SaveCruiserStateServerRpc();
    }

    internal void RequestLoadCruiserState()
    {
        if (!gameInterop.IsHost())
        {
            gameInterop.DisplayLocalTip("CruiserJumpPractice", "Only the host can load the cruiser state.");
            return;
        }

        var rpcSurrogateNetworkBehaviour = gameInterop.GetRpcSurrogateNetworkBehaviour();
        rpcSurrogateNetworkBehaviour.LoadCruiserStateServerRpc();
    }
}
