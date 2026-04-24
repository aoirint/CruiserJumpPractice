#nullable enable

using BepInEx.Logging;
using GameNetcodeStuff;

namespace CruiserJumpPractice.BaseGame.Finders;

class LocalPlayerFinderException : System.Exception
{
    public LocalPlayerFinderException(string message) : base(message) { }
}

class LocalPlayerFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public PlayerControllerB GetLocalPlayer()
    {
        try
        {
            var gameNetworkManager = GameNetworkManager.Instance;
            if (gameNetworkManager == null)
            {
                throw new LocalPlayerFinderException("GameNetworkManager.Instance is null.");
            }

            var localPlayer = gameNetworkManager.localPlayerController;
            if (localPlayer == null)
            {
                throw new LocalPlayerFinderException("localPlayerController is null.");
            }

            return localPlayer;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting local player: {error}");
            throw new LocalPlayerFinderException($"Exception while getting local player: {error}");
        }
    }
}
