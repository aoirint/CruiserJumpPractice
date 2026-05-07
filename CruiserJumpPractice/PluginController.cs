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

/// <summary>
/// Plugin-facing facade that exposes named callback actions while keeping Core
/// use cases private.
/// </summary>
/// <remarks>
/// This type lives beside the BepInEx entrypoint because it wires BepInEx
/// logging, InputUtils input, and game interop into Core.
/// </remarks>
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

    /// <summary>
    /// Builds the plugin controller and manually wires concrete integrations to
    /// Core ports.
    /// </summary>
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

    /// <summary>
    /// Records that the save ServerRpc was accepted by the Netcode boundary.
    /// </summary>
    public void RecordSaveServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.SaveServerRpcReceived(GetRole()));
    }

    /// <summary>
    /// Records that a save result ClientRpc was delivered for presentation.
    /// </summary>
    public void RecordSaveClientRpcReceived(SaveCruiserStateResult result)
    {
        validationLogger.Record(
            ValidationLogRecord.SaveClientRpcReceived(role: GetRole(), result: result)
        );
    }

    /// <summary>
    /// Records that the load ServerRpc was accepted by the Netcode boundary.
    /// </summary>
    public void RecordLoadServerRpcReceived()
    {
        validationLogger.Record(ValidationLogRecord.LoadServerRpcReceived(GetRole()));
    }

    /// <summary>
    /// Records that a load result ClientRpc was delivered for presentation.
    /// </summary>
    public void RecordLoadClientRpcReceived(LoadCruiserStateResult result)
    {
        validationLogger.Record(
            record: ValidationLogRecord.LoadClientRpcReceived(role: GetRole(), result: result)
        );
    }

    /// <summary>
    /// Marks entry into the base-game engine-oil ClientRpc receiver path.
    /// </summary>
    public void HandleBaseGameEngineOilClientRpcEntered()
    {
        baseGameAppliedStateValidationHandler.EnterEngineOilClientRpc();
    }

    /// <summary>
    /// Marks exit from the base-game engine-oil ClientRpc receiver path.
    /// </summary>
    public void HandleBaseGameEngineOilClientRpcExited()
    {
        baseGameAppliedStateValidationHandler.ExitEngineOilClientRpc();
    }

    /// <summary>
    /// Captures cruiser HP before the base-game local engine-oil apply helper runs.
    /// </summary>
    public void HandleBaseGameEngineOilLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleEngineOilLocalPreApply();
    }

    /// <summary>
    /// Records cruiser HP after the base-game local engine-oil apply helper runs.
    /// </summary>
    public void HandleBaseGameEngineOilLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleEngineOilLocalApplied();
    }

    /// <summary>
    /// Marks entry into the base-game turbo ClientRpc receiver path.
    /// </summary>
    public void HandleBaseGameTurboClientRpcEntered()
    {
        baseGameAppliedStateValidationHandler.EnterTurboClientRpc();
    }

    /// <summary>
    /// Marks exit from the base-game turbo ClientRpc receiver path.
    /// </summary>
    public void HandleBaseGameTurboClientRpcExited()
    {
        baseGameAppliedStateValidationHandler.ExitTurboClientRpc();
    }

    /// <summary>
    /// Captures turbo count before the base-game local turbo apply helper runs.
    /// </summary>
    public void HandleBaseGameTurboLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleTurboLocalPreApply();
    }

    /// <summary>
    /// Records turbo count after the base-game local turbo apply helper runs.
    /// </summary>
    public void HandleBaseGameTurboLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleTurboLocalApplied();
    }

    /// <summary>
    /// Captures ship-magnet state before the base-game local magnet apply path runs.
    /// </summary>
    public void HandleBaseGameShipMagnetLocalPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetLocalPreApply();
    }

    /// <summary>
    /// Records ship-magnet state after the base-game local magnet apply path runs.
    /// </summary>
    public void HandleBaseGameShipMagnetLocalApplied()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetLocalApplied();
    }

    /// <summary>
    /// Captures ship-magnet state before the base-game magnet ClientRpc apply path runs.
    /// </summary>
    public void HandleBaseGameShipMagnetClientRpcPreApply()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetClientRpcPreApply();
    }

    /// <summary>
    /// Records ship-magnet state after the base-game magnet ClientRpc apply path runs.
    /// </summary>
    public void HandleBaseGameShipMagnetClientRpcApplied()
    {
        baseGameAppliedStateValidationHandler.HandleShipMagnetClientRpcApplied();
    }

    private ValidationLogRole GetRole()
    {
        return gameInterop.IsHost() ? ValidationLogRole.Host : ValidationLogRole.Client;
    }

}
