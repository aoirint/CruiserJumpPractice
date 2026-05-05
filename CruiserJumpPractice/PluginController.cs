// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Handlers;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.UseCases.Server;
using CruiserJumpPractice.Interop.Game;
using CruiserJumpPractice.Interop.InputUtils;

namespace CruiserJumpPractice;

// The controller is the plugin-facing facade. Callbacks ask it to perform named
// plugin actions, while the controller keeps the actual Core use cases private.
// It lives beside the BepInEx entrypoint because it wires BepInEx logging,
// InputUtils input, and game interop into Core.
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

    public static PluginController Create(IPluginLogger logger)
    {
        // Concrete integrations become Core ports here. Adding a new external
        // dependency should usually mean adding another adapter here, not
        // another static property on CruiserJumpPractice.
        var inputActions = new InputUtilsActions();
        var practiceInput = new InputUtilsPracticeInput(inputActions);
        IGameInterop gameInterop = new GameInterop(logger);

        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            logger
        );
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            logger
        );

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(gameInterop);
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(gameInterop);
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop);
        var presentSaveCruiserStateResultUseCase = new PresentSaveCruiserStateResultUseCase(
            gameInterop,
            logger
        );
        var presentLoadCruiserStateResultUseCase = new PresentLoadCruiserStateResultUseCase(
            gameInterop,
            logger
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
