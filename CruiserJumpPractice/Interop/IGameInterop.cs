#nullable enable

using CruiserJumpPractice.Interop.Behaviours;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop;

internal interface IGameInterop
{
    bool IsClient();

    bool IsHost();

    bool IsLocalPlayerBusy();

    void DisplayTip(string headerText, string bodyText);

    void SpawnRpcSurrogate();

    RpcSurrogateBehaviour GetRpcSurrogateBehaviour();

    bool CruiserExists();

    CruiserSnapshot? CaptureCruiser();

    void RestoreCruiser(CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip();

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
