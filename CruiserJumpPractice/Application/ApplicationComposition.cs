#nullable enable

using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application.Runtime;
using CruiserJumpPractice.Application.UseCases.Client;
using CruiserJumpPractice.Application.UseCases.Server;

namespace CruiserJumpPractice.Application;

internal sealed class ApplicationComposition
{
    public IGameInterop GameInterop { get; }

    public SaveCruiserStateUseCase SaveCruiserStateUseCase { get; }

    public LoadCruiserStateUseCase LoadCruiserStateUseCase { get; }

    public RequestSaveCruiserStateUseCase RequestSaveCruiserStateUseCase { get; }

    public RequestLoadCruiserStateUseCase RequestLoadCruiserStateUseCase { get; }

    public ToggleMagnetUseCase ToggleMagnetUseCase { get; }

    public PresentSaveCruiserStateResultUseCase PresentSaveCruiserStateResultUseCase { get; }

    public PresentLoadCruiserStateResultUseCase PresentLoadCruiserStateResultUseCase { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    private ApplicationComposition(
        IGameInterop gameInterop,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase,
        PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase,
        PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase,
        FrameHandler frameHandler,
        StartupHandler startupHandler
    )
    {
        GameInterop = gameInterop;
        SaveCruiserStateUseCase = saveCruiserStateUseCase;
        LoadCruiserStateUseCase = loadCruiserStateUseCase;
        RequestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        RequestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        ToggleMagnetUseCase = toggleMagnetUseCase;
        PresentSaveCruiserStateResultUseCase = presentSaveCruiserStateResultUseCase;
        PresentLoadCruiserStateResultUseCase = presentLoadCruiserStateResultUseCase;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
    }

    public static ApplicationComposition Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new GameInteropCurrent(logger);

        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(gameInterop, cruiserStateStore);
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(gameInterop, cruiserStateStore);

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(gameInterop);
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(gameInterop);
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop);
        var presentSaveCruiserStateResultUseCase = new PresentSaveCruiserStateResultUseCase(gameInterop);
        var presentLoadCruiserStateResultUseCase = new PresentLoadCruiserStateResultUseCase(gameInterop);

        var frameHandler = new FrameHandler(
            gameInterop,
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            toggleMagnetUseCase
        );

        return new ApplicationComposition(
            gameInterop: gameInterop,
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase,
            presentSaveCruiserStateResultUseCase: presentSaveCruiserStateResultUseCase,
            presentLoadCruiserStateResultUseCase: presentLoadCruiserStateResultUseCase,
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop)
        );
    }
}
