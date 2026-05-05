// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Interop.Game.Adapters;

namespace CruiserJumpPractice.Interop.Game;

// GameInterop is the game-facing implementation of the practice operations requested by Core.
// It presents one practice-oriented surface while focused adapters handle HUD, networking,
// cruiser reflection, and ship magnet objects.
internal sealed class GameInterop : IGameInterop
{
    private readonly NetworkAdapter networkInterop;
    private readonly IValidationLogger validationLogger;
    private readonly PlayerAdapter playerInterop;
    private readonly HudAdapter hudInterop;
    private readonly RpcSurrogateAdapter rpcSurrogateInterop;
    private readonly CruiserAdapter cruiserInterop;
    private readonly ShipMagnetAdapter shipMagnetInterop;

    public GameInterop(IPluginLogger logger, IValidationLogger validationLogger)
    {
        this.validationLogger = validationLogger;
        var gameObjects = new GameObjectAdapter(logger);

        networkInterop = new NetworkAdapter(logger, gameObjects);
        playerInterop = new PlayerAdapter(logger, gameObjects);
        hudInterop = new HudAdapter(logger, gameObjects);
        rpcSurrogateInterop = new RpcSurrogateAdapter(logger, gameObjects, validationLogger);
        cruiserInterop = new CruiserAdapter(logger, gameObjects);
        shipMagnetInterop = new ShipMagnetAdapter(logger, gameObjects);
    }

    public bool IsHost()
    {
        return networkInterop.IsHost();
    }

    public LocalPlayerBusyState GetLocalPlayerBusyState()
    {
        return playerInterop.GetLocalPlayerBusyState();
    }

    public void DisplayTip(HudTipMessage message)
    {
        validationLogger.Record(
            "hud_tip",
            ValidationLogField.String("role", GetRoleToken()),
            ValidationLogField.String("message", message.Token)
        );
        hudInterop.DisplayTip(message.HeaderText, message.BodyText);
    }

    public RpcSurrogateSpawnResult SpawnRpcSurrogate()
    {
        return rpcSurrogateInterop.SpawnRpcSurrogate();
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

    public CruiserRestoreObservation RestoreCruiser(CruiserSnapshot snapshot)
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            throw new GameInteropException("No cruiser found.");
        }

        return cruiserInterop.RestoreCruiser(cruiser, snapshot);
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

    private string GetRoleToken()
    {
        return IsHost() ? "host" : "client";
    }
}
