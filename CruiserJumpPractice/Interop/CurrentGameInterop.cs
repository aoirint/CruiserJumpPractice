#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Interop.Adapters.V73;
using CruiserJumpPractice.Interop.Behaviours;
using CruiserJumpPractice.Interop.Domain;

namespace CruiserJumpPractice.Interop;

internal sealed class CurrentGameInterop : IGameInterop
{
    private readonly NetworkInterop networkInterop;
    private readonly PlayerInterop playerInterop;
    private readonly HudInterop hudInterop;
    private readonly RpcSurrogateInterop rpcSurrogateInterop;
    private readonly CruiserInterop cruiserInterop;
    private readonly ShipMagnetInterop shipMagnetInterop;

    public CurrentGameInterop(ManualLogSource logger)
    {
        var gameObjects = new GameObjectInterop(logger);

        networkInterop = new NetworkInterop(logger, gameObjects);
        playerInterop = new PlayerInterop(logger, gameObjects);
        hudInterop = new HudInterop(logger, gameObjects);
        rpcSurrogateInterop = new RpcSurrogateInterop(logger, gameObjects);
        cruiserInterop = new CruiserInterop(logger, gameObjects);
        shipMagnetInterop = new ShipMagnetInterop(logger, gameObjects);
    }

    public bool IsServer()
    {
        return networkInterop.IsServer();
    }

    public bool IsClient()
    {
        return networkInterop.IsClient();
    }

    public bool IsHost()
    {
        return networkInterop.IsHost();
    }

    public bool IsLocalPlayerBusy()
    {
        return playerInterop.IsLocalPlayerBusy();
    }

    public void DisplayTip(HUDManager hudManager, string headerText, string bodyText)
    {
        hudInterop.DisplayTip(hudManager, headerText, bodyText);
    }

    public void DisplayLocalTip(string headerText, string bodyText)
    {
        hudInterop.DisplayLocalTip(headerText, bodyText);
    }

    public void SpawnRpcSurrogate()
    {
        rpcSurrogateInterop.SpawnRpcSurrogate();
    }

    public RpcSurrogateNetworkBehaviour GetRpcSurrogateNetworkBehaviour()
    {
        return rpcSurrogateInterop.GetRpcSurrogateNetworkBehaviour();
    }

    public VehicleController? FindCruiser()
    {
        return cruiserInterop.FindCruiser();
    }

    public CruiserSnapshot CaptureCruiser(VehicleController cruiser)
    {
        return cruiserInterop.CaptureCruiser(cruiser);
    }

    public void RestoreCruiser(VehicleController cruiser, CruiserSnapshot snapshot)
    {
        cruiserInterop.RestoreCruiser(cruiser, snapshot);
    }

    public bool IsCruiserMagnetedToShip(VehicleController cruiser)
    {
        return cruiserInterop.IsCruiserMagnetedToShip(cruiser);
    }

    public bool IsShipMagnetOn()
    {
        return shipMagnetInterop.IsShipMagnetOn();
    }

    public void ToggleShipMagnet()
    {
        shipMagnetInterop.ToggleShipMagnet();
    }
}
