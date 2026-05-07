#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.Validation;
using CruiserJumpPractice.Interop.Game.Adapters;

namespace CruiserJumpPractice.Interop.Game;

/// <summary>
/// Game-facing implementation of the practice operations requested by Core.
/// </summary>
/// <remarks>
/// Presents one practice-oriented surface while focused adapters handle HUD,
/// networking, cruiser reflection, and ship magnet objects.
/// </remarks>
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

    /// <summary>
    /// Checks whether the current scene has a cruiser that Core can address.
    /// </summary>
    /// <remarks>
    /// Probe-style cruiser reads report absence as null/false-style results so
    /// Core use cases can choose user-facing outcomes. Operations that require
    /// an already validated cruiser throw instead.
    /// </remarks>
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

    public int? GetCruiserCarHP()
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            return null;
        }

        return CruiserAdapter.GetCarHP(cruiser: cruiser);
    }

    public int? GetCruiserTurboBoosts()
    {
        var cruiser = cruiserInterop.FindCruiser();
        if (cruiser == null)
        {
            return null;
        }

        return CruiserAdapter.GetTurboBoosts(cruiser: cruiser);
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
