#nullable enable

using CruiserJumpPractice.Application;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Presentation;

internal sealed class ClientCruiserStateCoordinator
{
    private readonly RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase;
    private readonly RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase;
    private readonly ClientNotificationService clientNotificationService;

    public ClientCruiserStateCoordinator(
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
}
