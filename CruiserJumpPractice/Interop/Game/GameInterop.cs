#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Interop.Game.Adapters;

namespace CruiserJumpPractice.Interop.Game;

// GameInterop implements Core's game port by delegating to focused adapters. Core sees one
// practice-oriented surface instead of coordinating HUD, networking, cruiser reflection, and
// ship magnet objects itself.
internal sealed class GameInterop : IGameInterop
{
    private readonly NetworkAdapter networkInterop;
    private readonly PlayerAdapter playerInterop;
    private readonly HudAdapter hudInterop;
    private readonly RpcSurrogateAdapter rpcSurrogateInterop;
    private readonly CruiserAdapter cruiserInterop;
    private readonly ShipMagnetAdapter shipMagnetInterop;

    public GameInterop(ManualLogSource logger)
    {
        var gameObjects = new GameObjectAdapter(logger);

        networkInterop = new NetworkAdapter(logger, gameObjects);
        playerInterop = new PlayerAdapter(logger, gameObjects);
        hudInterop = new HudAdapter(logger, gameObjects);
        rpcSurrogateInterop = new RpcSurrogateAdapter(logger, gameObjects);
        cruiserInterop = new CruiserAdapter(logger, gameObjects);
        shipMagnetInterop = new ShipMagnetAdapter(logger, gameObjects);
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

    public void RequestSaveCruiserState()
    {
        rpcSurrogateInterop.GetRpcSurrogateBehaviour().SaveCruiserStateServerRpc();
    }

    public void RequestLoadCruiserState()
    {
        rpcSurrogateInterop.GetRpcSurrogateBehaviour().LoadCruiserStateServerRpc();
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
