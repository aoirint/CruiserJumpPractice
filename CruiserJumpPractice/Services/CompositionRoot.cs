#nullable enable

using CruiserJumpPractice.Application.UseCases;
using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Presentation;
using CruiserJumpPractice.Services.Client;
using CruiserJumpPractice.Services.Server;

namespace CruiserJumpPractice.Services;

internal sealed class CompositionRoot
{
    public IGameInterop GameInterop { get; }

    public ClientCruiserStateCoordinator ClientCruiserStateCoordinator { get; }

    public ServerCruiserStateCoordinator ServerCruiserStateCoordinator { get; }

    public ClientCruiserResultPresenter ClientCruiserResultPresenter { get; }

    public FrameInputCoordinator FrameInputCoordinator { get; }

    public StartupInitializer StartupInitializer { get; }

    public ClientMagnetCoordinator ClientMagnetCoordinator { get; }

    private CompositionRoot(
        IGameInterop gameInterop,
        ClientCruiserStateCoordinator clientCruiserStateCoordinator,
        ServerCruiserStateCoordinator serverCruiserStateCoordinator,
        ClientCruiserResultPresenter clientCruiserResultPresenter,
        FrameInputCoordinator frameInputCoordinator,
        StartupInitializer startupInitializer,
        ClientMagnetCoordinator clientMagnetCoordinator
    )
    {
        GameInterop = gameInterop;
        ClientCruiserStateCoordinator = clientCruiserStateCoordinator;
        ServerCruiserStateCoordinator = serverCruiserStateCoordinator;
        ClientCruiserResultPresenter = clientCruiserResultPresenter;
        FrameInputCoordinator = frameInputCoordinator;
        StartupInitializer = startupInitializer;
        ClientMagnetCoordinator = clientMagnetCoordinator;
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
        var clientCruiserStateCoordinator = new ClientCruiserStateCoordinator(
            requestSaveCruiserStateUseCase,
            requestLoadCruiserStateUseCase,
            clientNotificationService
        );
        var clientCruiserResultPresenter = new ClientCruiserResultPresenter(clientNotificationService);
        var clientMagnetCoordinator = new ClientMagnetCoordinator(
            toggleMagnetUseCase,
            clientNotificationService
        );

        var frameInputCoordinator = new FrameInputCoordinator(
            gameInterop,
            clientCruiserStateCoordinator,
            clientMagnetCoordinator
        );

        return new CompositionRoot(
            gameInterop: gameInterop,
            clientCruiserStateCoordinator: clientCruiserStateCoordinator,
            serverCruiserStateCoordinator: new ServerCruiserStateCoordinator(
                saveCruiserStateUseCase,
                loadCruiserStateUseCase
            ),
            clientCruiserResultPresenter: clientCruiserResultPresenter,
            frameInputCoordinator: frameInputCoordinator,
            startupInitializer: new StartupInitializer(gameInterop),
            clientMagnetCoordinator: clientMagnetCoordinator
        );
    }
}
