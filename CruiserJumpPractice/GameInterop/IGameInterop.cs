#nullable enable

using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.GameInterop;

internal interface IGameInterop
{
    bool IsServer();

    bool IsClient();

    bool IsHost();

    bool IsLocalPlayerBusy();

    void DisplayTip(HUDManager hudManager, string headerText, string bodyText);

    void DisplayLocalTip(string headerText, string bodyText);

    void SpawnRpcSurrogate();

    RpcSurrogateNetworkBehaviour GetRpcSurrogateNetworkBehaviour();

    VehicleController? FindCruiser();

    CruiserSnapshot CaptureCruiser(VehicleController cruiser);

    void RestoreCruiser(VehicleController cruiser, CruiserSnapshot snapshot);

    bool IsCruiserMagnetedToShip(VehicleController cruiser);

    bool IsShipMagnetOn();

    void ToggleShipMagnet();
}
