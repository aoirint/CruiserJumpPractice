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
    private readonly BaseGameAppliedStateValidationHandler baseGameAppliedStateValidationHandler;
    private readonly SaveCruiserStateUseCase saveCruiserStateUseCase;
    private readonly LoadCruiserStateUseCase loadCruiserStateUseCase;
    private readonly PresentSaveCruiserStateResultUseCase presentSaveCruiserStateResultUseCase;
    private readonly PresentLoadCruiserStateResultUseCase presentLoadCruiserStateResultUseCase;

    private PluginController(
        IGameInterop gameInterop,
        IValidationLogger validationLogger,
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        BaseGameAppliedStateValidationHandler baseGameAppliedStateValidationHandler,
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
        this.baseGameAppliedStateValidationHandler = baseGameAppliedStateValidationHandler;
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
        IGameInterop gameInterop = new GameInterop(
            logger: logger,
            validationLogger: validationLogger
        );

        var cruiserStateStore = new CruiserStateStore();
        var baseGameAppliedStateValidationStore = new BaseGameAppliedStateValidationStore();
        validationLogger.Record(ValidationLogRecord.StateStoreCreated());

        // Manual wiring is grouped by direction of control: server state
        // mutations, client requests/presentation, then frame/startup handlers.
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(
            gameInterop: gameInterop,
            cruiserStateStore: cruiserStateStore,
            logger: logger,
            validationLogger: validationLogger
        );
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(
            gameInterop: gameInterop,
            cruiserStateStore: cruiserStateStore,
            logger: logger,
            validationLogger: validationLogger
        );

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(
            gameInterop: gameInterop,
            validationLogger: validationLogger
        );
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(
            gameInterop: gameInterop,
            validationLogger: validationLogger
        );
        var toggleMagnetUseCase = new ToggleMagnetUseCase(
            gameInterop: gameInterop,
            validationLogger: validationLogger
        );
        var presentSaveCruiserStateResultUseCase = new PresentSaveCruiserStateResultUseCase(
            gameInterop: gameInterop,
            logger: logger
        );
        var presentLoadCruiserStateResultUseCase = new PresentLoadCruiserStateResultUseCase(
            gameInterop: gameInterop,
            logger: logger
        );

        var frameHandler = new FrameHandler(
            gameInterop: gameInterop,
            practiceInput: practiceInput,
            validationLogger: validationLogger,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase
        );

        validationLogger.Record(ValidationLogRecord.ControllerCreated());
        return new PluginController(
            gameInterop: gameInterop,
            validationLogger: validationLogger,
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(
                gameInterop: gameInterop,
                validationLogger: validationLogger
            ),
            baseGameAppliedStateValidationHandler: new BaseGameAppliedStateValidationHandler(
                gameInterop: gameInterop,
                validationLogger: validationLogger,
                stateStore: baseGameAppliedStateValidationStore
            ),
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

    // Netcode callbacks enter through these methods so RPC receipt logging stays
    // at the same boundary as result presentation.
    public void RecordSaveServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.SaveServerRpcReceived(GetRole()));
    }

    public void RecordSaveClientRpcReceived(SaveCruiserStateResult result)
    {
        validationLogger.Record(
            ValidationLogRecord.SaveClientRpcReceived(role: GetRole(), result: result)
        );
    }

    public void RecordLoadServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.LoadServerRpcReceived(GetRole()));
    }

    public void RecordLoadClientRpcReceived(LoadCruiserStateResult result)
    {
        validationLogger.Record(
            record: ValidationLogRecord.LoadClientRpcReceived(role: GetRole(), result: result)
        );
    }

    // Base-game Harmony callbacks use these narrow methods to keep patch
    // classes free of validation-store details.
    public void HandleBaseGameEngineOilClientRpcEntered()
    {
        baseGameAppliedStateValidationHandler.EnterEngineOilClientRpc();
    }

    public void HandleBaseGameEngineOilClientRpcExited()
    {
        baseGameAppliedStateValidationHandler.ExitEngineOilClientRpc();
    }

    public void HandleBaseGameEngineOilLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleEngineOilLocalPreApply();
    }

    public void HandleBaseGameEngineOilLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleEngineOilLocalApplied();
    }

    public void HandleBaseGameTurboClientRpcEntered()
    {
        baseGameAppliedStateValidationHandler.EnterTurboClientRpc();
    }

    public void HandleBaseGameTurboClientRpcExited()
    {
        baseGameAppliedStateValidationHandler.ExitTurboClientRpc();
    }

    public void HandleBaseGameTurboLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleTurboLocalPreApply();
    }

    public void HandleBaseGameTurboLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleTurboLocalApplied();
    }

    public void HandleBaseGameShipMagnetLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetLocalPreApply();
    }

    public void HandleBaseGameShipMagnetLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetLocalApplied();
    }

    public void HandleBaseGameShipMagnetClientRpcPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetClientRpcPreApply();
    }

    public void HandleBaseGameShipMagnetClientRpcApplied()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetClientRpcApplied();
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }

}
