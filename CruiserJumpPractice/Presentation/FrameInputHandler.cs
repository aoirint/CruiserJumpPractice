#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Presentation;

internal sealed class FrameInputHandler
{
    private readonly IGameInterop gameInterop;
    private readonly ClientCruiserStateFacade clientCruiserStateFacade;
    private readonly ClientMagnetFacade clientMagnetFacade;

    public FrameInputHandler(
        IGameInterop gameInterop,
        ClientCruiserStateFacade clientCruiserStateFacade,
        ClientMagnetFacade clientMagnetFacade
    )
    {
        this.gameInterop = gameInterop;
        this.clientCruiserStateFacade = clientCruiserStateFacade;
        this.clientMagnetFacade = clientMagnetFacade;
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

        clientCruiserStateFacade.RequestSaveCruiserState();
    }

    private void UpdateLoadCruiser()
    {
        if (!(CruiserJumpPractice.InputActions?.LoadCruiserKey?.triggered ?? false))
        {
            return;
        }

        clientCruiserStateFacade.RequestLoadCruiserState();
    }

    private void UpdateToggleMagnet()
    {
        if (!(CruiserJumpPractice.InputActions?.ToggleMagnetKey?.triggered ?? false))
        {
            return;
        }

        clientMagnetFacade.ToggleMagnet();
    }
}