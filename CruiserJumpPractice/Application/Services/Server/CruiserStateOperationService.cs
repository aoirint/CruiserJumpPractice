#nullable enable

using CruiserJumpPractice.Domain;
using CruiserJumpPractice.Application.UseCases.Server;

namespace CruiserJumpPractice.Application.Services.Server;

internal sealed class CruiserStateOperationService
{
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;

    public CruiserStateOperationService(
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
