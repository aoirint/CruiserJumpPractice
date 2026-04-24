#nullable enable

using CruiserJumpPractice.UseCases;
using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.Interop;
using CruiserJumpPractice.Runtime;

namespace CruiserJumpPractice.Services;

internal sealed class CompositionRoot
{
    public IGameInterop GameInterop { get; }

    public SaveCruiserStateUseCase SaveCruiserStateUseCase { get; }

    public LoadCruiserStateUseCase LoadCruiserStateUseCase { get; }

    public RequestSaveCruiserStateUseCase RequestSaveCruiserStateUseCase { get; }

    public RequestLoadCruiserStateUseCase RequestLoadCruiserStateUseCase { get; }

    public ToggleMagnetUseCase ToggleMagnetUseCase { get; }

    public ServerCruiserStateService ServerCruiserStateService { get; }

    public ClientCruiserResultPresenter ClientCruiserResultPresenter { get; }

    public ClientNotificationService ClientNotificationService { get; }

    public ClientCruiserStateService ClientCruiserStateService { get; }

    public ClientMagnetService ClientMagnetService { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    private CompositionRoot(
        IGameInterop gameInterop,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase,
        ServerCruiserStateService serverCruiserStateService,
        ClientCruiserResultPresenter clientCruiserResultPresenter,
        ClientNotificationService clientNotificationService,
        ClientCruiserStateService clientCruiserStateService,
        ClientMagnetService clientMagnetService,
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
        ServerCruiserStateService = serverCruiserStateService;
        ClientCruiserResultPresenter = clientCruiserResultPresenter;
        ClientNotificationService = clientNotificationService;
        ClientCruiserStateService = clientCruiserStateService;
        ClientMagnetService = clientMagnetService;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
    }

    public static CompositionRoot Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);

        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(gameInterop, cruiserStateStore);
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(gameInterop, cruiserStateStore);

        var requestSaveCruiserStateUseCase = new RequestSaveCruiserStateUseCase(gameInterop);
        var requestLoadCruiserStateUseCase = new RequestLoadCruiserStateUseCase(gameInterop);
        var toggleMagnetUseCase = new ToggleMagnetUseCase(gameInterop);
        var serverCruiserStateService = new ServerCruiserStateService(
            saveCruiserStateUseCase,
            loadCruiserStateUseCase
        );

        var clientNotificationService = new ClientNotificationService(gameInterop);
        var clientCruiserStateService = new ClientCruiserStateService(
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            clientNotificationService
        );
        var clientMagnetService = new ClientMagnetService(
            toggleMagnetUseCase,
            clientNotificationService
        );
        var clientCruiserResultPresenter = new ClientCruiserResultPresenter(clientNotificationService);

        var frameHandler = new FrameHandler(
            gameInterop,
            clientCruiserStateService,
            clientMagnetService
        );

        return new CompositionRoot(
            gameInterop: gameInterop,
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase,
            serverCruiserStateService: serverCruiserStateService,
            clientCruiserResultPresenter: clientCruiserResultPresenter,
            clientNotificationService: clientNotificationService,
            clientCruiserStateService: clientCruiserStateService,
            clientMagnetService: clientMagnetService,
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop)
        );
    }
}
