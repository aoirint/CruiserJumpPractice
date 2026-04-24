#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;

namespace CruiserJumpPractice.Services;

internal sealed class ClientCruiserResultPresenter
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly ClientNotificationService clientNotificationService;

    public ClientCruiserResultPresenter(ClientNotificationService clientNotificationService)
    {
        this.clientNotificationService = clientNotificationService;
    }

    public void PresentSaveResult(SaveCruiserStateResult result)
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

    public void PresentLoadResult(LoadCruiserStateResult result)
    {
        if (result == LoadCruiserStateResult.Success)
        {
            DisplayTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            DisplayTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            DisplayTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            DisplayTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }

    private void DisplayTip(string bodyText)
    {
        clientNotificationService.ShowCruiserTip(bodyText);
    }
}