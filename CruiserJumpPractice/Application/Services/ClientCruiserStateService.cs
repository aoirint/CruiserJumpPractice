#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application.Services;

internal sealed class ClientCruiserStateService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly ClientNotificationService clientNotificationService;

    public ClientCruiserStateService(
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ClientNotificationService clientNotificationService
    )
    {
        this.requestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        this.requestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        this.clientNotificationService = clientNotificationService;
    }

    internal void RequestSaveCruiserState()
    {
        var result = requestSaveCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            clientNotificationService.ShowCruiserTip("Only the host can save the cruiser state.");
        }
    }

    internal void RequestLoadCruiserState()
    {
        var result = requestLoadCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            clientNotificationService.ShowCruiserTip("Only the host can load the cruiser state.");
        }
    }

    public void PresentSaveResult(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            clientNotificationService.ShowCruiserTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            clientNotificationService.ShowCruiserTip("No cruiser found to save.");
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
            clientNotificationService.ShowCruiserTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            clientNotificationService.ShowCruiserTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            clientNotificationService.ShowCruiserTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            clientNotificationService.ShowCruiserTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }
}
