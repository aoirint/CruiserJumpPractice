// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.Presentation;

namespace CruiserJumpPractice.Core.Ports;

// IGameInterop names the game operations the practice rules need, not the Unity objects used to
// perform them. Reflection, Netcode, HUD, and VehicleController details can change in Interop
// without forcing use cases to speak in Lethal Company types.
internal interface IGameInterop
{
    bool IsHost();

    bool IsLocalPlayerBusy();

    void DisplayTip(HudTipMessage message);

    void SpawnRpcSurrogate();

    void RequestSaveCruiserState();

    void RequestLoadCruiserState();

    bool CruiserExists();

    CruiserSnapshot? CaptureCruiser();

    void RestoreCruiser(CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip();

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
