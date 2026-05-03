#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.Core.Handlers;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.UseCases.Server;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Interop.Game;
using CruiserJumpPractice.Interop.InputUtils;

namespace CruiserJumpPractice;

// Harmony patches and Netcode behaviours enter the mod through this plugin boundary.
// The file stays next to the BepInEx entrypoint because it describes plugin-level operations,
// not game adapters or Core policy. Keeping the dependency graph here also prevents Core from
// learning about BepInEx, InputUtils, Harmony, or the concrete Lethal Company objects.
internal sealed class PluginController
{
    private readonly FrameHandler frameHandler;
    private readonly StartupHandler startupHandler;
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;
    private readonly PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase;
    private readonly PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase;

    private PluginController(
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase,
        PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase
    )
    {
        this.frameHandler = frameHandler;
        this.startupHandler = startupHandler;
        this.saveCruiserStateUseCase = saveCruiserStateUseCase;
        this.loadCruiserStateUseCase = loadCruiserStateUseCase;
        this.presentSaveCruiserStateResultUseCase = presentSaveCruiserStateResultUseCase;
        this.presentLoadCruiserStateResultUseCase = presentLoadCruiserStateResultUseCase;
    }

    public static PluginController Create(ManualLogSource logger)
    {
        // External tools are adapted to Core ports in one place, then game callbacks go through
        // the semantic methods below instead of reaching into individual use cases.
        var coreLogger = new BepInExCoreLogger(logger);
        var inputActions = new InputUtilsActions();
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

        return new PluginController(
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop),
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            presentSaveCruiserStateResultUseCase: presentSaveCruiserStateResultUseCase,
            presentLoadCruiserStateResultUseCase: presentLoadCruiserStateResultUseCase
        );
    }

    public void HandleStartup()
    {
        startupHandler.HandleStartup();
    }

    public void HandleFrame()
    {
        frameHandler.HandleFrame();
    }

    public SaveCruiserStateResult SaveCruiserState()
    {
        return saveCruiserStateUseCase.Execute();
    }

    public LoadCruiserStateResult LoadCruiserState()
    {
        return loadCruiserStateUseCase.Execute();
    }

    public void PresentSaveCruiserStateResult(SaveCruiserStateResult result)
    {
        presentSaveCruiserStateResultUseCase.Execute(result);
    }

    public void PresentLoadCruiserStateResult(LoadCruiserStateResult result)
    {
        presentLoadCruiserStateResultUseCase.Execute(result);
    }
}
