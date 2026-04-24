#nullable enable

using CruiserJumpPractice.Application;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Services.Server;

internal sealed class ServerCruiserStateCoordinator
{
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;

    public ServerCruiserStateCoordinator(
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase
    )
    {
        this.saveCruiserStateUseCase = saveCruiserStateUseCase;
        this.loadCruiserStateUseCase = loadCruiserStateUseCase;
    }

    internal SaveCruiserStateResult SaveCruiserState()
    {
        return saveCruiserStateUseCase.Execute();
    }

    internal LoadCruiserStateResult LoadCruiserState()
    {
        return loadCruiserStateUseCase.Execute();
    }
}
