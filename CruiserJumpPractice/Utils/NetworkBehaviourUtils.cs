#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.BaseGame.Finders;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.Utils;

internal static class NetworkBehaviourUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private static CruiserStateNetworkBehaviour? cachedCruiserStateNetworkBehaviour;

    public static CruiserStateNetworkBehaviour? GetCruiserStateNetworkBehaviour()
    {
        if (cachedCruiserStateNetworkBehaviour != null)
        {
            return cachedCruiserStateNetworkBehaviour;
        }

        var hudManagerFinder = new HUDManagerFinder();
        var hudManager = hudManagerFinder.GetHUDManager();

        var cruiserStateNetworkBehaviour = hudManager.GetComponent<CruiserStateNetworkBehaviour>();
        if (cruiserStateNetworkBehaviour == null)
        {
            Logger.LogError("CruiserStateNetworkBehaviour component not found on HUDManager instance.");
            return null;
        }

        cachedCruiserStateNetworkBehaviour = cruiserStateNetworkBehaviour;

        return cruiserStateNetworkBehaviour;
    }
}
