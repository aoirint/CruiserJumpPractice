#nullable enable

using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application.Facades;

internal sealed class ServerCruiserStateFacade
{
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;

    public ServerCruiserStateFacade(
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
