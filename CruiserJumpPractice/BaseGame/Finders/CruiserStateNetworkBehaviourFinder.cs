#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.BaseGame.Finders;

class CruiserStateNetworkBehaviourFinderException : System.Exception
{
    public CruiserStateNetworkBehaviourFinderException(string message) : base(message) { }
}

class CruiserStateNetworkBehaviourFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private static CruiserStateNetworkBehaviour? cachedCruiserStateNetworkBehaviour;

    public CruiserStateNetworkBehaviour GetCruiserStateNetworkBehaviour()
    {
        if (cachedCruiserStateNetworkBehaviour != null)
        {
            return cachedCruiserStateNetworkBehaviour;
        }

        try
        {
            var hudManagerFinder = new HUDManagerFinder();
            var hudManager = hudManagerFinder.GetHUDManager();
            var cruiserStateNetworkBehaviour = hudManager.GetComponent<CruiserStateNetworkBehaviour>();
            if (cruiserStateNetworkBehaviour == null)
            {
                throw new CruiserStateNetworkBehaviourFinderException(
                    "CruiserStateNetworkBehaviour component not found on HUDManager instance."
                );
            }

            cachedCruiserStateNetworkBehaviour = cruiserStateNetworkBehaviour;

            return cruiserStateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting CruiserStateNetworkBehaviour: {error}");
            throw new CruiserStateNetworkBehaviourFinderException(
                $"Exception while getting CruiserStateNetworkBehaviour: {error}"
            );
        }
    }
}
