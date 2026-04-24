#nullable enable

using CruiserJumpPractice.Interop.Behaviours;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Interop;

internal interface IGameInterop
{
    bool IsServer();

    bool IsClient();

    bool IsHost();

    bool IsLocalPlayerBusy();

    void DisplayTip(string headerText, string bodyText);

    void SpawnRpcSurrogate();

    RpcSurrogateBehaviour GetRpcSurrogateBehaviour();

    VehicleController? FindCruiser();

    CruiserSnapshot CaptureCruiser(VehicleController cruiser);

    void RestoreCruiser(VehicleController cruiser, CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip(VehicleController cruiser);

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
