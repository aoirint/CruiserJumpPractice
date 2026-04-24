#nullable enable

using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Services.Client;

namespace CruiserJumpPractice.Presentation;

internal sealed class FrameInputCoordinator
{
    private readonly IGameInterop gameInterop;
    private readonly ClientCruiserStateCoordinator clientCruiserStateCoordinator;
    private readonly ClientMagnetCoordinator clientMagnetCoordinator;

    public FrameInputCoordinator(
        IGameInterop gameInterop,
        ClientCruiserStateCoordinator clientCruiserStateCoordinator,
        ClientMagnetCoordinator clientMagnetCoordinator
    )
    {
        this.gameInterop = gameInterop;
        this.clientCruiserStateCoordinator = clientCruiserStateCoordinator;
        this.clientMagnetCoordinator = clientMagnetCoordinator;
    }

    public void HandleFrame()
    {
        if (!gameInterop.IsClient())
        {
            return;
        }

        if (gameInterop.IsLocalPlayerBusy())
        {
            return;
        }

        UpdateSaveCruiser();
        UpdateLoadCruiser();
        UpdateToggleMagnet();
    }

    private void UpdateSaveCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.SaveCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateCoordinator.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateCoordinator.RequestLoadCruiserState();
    }

    private void UpdateToggleMagnet()
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        clientMagnetCoordinator.ToggleMagnet();
    }
}