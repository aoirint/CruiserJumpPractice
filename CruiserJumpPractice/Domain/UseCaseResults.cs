#nullable enable

namespace CruiserJumpPractice.Domain;

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
