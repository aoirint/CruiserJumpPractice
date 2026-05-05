// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Handlers;
using CruiserJumpPractice.Core.Ports;
using CruiserJumpPractice.Core.State;
using CruiserJumpPractice.Core.UseCases;
using CruiserJumpPractice.Core.UseCases.Client;
using CruiserJumpPractice.Core.UseCases.Server;
using CruiserJumpPractice.Core.Validation;
using CruiserJumpPractice.Interop.Game;
using CruiserJumpPractice.Interop.InputUtils;

namespace CruiserJumpPractice;

// The controller is the plugin-facing facade. Callbacks ask it to perform named
// plugin actions, while the controller keeps the actual Core use cases private.
// It lives beside the BepInEx entrypoint because it wires BepInEx logging,
// InputUtils input, and game interop into Core.
internal sealed class PluginController
{
    private readonly IGameInterop gameInterop;
    private readonly IValidationLogger validationLogger;
    private readonly FrameHandler frameHandler;
    private readonly StartupHandler startupHandler;
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;
    private readonly PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase;
    private readonly PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase;

    private PluginController(
        IGameInterop gameInterop,
        IValidationLogger validationLogger,
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase,
        PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase
    )
    {
        this.gameInterop = gameInterop;
        this.validationLogger = validationLogger;
        this.frameHandler = frameHandler;
        this.startupHandler = startupHandler;
        this.saveCruiserStateUseCase = saveCruiserStateUseCase;
        this.loadCruiserStateUseCase = loadCruiserStateUseCase;
        this.presentSaveCruiserStateResultUseCase = presentSaveCruiserStateResultUseCase;
        this.presentLoadCruiserStateResultUseCase = presentLoadCruiserStateResultUseCase;
    }

    public static PluginController Create(IPluginLogger logger, IValidationLogger validationLogger)
    {
        // Concrete integrations become Core ports here. Adding a new external
        // dependency should usually mean adding another adapter here, not
        // another static property on CruiserJumpPractice.
        var inputActions = new InputUtilsActions();
        var practiceInput = new InputUtilsPracticeInput(inputActions);
        IGameInterop gameInterop = new GameInterop(logger, validationLogger);

        var cruiserStateStore = new CruiserStateStore();
        validationLogger.Record(ValidationLogRecord.StateStoreCreated());
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            logger,
            validationLogger
        );
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(
            gameInterop,
            cruiserStateStore,
            logger,
            validationLogger
        );

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(
            gameInterop,
            validationLogger
        );
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(
            gameInterop,
            validationLogger
        );
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop, validationLogger);
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
            validationLogger,
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            toggleMagnetUseCase
        );

        validationLogger.Record(ValidationLogRecord.ControllerCreated());
        return new PluginController(
            gameInterop: gameInterop,
            validationLogger: validationLogger,
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop, validationLogger),
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

    public void RecordSaveServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.SaveServerRpcReceived(GetRole()));
    }

    public void RecordSaveClientRpcReceived(SaveCruiserStateResult result)
    {
        validationLogger.Record(
            ValidationLogRecord.SaveClientRpcReceived(GetRole(), result)
        );
    }

    public void RecordLoadServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.LoadServerRpcReceived(GetRole()));
    }

    public void RecordLoadClientRpcReceived(LoadCruiserStateResult result)
    {
        validationLogger.Record(
            ValidationLogRecord.LoadClientRpcReceived(GetRole(), result)
        );
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }

}
