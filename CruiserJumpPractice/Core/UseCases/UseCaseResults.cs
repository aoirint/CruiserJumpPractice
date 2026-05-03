#nullable enable

namespace CruiserJumpPractice.Core.UseCases;

// Result enums are deliberately small enough to travel through RPC callbacks and presentation
// code. Exceptions and adapter details stay on the side that observed them.
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
