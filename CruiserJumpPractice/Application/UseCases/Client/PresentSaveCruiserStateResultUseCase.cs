#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Interop;

namespace CruiserJumpPractice.Application.UseCases.Client;

internal sealed class PresentSaveCruiserStateResultUseCase
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly IGameInterop gameInterop;

    public PresentSaveCruiserStateResultUseCase(IGameInterop gameInterop)
    {
        this.gameInterop = gameInterop;
    }

    public void Execute(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            DisplayTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to save.");
        }
        else
        {
            Logger.LogError($"Unknown SaveCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(string message)
    {
        gameInterop.DisplayTip("CruiserJumpPractice", message);
    }
}
