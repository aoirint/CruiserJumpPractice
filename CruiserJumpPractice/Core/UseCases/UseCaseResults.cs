#nullable enable

namespace CruiserJumpPractice.Core.UseCases;

/// <summary>
/// Small result tokens that can travel through RPC callbacks and presentation code.
/// </summary>
/// <remarks>
/// Exceptions and adapter details stay on the side that observed them.
/// </remarks>
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
