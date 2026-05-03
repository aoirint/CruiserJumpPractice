#nullable enable

namespace CruiserJumpPractice.Core.UseCases;

// Result enums are the narrow contract between Core decisions, Netcode callbacks, and HUD
// presentation. They avoid passing exceptions or adapter details across the client/server boundary.
internal enum RequestSaveCruiserStateResult
{
    Success,
    HostOnly
}

internal enum RequestLoadCruiserStateResult
{
    Success,
    HostOnly
}

internal enum SaveCruiserStateResult
{
    Success,
    NoCruiserFound,
    UnexpectedState
}

internal enum LoadCruiserStateResult
{
    Success,
    NoCruiserFound,
    NoSavedState,
    MagnetedToShip,
    UnexpectedState
}

internal enum ToggleMagnetResult
{
    HostOnly,
    MagnetOn,
    MagnetOff
}
