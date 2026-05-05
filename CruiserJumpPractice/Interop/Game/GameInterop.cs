// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;
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

        networkInterop = new NetworkAdapter(logger: logger, gameObjects: gameObjects);
        playerInterop = new PlayerAdapter(logger: logger, gameObjects: gameObjects);
        hudInterop = new HudAdapter(logger: logger, gameObjects: gameObjects);
        rpcSurrogateInterop = new RpcSurrogateAdapter(
            logger: logger,
            gameObjects: gameObjects,
            validationLogger: validationLogger
        );
        cruiserInterop = new CruiserAdapter(logger: logger, gameObjects: gameObjects);
        shipMagnetInterop = new ShipMagnetAdapter(logger: logger, gameObjects: gameObjects);
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
        // Message selection stays in Core; Interop records the closed token
        // before unwrapping user-visible text at the final HUD boundary.
        validationLogger.Record(ValidationLogRecord.HudTip(role: GetRole(), message: message));
        hudInterop.DisplayTip(headerText: message.HeaderText, bodyText: message.BodyText);
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

        return cruiserInterop.RestoreCruiser(cruiser: cruiser, snapshot: snapshot);
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

    private ValidationLogRole GetRole()
    {
        return IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }
}
