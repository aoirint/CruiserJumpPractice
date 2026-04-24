#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Services.Client;
using CruiserJumpPractice.Services.Server;

namespace CruiserJumpPractice.Services;

internal sealed class ServiceRegistry
{
    public IGameInterop GameInterop { get; }

    public ClientCruiserStateService ClientCruiserStateService { get; }

    public ServerCruiserStateService ServerCruiserStateService { get; }

    public ClientTickService ClientTickService { get; }

    public ClientRpcSurrogateService ClientRpcSurrogateService { get; }

    public ClientMagnetService ClientMagnetService { get; }

    private ServiceRegistry(
        IGameInterop gameInterop,
        ClientCruiserStateService clientCruiserStateService,
        ServerCruiserStateService serverCruiserStateService,
        ClientTickService clientTickService,
        ClientRpcSurrogateService clientRpcSurrogateService,
        ClientMagnetService clientMagnetService
    )
    {
        GameInterop = gameInterop;
        ClientCruiserStateService = clientCruiserStateService;
        ServerCruiserStateService = serverCruiserStateService;
        ClientTickService = clientTickService;
        ClientRpcSurrogateService = clientRpcSurrogateService;
        ClientMagnetService = clientMagnetService;
    }

    public static ServiceRegistry Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);
        var clientCruiserStateService = new ClientCruiserStateService(gameInterop);
        var clientMagnetService = new ClientMagnetService(gameInterop);
        var clientTickService = new ClientTickService(
            gameInterop,
            clientCruiserStateService,
            clientMagnetService
        );

        return new ServiceRegistry(
            gameInterop: gameInterop,
            clientCruiserStateService: clientCruiserStateService,
            serverCruiserStateService: new ServerCruiserStateService(gameInterop),
            clientTickService: clientTickService,
            clientRpcSurrogateService: new ClientRpcSurrogateService(gameInterop),
            clientMagnetService: clientMagnetService
        );
    }
}
