#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Finders;

class HUDManagerFinderException : System.Exception
{
    public HUDManagerFinderException(string message) : base(message) { }
}

class HUDManagerFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public HUDManager GetHUDManager()
    {
        try
        {
            var hudManager = HUDManager.Instance;
            if (hudManager == null)
            {
                throw new HUDManagerFinderException("HUDManager.Instance is null.");
            }

            return hudManager;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting HUDManager: {error}");
            throw new HUDManagerFinderException($"Exception while getting HUDManager: {error}");
        }
    }
}
