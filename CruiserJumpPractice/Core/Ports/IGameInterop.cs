#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.Ports;

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
