#nullable enable

using CruiserJumpPractice.Application.UseCases;
using CruiserJumpPractice.Domain;
using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Runtime;
using CruiserJumpPractice.Services.Client;
using CruiserJumpPractice.Services.Server;

namespace CruiserJumpPractice.Services;

internal sealed class ServiceRegistry
{
    public IGameInterop GameInterop { get; }

    public ClientCruiserStateService ClientCruiserStateService { get; }

    public ServerCruiserStateService ServerCruiserStateService { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    public ClientMagnetService ClientMagnetService { get; }

    private ServiceRegistry(
        IGameInterop gameInterop,
        ClientCruiserStateService clientCruiserStateService,
        ServerCruiserStateService serverCruiserStateService,
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        ClientMagnetService clientMagnetService
    )
    {
        GameInterop = gameInterop;
        ClientCruiserStateService = clientCruiserStateService;
        ServerCruiserStateService = serverCruiserStateService;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
        ClientMagnetService = clientMagnetService;
    }

    public static ServiceRegistry Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);
        var cruiserStateStore = new CruiserStateStore();
        var saveCruiserStateUseCase = new SaveCruiserStateUseCase(gameInterop, cruiserStateStore);
        var loadCruiserStateUseCase = new LoadCruiserStateUseCase(gameInterop, cruiserStateStore);
        var clientCruiserStateService = new ClientCruiserStateService(gameInterop);
        var clientMagnetService = new ClientMagnetService(gameInterop);
        var frameHandler = new FrameHandler(
            gameInterop,
            clientCruiserStateService,
            clientMagnetService
        );

        return new ServiceRegistry(
            gameInterop: gameInterop,
            clientCruiserStateService: clientCruiserStateService,
            serverCruiserStateService: new ServerCruiserStateService(
                saveCruiserStateUseCase,
                loadCruiserStateUseCase
            ),
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop),
            clientMagnetService: clientMagnetService
        );
    }
}
