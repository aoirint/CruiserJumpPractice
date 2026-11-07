using System;
using System.Collections.Generic;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Presentation;
using CruiserJumpPractice.Core.Snapshots;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Tests.Fakes;

internal sealed class FakeGameInterop : IGameInterop
{
    public bool Host { get; set; } = true;
    public bool CruiserPresent { get; set; } = true;
    public bool CruiserMagneted { get; set; }
    public bool MagnetOn { get; set; }
    public LocalPlayerBusyState BusyState { get; set; }
    public CruiserSnapshot? CapturedSnapshot { get; set; }
    public CruiserRestoreObservation? RestoreObservation { get; set; }
    public CruiserSnapshot? RestoredSnapshot { get; private set; }
    public string? ThrowOn { get; set; }
    public Action? OnSaveRequest { get; set; }
    public Action? OnLoadRequest { get; set; }
    public List<string> Calls { get; } = new();
    public List<HudTipMessage> Tips { get; } = new();

    private void Call(string operation)
    {
        Calls.Add(operation);
        if (ThrowOn == operation)
        {
            throw new InvalidOperationException($"Injected failure: {operation}");
        }
    }

    public bool IsHost()
    {
        Call(nameof(IsHost));
        return Host;
    }

    public LocalPlayerBusyState GetLocalPlayerBusyState()
    {
        Call(nameof(GetLocalPlayerBusyState));
        return BusyState;
    }

    public void DisplayTip(HudTipMessage message)
    {
        Call(nameof(DisplayTip));
        Tips.Add(message);
    }

    public void RequestSaveCruiserState()
    {
        Call(nameof(RequestSaveCruiserState));
        OnSaveRequest?.Invoke();
    }

    public void RequestLoadCruiserState()
    {
        Call(nameof(RequestLoadCruiserState));
        OnLoadRequest?.Invoke();
    }

    public bool CruiserExists()
    {
        Call(nameof(CruiserExists));
        return CruiserPresent;
    }

    public CruiserSnapshot? CaptureCruiser()
    {
        Call(nameof(CaptureCruiser));
        return CapturedSnapshot;
    }

    public bool IsCruiserMagnetedToShip()
    {
        Call(nameof(IsCruiserMagnetedToShip));
        return CruiserMagneted;
    }

    public bool IsShipMagnetOn()
    {
        Call(nameof(IsShipMagnetOn));
        return MagnetOn;
    }

    // Leave the observed state unchanged to model deferred lever/RPC synchronization.
    public void ToggleShipMagnet()
    {
        Call(nameof(ToggleShipMagnet));
    }

    public CruiserRestoreObservation RestoreCruiser(CruiserSnapshot snapshot)
    {
        Call(nameof(RestoreCruiser));
        RestoredSnapshot = snapshot;
        return RestoreObservation ?? throw new InvalidOperationException("Set a restore observation for this test.");
    }

    public RpcSurrogateSpawnResult SpawnRpcSurrogate() => throw new NotSupportedException();
    public int? GetCruiserCarHP() => throw new NotSupportedException();
    public int? GetCruiserTurboBoosts() => throw new NotSupportedException();
}
