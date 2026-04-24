#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Finders;

namespace CruiserJumpPractice.BaseGame.Controllers.Server;

class MagnetControllerException : System.Exception
{
    public MagnetControllerException(string message) : base(message) { }
}

class MagnetController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected StartOfRound startOfRound;

    public MagnetController(StartOfRound startOfRound)
    {
        this.startOfRound = startOfRound;
    }

    public bool IsMagnetOn()
    {
        try
        {
            return startOfRound.magnetOn;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'magnetOn': {error}");
            throw new MagnetControllerException($"Exception while getting 'magnetOn': {error}");
        }
    }

    public void ToggleMagnet()
    {
        try
        {
            var magnetLever = startOfRound.magnetLever;
            if (magnetLever == null)
            {
                throw new MagnetControllerException("StartOfRound.magnetLever is null.");
            }

            var localPlayerFinder = new LocalPlayerFinder();
            var localPlayer = localPlayerFinder.GetLocalPlayer();

            // NOTE: This AnimatedObjectTrigger method calls StartOfRound.SetMagnetOn and sends a ServerRpc internally.
            magnetLever.TriggerAnimation(localPlayer);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while toggling magnet: {error}");
            throw new MagnetControllerException($"Exception while toggling magnet: {error}");
        }
    }
}
