#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Interop.Adapters.V73;
using CruiserJumpPractice.Interop.Behaviours;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop;

internal sealed class GameInteropV73 : IGameInterop
{
    private readonly NetworkAdapterV73 networkInterop;
    private readonly PlayerAdapterV73 playerInterop;
    private readonly HudAdapterV73 hudInterop;
    private readonly RpcSurrogateAdapterV73 rpcSurrogateInterop;
    private readonly CruiserAdapterV73 cruiserInterop;
    private readonly ShipMagnetAdapterV73 shipMagnetInterop;

    public GameInteropV73(ManualLogSource logger)
    {
        var gameObjects = new GameObjectAdapterV73(logger);

        networkInterop = new NetworkAdapterV73(logger, gameObjects);
        playerInterop = new PlayerAdapterV73(logger, gameObjects);
        hudInterop = new HudAdapterV73(logger, gameObjects);
        rpcSurrogateInterop = new RpcSurrogateAdapterV73(logger, gameObjects);
        cruiserInterop = new CruiserAdapterV73(logger, gameObjects);
        shipMagnetInterop = new ShipMagnetAdapterV73(logger, gameObjects);
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

    public void DisplayTip(string headerText, string bodyText)
    {
        hudInterop.DisplayTip(headerText, bodyText);
    }

    public void SpawnRpcSurrogate()
    {
        rpcSurrogateInterop.SpawnRpcSurrogate();
    }

    public RpcSurrogateBehaviour GetRpcSurrogateBehaviour()
    {
        return rpcSurrogateInterop.GetRpcSurrogateBehaviour();
    }

    public bool CruiserExists()
    {
        return cruiserInterop.FindCruiser() != null;
    }

    public CruiserSnapshot? CaptureCruiser()
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            return null;
        }

        return cruiserInterop.CaptureCruiser(cruiser);
    }

    public void RestoreCruiser(CruiserSnapshot snapshot)
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            throw new GameInteropException("No cruiser found.");
        }

        cruiserInterop.RestoreCruiser(cruiser, snapshot);
    }

    public bool IsCruiserMagnetedToShip()
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            throw new GameInteropException("No cruiser found.");
        }

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
