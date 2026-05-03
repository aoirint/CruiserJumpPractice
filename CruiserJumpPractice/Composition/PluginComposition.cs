#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.Runtime;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.UseCases.Server;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Interop.Game;
using CruiserJumpPractice.Interop.InputUtils;

namespace CruiserJumpPractice.Composition;

internal sealed class PluginComposition
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

    private PluginComposition(
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

    public static PluginComposition Create(ManualLogSource logger, InputUtilsActions inputActions)
    {
        var coreLogger = new BepInExCoreLogger(logger);
        var practiceInput = new InputUtilsPracticeInput(inputActions);
        IGameInterop gameInterop = new GameInterop(logger);

        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            coreLogger
        );
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            coreLogger
        );

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(gameInterop);
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(gameInterop);
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop);
        var presentSaveCruiserStateResultUseCase = new PresentSaveCruiserStateResultUseCase(
            gameInterop,
            coreLogger
        );
        var presentLoadCruiserStateResultUseCase = new PresentLoadCruiserStateResultUseCase(
            gameInterop,
            coreLogger
        );

        var frameHandler = new FrameHandler(
            gameInterop,
            practiceInput,
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            toggleMagnetUseCase
        );

        return new PluginComposition(
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
