#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Core.UseCases.Server;

internal sealed class SaveCruiserStateUseCase
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;
    private readonly CruiserStateStore cruiserStateStore;

    public SaveCruiserStateUseCase(IGameInterop gameInterop, CruiserStateStore cruiserStateStore)
    {
        this.gameInterop = gameInterop;
        this.cruiserStateStore = cruiserStateStore;
    }

    public SaveCruiserStateResult Execute()
    {
        try
        {
            var cruiserState = gameInterop.CaptureCruiser();
            if (cruiserState == null)
            {
                Logger.LogInfo("No cruiser found.");
                return SaveCruiserStateResult.NoCruiserFound;
            }

            cruiserStateStore.SavedCruiserState = cruiserState;
            return SaveCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while saving cruiser state: {error}");
            return SaveCruiserStateResult.UnexpectedState;
        }
    }
}
