// SPDX-License-Identifier: Unlicense
#nullable enable

namespace CruiserJumpPractice.Core.UseCases.Client;

internal enum MagnetState
{
    Unknown,
    On,
    Off
}

internal sealed class MagnetToggleObservation
{
    private MagnetToggleObservation(
        MagnetState beforeState,
        MagnetState expectedAfterState,
        MagnetState observedAfterState
    )
    {
        BeforeState = beforeState;
        ExpectedAfterState = expectedAfterState;
        ObservedAfterState = observedAfterState;
    }

    public MagnetState BeforeState { get; }

    public MagnetState ExpectedAfterState { get; }

    public MagnetState ObservedAfterState { get; }

    public static MagnetToggleObservation FromBeforeState(bool beforeIsOn)
    {
        var beforeState = beforeIsOn ? MagnetState.On : MagnetState.Off;
        var expectedAfterState = beforeIsOn ? MagnetState.Off : MagnetState.On;

        // The vanilla lever/RPC boundary is asynchronous from here; this use
        // case intentionally does not claim to observe the synced result.
        return new MagnetToggleObservation(
            beforeState,
            expectedAfterState,
            MagnetState.Unknown
        );
    }
}
