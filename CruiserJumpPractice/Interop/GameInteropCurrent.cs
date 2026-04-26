#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Interop.Adapters.Current;
using CruiserJumpPractice.Interop.Behaviours;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop;

internal sealed class GameInteropCurrent : IGameInterop
{
    private readonly NetworkAdapterCurrent networkInterop;
    private readonly PlayerAdapterCurrent playerInterop;
    private readonly HudAdapterCurrent hudInterop;
    private readonly RpcSurrogateAdapterCurrent rpcSurrogateInterop;
    private readonly CruiserAdapterCurrent cruiserInterop;
    private readonly ShipMagnetAdapterCurrent shipMagnetInterop;

    public GameInteropCurrent(ManualLogSource logger)
    {
        var gameObjects = new GameObjectAdapterCurrent(logger);

        networkInterop = new NetworkAdapterCurrent(logger, gameObjects);
        playerInterop = new PlayerAdapterCurrent(logger, gameObjects);
        hudInterop = new HudAdapterCurrent(logger, gameObjects);
        rpcSurrogateInterop = new RpcSurrogateAdapterCurrent(logger, gameObjects);
        cruiserInterop = new CruiserAdapterCurrent(logger, gameObjects);
        shipMagnetInterop = new ShipMagnetAdapterCurrent(logger, gameObjects);
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
