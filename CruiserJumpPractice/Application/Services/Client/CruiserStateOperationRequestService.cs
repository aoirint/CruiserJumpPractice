#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases.Client;

namespace CruiserJumpPractice.Application.Services.Client;

internal sealed class CruiserStateOperationRequestService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly NotificationUseCase notificationUseCase;

    public CruiserStateOperationRequestService(
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        NotificationUseCase notificationUseCase
    )
    {
        this.requestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        this.requestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        this.notificationUseCase = notificationUseCase;
    }

    internal void RequestSaveCruiserState()
    {
        var result = requestSaveCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            notificationUseCase.DisplayTip("Only the host can save the cruiser state.");
        }
    }

    internal void RequestLoadCruiserState()
    {
        var result = requestLoadCruiserStateUseCase.Execute();
        if (result == HostGuardResult.HostOnly)
        {
            notificationUseCase.DisplayTip("Only the host can load the cruiser state.");
        }
    }

    public void PresentSaveResult(SaveCruiserStateResult result)
    {
        if (result == SaveCruiserStateResult.Success)
        {
            notificationUseCase.DisplayTip("Cruiser state saved.");
        }
        else if (result == SaveCruiserStateResult.NoCruiserFound)
        {
            notificationUseCase.DisplayTip("No cruiser found to save.");
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
            notificationUseCase.DisplayTip("Cruiser state loaded.");
        }
        else if (result == LoadCruiserStateResult.NoCruiserFound)
        {
            notificationUseCase.DisplayTip("No cruiser found to load.");
        }
        else if (result == LoadCruiserStateResult.NoSavedState)
        {
            notificationUseCase.DisplayTip("No saved cruiser state to load.");
        }
        else if (result == LoadCruiserStateResult.MagnetedToShip)
        {
            notificationUseCase.DisplayTip("Cannot load cruiser state while magneted to ship.");
        }
        else
        {
            Logger.LogError($"Unknown LoadCruiserStateResult: {result}");
        }
    }
}
