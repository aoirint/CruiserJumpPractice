#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Finders;

class LocalPlayerIdFinderException : System.Exception
{
    public LocalPlayerIdFinderException(string message) : base(message) { }
}

class LocalPlayerIdFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public int GetLocalPlayerId()
    {
        try
        {
            var gameNetworkManager = GameNetworkManager.Instance;
            if (gameNetworkManager == null)
            {
                throw new LocalPlayerIdFinderException("GameNetworkManager.Instance is null.");
            }

            var localPlayer = gameNetworkManager.localPlayerController;
            if (localPlayer == null)
            {
                throw new LocalPlayerIdFinderException("localPlayerController is null.");
            }

            return (int)localPlayer.playerClientId;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting local player ID: {error}");
            throw new LocalPlayerIdFinderException($"Exception while getting local player ID: {error}");
        }
    }
}
