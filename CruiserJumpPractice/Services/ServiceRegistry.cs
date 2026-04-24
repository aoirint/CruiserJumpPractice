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

    public CruiserStateService CruiserStateService { get; }

    public ServerCruiserStateService ServerCruiserStateService { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    public MagnetService MagnetService { get; }

    private ServiceRegistry(
        IGameInterop gameInterop,
        CruiserStateService cruiserStateService,
        ServerCruiserStateService serverCruiserStateService,
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        MagnetService magnetService
    )
    {
        GameInterop = gameInterop;
        CruiserStateService = cruiserStateService;
        ServerCruiserStateService = serverCruiserStateService;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
        MagnetService = magnetService;
    }

    public static ServiceRegistry Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);
        var cruiserStateService = new CruiserStateService(gameInterop);
        var magnetService = new MagnetService(gameInterop);
        var frameHandler = new FrameHandler(
            gameInterop,
            cruiserStateService,
            magnetService
        );

        return new ServiceRegistry(
            gameInterop: gameInterop,
            cruiserStateService: cruiserStateService,
            serverCruiserStateService: new ServerCruiserStateService(gameInterop),
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop),
            magnetService: magnetService
        );
    }
}
