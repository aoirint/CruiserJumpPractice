#nullable enable

using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.Ports;

/// <summary>
/// Names the game operations the practice rules need without exposing Unity objects.
/// </summary>
/// <remarks>
/// Reflection, Netcode, HUD, and VehicleController details can change in
/// Interop without forcing use cases to speak in Lethal Company types.
/// </remarks>
internal interface IGameInterop
{
    bool IsHost();

    LocalPlayerBusyState GetLocalPlayerBusyState();

    void DisplayTip(HudTipMessage message);

    RpcSurrogateSpawnResult SpawnRpcSurrogate();

    void RequestSaveCruiserState();

    void RequestLoadCruiserState();

    bool CruiserExists();

    CruiserSnapshot? CaptureCruiser();

    int? GetCruiserCarHP();

    int? GetCruiserTurboBoosts();

    CruiserRestoreObservation RestoreCruiser(CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip();

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}

internal enum RpcSurrogateSpawnResult
{
    Added,
    Reused,
    Missing,
    Error
}
