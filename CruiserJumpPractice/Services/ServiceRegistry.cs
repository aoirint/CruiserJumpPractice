#nullable enable

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

    public ClientFrameHandler ClientFrameHandler { get; }

    public ClientStartupHandler ClientStartupHandler { get; }

    public ClientMagnetService ClientMagnetService { get; }

    private ServiceRegistry(
        IGameInterop gameInterop,
        ClientCruiserStateService clientCruiserStateService,
        ServerCruiserStateService serverCruiserStateService,
        ClientFrameHandler clientFrameHandler,
        ClientStartupHandler clientStartupHandler,
        ClientMagnetService clientMagnetService
    )
    {
        GameInterop = gameInterop;
        ClientCruiserStateService = clientCruiserStateService;
        ServerCruiserStateService = serverCruiserStateService;
        ClientFrameHandler = clientFrameHandler;
        ClientStartupHandler = clientStartupHandler;
        ClientMagnetService = clientMagnetService;
    }

    public static ServiceRegistry Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);
        var clientCruiserStateService = new ClientCruiserStateService(gameInterop);
        var clientMagnetService = new ClientMagnetService(gameInterop);
        var clientFrameHandler = new ClientFrameHandler(
            gameInterop,
            clientCruiserStateService,
            clientMagnetService
        );

        return new ServiceRegistry(
            gameInterop: gameInterop,
            clientCruiserStateService: clientCruiserStateService,
            serverCruiserStateService: new ServerCruiserStateService(gameInterop),
            clientFrameHandler: clientFrameHandler,
            clientStartupHandler: new ClientStartupHandler(gameInterop),
            clientMagnetService: clientMagnetService
        );
    }
}
