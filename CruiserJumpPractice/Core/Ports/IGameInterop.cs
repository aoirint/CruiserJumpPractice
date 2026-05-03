#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.Ports;

// Core depends on game capabilities instead of Lethal Company or Unity types directly.
// The method names are phrased as practice-mode needs, so use cases can stay stable when
// reflection fields, NetworkBehaviour plumbing, or concrete game objects move in Interop.
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

    void RestoreCruiser(CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip();

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
