#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.GameInterop;
using CruiserJumpPractice.Runtime;

namespace CruiserJumpPractice.Services;

internal sealed class ServiceRegistry
{
    public IGameInterop GameInterop { get; }

    public CruiserStateClientService CruiserStateClientService { get; }

    public CruiserStateServerService CruiserStateServerService { get; }

    public FrameHandler FrameHandler { get; }

    public StartupHandler StartupHandler { get; }

    public MagnetService MagnetService { get; }

    private ServiceRegistry(
        IGameInterop gameInterop,
        CruiserStateClientService cruiserStateClientService,
        CruiserStateServerService cruiserStateServerService,
        FrameHandler frameHandler,
        StartupHandler startupHandler,
        MagnetService magnetService
    )
    {
        GameInterop = gameInterop;
        CruiserStateClientService = cruiserStateClientService;
        CruiserStateServerService = cruiserStateServerService;
        FrameHandler = frameHandler;
        StartupHandler = startupHandler;
        MagnetService = magnetService;
    }

    public static ServiceRegistry Create(ManualLogSource logger)
    {
        IGameInterop gameInterop = new CurrentGameInterop(logger);
        var cruiserStateClientService = new CruiserStateClientService(gameInterop);
        var magnetService = new MagnetService(gameInterop);
        var frameHandler = new FrameHandler(
            gameInterop,
            cruiserStateClientService,
            magnetService
        );

        return new ServiceRegistry(
            gameInterop: gameInterop,
            cruiserStateClientService: cruiserStateClientService,
            cruiserStateServerService: new CruiserStateServerService(gameInterop),
            frameHandler: frameHandler,
            startupHandler: new StartupHandler(gameInterop),
            magnetService: magnetService
        );
    }
}
