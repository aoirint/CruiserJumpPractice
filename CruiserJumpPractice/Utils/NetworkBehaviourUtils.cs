#nullable enable

using BepInEx.Logging;
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

        var hudManager = HUDManagerUtils.GetHUDManager();
        if (hudManager == null)
        {
            Logger.LogError("HUDManager instance is null.");
            return null;
        }

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
