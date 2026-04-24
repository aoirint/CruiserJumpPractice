#nullable enable

using CruiserJumpPractice.Application.Facades;
using CruiserJumpPractice.Application.UseCases;
using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Presentation;

namespace CruiserJumpPractice.Composition;

internal sealed class CompositionRoot
{
    public IGameInterop GameInterop { get; }

    public SaveCruiserStateUseCase SaveCruiserStateUseCase { get; }

    public LoadCruiserStateUseCase LoadCruiserStateUseCase { get; }

    public RequestSaveCruiserStateUseCase RequestSaveCruiserStateUseCase { get; }

    public RequestLoadCruiserStateUseCase RequestLoadCruiserStateUseCase { get; }

    public ToggleMagnetUseCase ToggleMagnetUseCase { get; }

    public ServerCruiserStateFacade ServerCruiserStateFacade { get; }

    public ClientCruiserResultPresenter ClientCruiserResultPresenter { get; }

    public ClientNotificationService ClientNotificationService { get; }

    public ClientCruiserStateFacade ClientCruiserStateFacade { get; }

    public ClientMagnetFacade ClientMagnetFacade { get; }

    public FrameInputHandler FrameInputHandler { get; }

    public StartupInitializer StartupInitializer { get; }

    private CompositionRoot(
        IGameInterop gameInterop,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase,
        ServerCruiserStateFacade serverCruiserStateFacade,
        ClientCruiserResultPresenter clientCruiserResultPresenter,
        ClientNotificationService clientNotificationService,
        ClientCruiserStateFacade clientCruiserStateFacade,
        ClientMagnetFacade clientMagnetFacade,
        FrameInputHandler frameInputHandler,
        StartupInitializer startupInitializer
    )
    {
        GameInterop = gameInterop;
        SaveCruiserStateUseCase = saveCruiserStateUseCase;
        LoadCruiserStateUseCase = loadCruiserStateUseCase;
        RequestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        RequestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        ToggleMagnetUseCase = toggleMagnetUseCase;
        ServerCruiserStateFacade = serverCruiserStateFacade;
        ClientCruiserResultPresenter = clientCruiserResultPresenter;
        ClientNotificationService = clientNotificationService;
        ClientCruiserStateFacade = clientCruiserStateFacade;
        ClientMagnetFacade = clientMagnetFacade;
        FrameInputHandler = frameInputHandler;
        StartupInitializer = startupInitializer;
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
        var serverCruiserStateFacade = new ServerCruiserStateFacade(
            saveCruiserStateUseCase,
            loadCruiserStateUseCase
        );

        var clientNotificationService = new ClientNotificationService(gameInterop);
        var clientCruiserStateFacade = new ClientCruiserStateFacade(
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            clientNotificationService
        );
        var clientMagnetFacade = new ClientMagnetFacade(
            toggleMagnetUseCase,
            clientNotificationService
        );
        var clientCruiserResultPresenter = new ClientCruiserResultPresenter(clientNotificationService);

        var frameInputHandler = new FrameInputHandler(
            gameInterop,
            clientCruiserStateFacade,
            clientMagnetFacade
        );

        return new CompositionRoot(
            gameInterop: gameInterop,
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase,
            serverCruiserStateFacade: serverCruiserStateFacade,
            clientCruiserResultPresenter: clientCruiserResultPresenter,
            clientNotificationService: clientNotificationService,
            clientCruiserStateFacade: clientCruiserStateFacade,
            clientMagnetFacade: clientMagnetFacade,
            frameInputHandler: frameInputHandler,
            startupInitializer: new StartupInitializer(gameInterop)
        );
    }
}
