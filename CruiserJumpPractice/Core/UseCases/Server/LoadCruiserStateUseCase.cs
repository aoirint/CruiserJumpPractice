#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.UseCases.Server;

// Restoring a cruiser snapshot touches live game physics, so the checks live in the server
// use case with the restore operation. Client presentation receives only the result enum.
internal sealed class LoadCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;
    private readonly ICoreLogger logger;

    public LoadCruiserStateUseCase(
        IGameInterop gameInterop,
        CruiserStateStore cruiserStateStore,
        ICoreLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateStore = cruiserStateStore;
        this.logger = logger;
    }

    public LoadCruiserStateResult Execute()
    {
        try
        {
            if (!gameInterop.CruiserExists())
            {
                logger.LogInfo("No cruiser found.");
                return LoadCruiserStateResult.NoCruiserFound;
            }

            var savedCruiserState = cruiserStateStore.SavedCruiserState;
            if (savedCruiserState == null)
            {
                logger.LogInfo("No saved cruiser state found.");
                return LoadCruiserStateResult.NoSavedState;
            }

            if (gameInterop.IsCruiserMagnetedToShip())
            {
                logger.LogInfo("Cruiser is currently magneted to the ship. Cannot load state.");
                return LoadCruiserStateResult.MagnetedToShip;
            }

            gameInterop.RestoreCruiser(savedCruiserState);
            return LoadCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while loading cruiser state: {error}");
            return LoadCruiserStateResult.UnexpectedState;
        }
    }
}
