#nullable enable

using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;

namespace CruiserJumpPractice.Core.UseCases.Server;

internal sealed class SaveCruiserStateUseCase
{
    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;
    private readonly ICoreLogger logger;

    public SaveCruiserStateUseCase(
        IGameInterop gameInterop,
        CruiserStateStore cruiserStateStore,
        ICoreLogger logger
    )
    {
        this.gameInterop = gameInterop;
        this.cruiserStateStore = cruiserStateStore;
        this.logger = logger;
    }

    public SaveCruiserStateResult Execute()
    {
        try
        {
            var cruiserState = gameInterop.CaptureCruiser();
            if (cruiserState == null)
            {
                logger.LogInfo("No cruiser found.");
                return SaveCruiserStateResult.NoCruiserFound;
            }

            cruiserStateStore.SavedCruiserState = cruiserState;
            return SaveCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            logger.LogError($"Exception while saving cruiser state: {error}");
            return SaveCruiserStateResult.UnexpectedState;
        }
    }
}
