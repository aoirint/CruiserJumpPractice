// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.Ports;

// IGameInterop names the game operations the practice rules need, not the Unity objects used to
// perform them. Reflection, Netcode, HUD, and VehicleController details can change in Interop
// without forcing use cases to speak in Lethal Company types.
internal interface IGameInterop
{
    bool IsHost();

    bool IsLocalPlayerBusy();

    void DisplayTip(string headerText, string bodyText);

    void SpawnRpcSurrogate();

    void RequestSaveCruiserState();

    void RequestLoadCruiserState();

    bool CruiserExists();

    CruiserSnapshot? CaptureCruiser();

    CruiserRestoreObservation RestoreCruiser(CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip();

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
