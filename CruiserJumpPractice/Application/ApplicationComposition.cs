#nullable enable

using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Application.Runtime;
using CruiserJumpPractice.Application.Services.Client;
using CruiserJumpPractice.Application.Services.Server;
using CruiserJumpPractice.Application.UseCases;

namespace CruiserJumpPractice.Application;

internal sealed class ApplicationComposition
{
    public IGameInterop GameInterop { get; }

    public SaveCruiserStateUseCase SaveCruiserStateUseCase { get; }

    public LoadCruiserStateUseCase LoadCruiserStateUseCase { get; }

    public RequestSaveCruiserStateUseCase RequestSaveCruiserStateUseCase { get; }

    public RequestLoadCruiserStateUseCase RequestLoadCruiserStateUseCase { get; }

    public ToggleMagnetUseCase ToggleMagnetUseCase { get; }

    public CruiserStateOperationService CruiserStateOperationService { get; }

    public NotificationUsecase NotificationUsecase { get; }

    public CruiserStateOperationRequestService CruiserStateOperationRequestService { get; }

    public MagnetService MagnetService { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    private ApplicationComposition(
        IGameInterop gameInterop,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase,
        CruiserStateOperationService cruiserStateOperationService,
        NotificationUsecase notificationUsecase,
        CruiserStateOperationRequestService cruiserStateOperationRequestService,
        MagnetService magnetService,
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
        CruiserStateOperationService = cruiserStateOperationService;
        NotificationUsecase = notificationUsecase;
        CruiserStateOperationRequestService = cruiserStateOperationRequestService;
        MagnetService = magnetService;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
    }

    public static ApplicationComposition Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new GameInteropV73(logger);

        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(gameInterop, cruiserStateStore);
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(gameInterop, cruiserStateStore);

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(gameInterop);
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(gameInterop);
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop);
        var cruiserStateOperationService = new CruiserStateOperationService(
            saveCruiserStateUseCase,
            loadCruiserStateUseCase
        );

        var notificationUsecase = new NotificationUsecase(gameInterop);
        var cruiserStateOperationRequestService = new CruiserStateOperationRequestService(
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            notificationUsecase
        );
        var magnetService = new MagnetService(
            toggleMagnetUseCase,
            notificationUsecase
        );

        var frameHandler = new FrameHandler(
            gameInterop,
            cruiserStateOperationRequestService,
            magnetService
        );

        return new ApplicationComposition(
            gameInterop: gameInterop,
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase,
            cruiserStateOperationService: cruiserStateOperationService,
            notificationUsecase: notificationUsecase,
            cruiserStateOperationRequestService: cruiserStateOperationRequestService,
            magnetService: magnetService,
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop)
        );
    }
}
