#nullable enable

using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.UseCases;

internal sealed class ToggleMagnetUseCase
{
    private readonly IGameInterop gameInterop;

    public ToggleMagnetUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public ToggleMagnetResult Execute()
    {
        if (!gameInterop.IsHost())
        {
            return ToggleMagnetResult.HostOnly;
        }

        var newMagnetState = !gameInterop.IsShipMagnetOn();

        // This value is synchronized by the game's built-in server RPC flow.
        gameInterop.ToggleShipMagnet();

        return newMagnetState ? ToggleMagnetResult.MagnetOn : ToggleMagnetResult.MagnetOff;
    }
}