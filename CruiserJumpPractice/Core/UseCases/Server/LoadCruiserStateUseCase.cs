#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.UseCases.Server;

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
            if (!gameInterop.CruiserExists())
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

            if (gameInterop.IsCruiserMagnetedToShip())
            {
                Logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
                return LoadCruiserStateResult.MagnetedToShip;
            }

            gameInterop.RestoreCruiser(savedCruiserState);
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while loading cruiser state: {error}");
            return LoadCruiserStateResult.UnexpectedState;
        }
    }
}
