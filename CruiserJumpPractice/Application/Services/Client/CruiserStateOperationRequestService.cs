#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class CruiserStateOperationRequestService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly NotificationService notificationService;

    public CruiserStateOperationRequestService(
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        NotificationService notificationService
    )
    {
        this.requestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        this.requestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        this.notificationService = notificationService;
    }

    internal void RequestSaveCruiserState()
    {
        var result = requestSaveCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            notificationService.DisplayTip("Only the host can save the cruiser state.");
        }
    }

    internal void RequestLoadCruiserState()
    {
        var result = requestLoadCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            notificationService.DisplayTip("Only the host can load the cruiser state.");
        }
    }

    public void PresentSaveResult(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            notificationService.DisplayTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            notificationService.DisplayTip("No cruiser found to save.");
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
            notificationService.DisplayTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            notificationService.DisplayTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            notificationService.DisplayTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            notificationService.DisplayTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }
}
