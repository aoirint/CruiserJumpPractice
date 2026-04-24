#nullable enable

namespace CruiserJumpPractice.Domain;

internal enum HostGuardResult
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
