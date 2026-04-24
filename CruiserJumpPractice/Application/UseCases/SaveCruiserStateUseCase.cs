#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.UseCases;

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
            var cruiser = gameInterop.FindCruiser();
            if (cruiser == null)
            {
                Logger.LogInfo("No cruiser found.");
                return SaveCruiserStateResult.NoCruiserFound;
            }

            cruiserStateStore.SavedCruiserState = gameInterop.CaptureCruiser(cruiser);
            return SaveCruiserStateResult.Success;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while saving cruiser state: {error}");
            return SaveCruiserStateResult.UnexpectedState;
        }
    }
}