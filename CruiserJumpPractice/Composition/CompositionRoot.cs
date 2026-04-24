#nullable enable

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

    public ClientCruiserResultPresenter ClientCruiserResultPresenter { get; }

    public ClientNotificationService ClientNotificationService { get; }

    public FrameInputCoordinator FrameInputCoordinator { get; }

    public StartupInitializer StartupInitializer { get; }

    private CompositionRoot(
        IGameInterop gameInterop,
        SaveCruiserStateUseCase saveCruiserStateUseCase,
        LoadCruiserStateUseCase loadCruiserStateUseCase,
        RequestSaveCruiserStateUseCase requestSaveCruiserStateUseCase,
        RequestLoadCruiserStateUseCase requestLoadCruiserStateUseCase,
        ToggleMagnetUseCase toggleMagnetUseCase,
        ClientCruiserResultPresenter clientCruiserResultPresenter,
        ClientNotificationService clientNotificationService,
        FrameInputCoordinator frameInputCoordinator,
        StartupInitializer startupInitializer
    )
    {
        GameInterop = gameInterop;
        SaveCruiserStateUseCase = saveCruiserStateUseCase;
        LoadCruiserStateUseCase = loadCruiserStateUseCase;
        RequestSaveCruiserStateUseCase = requestSaveCruiserStateUseCase;
        RequestLoadCruiserStateUseCase = requestLoadCruiserStateUseCase;
        ToggleMagnetUseCase = toggleMagnetUseCase;
        ClientCruiserResultPresenter = clientCruiserResultPresenter;
        ClientNotificationService = clientNotificationService;
        FrameInputCoordinator = frameInputCoordinator;
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

        var clientNotificationService = new ClientNotificationService(gameInterop);
        var clientCruiserResultPresenter = new ClientCruiserResultPresenter(clientNotificationService);

        var frameInputCoordinator = new FrameInputCoordinator(
            gameInterop,
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            toggleMagnetUseCase,
            clientNotificationService
        );

        return new CompositionRoot(
            gameInterop: gameInterop,
            saveCruiserStateUseCase: saveCruiserStateUseCase,
            loadCruiserStateUseCase: loadCruiserStateUseCase,
            requestSaveCruiserStateUseCase: requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase: requestLoadCruiserStateUseCase,
            toggleMagnetUseCase: toggleMagnetUseCase,
            clientCruiserResultPresenter: clientCruiserResultPresenter,
            clientNotificationService: clientNotificationService,
            frameInputCoordinator: frameInputCoordinator,
            startupInitializer: new StartupInitializer(gameInterop)
        );
    }
}
