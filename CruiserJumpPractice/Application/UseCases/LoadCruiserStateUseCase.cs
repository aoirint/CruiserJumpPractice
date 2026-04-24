#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.UseCases;

internal sealed class LoadCruiserStateUseCase
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;

    public LoadCruiserStateUseCase(IGameInterop gameInterop, CruiserStateStore cruiserStateStore)
    {
        this.gameInterop = gameInterop;
        this.cruiserStateStore = cruiserStateStore;
    }

    public LoadCruiserStateResult Execute()
    {
        try
        {
            var cruiser = gameInterop.FindCruiser();
            if (cruiser == null)
            {
                Logger.LogInfo("No cruiser found.");
                return LoadCruiserStateResult.NoCruiserFound;
            }

            var savedCruiserState = cruiserStateStore.SavedCruiserState;
            if (savedCruiserState == null)
            {
                Logger.LogInfo("No saved cruiser state found.");
                return LoadCruiserStateResult.NoSavedState;
            }

            var magnetedToShip = gameInterop.IsCruiserMagnetedToShip(cruiser);
            if (magnetedToShip)
            {
                Logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
                return LoadCruiserStateResult.MagnetedToShip;
            }

            gameInterop.RestoreCruiser(cruiser, savedCruiserState);
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while loading cruiser state: {error}");
            return LoadCruiserStateResult.UnexpectedState;
        }
    }
}