#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.Utils;

internal static class NetworkBehaviourUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private static CruiserJumpPracticeNetworkBehaviour? cachedCruiserJumpPracticeNetworkBehaviour;

    public static CruiserJumpPracticeNetworkBehaviour? GetCruiserJumpPracticeNetworkBehaviour()
    {
        if (cachedCruiserJumpPracticeNetworkBehaviour != null)
        {
            return cachedCruiserJumpPracticeNetworkBehaviour;
        }

        var hudManager = HUDManagerUtils.GetHUDManager();
        if (hudManager == null)
        {
            Logger.LogError("HUDManager instance is null.");
            return null;
        }

        var cruiserJumpPracticeNetworkBehaviour = hudManager.GetComponent<CruiserJumpPracticeNetworkBehaviour>();
        if (cruiserJumpPracticeNetworkBehaviour == null)
        {
            Logger.LogError("CruiserJumpPracticeNetworkBehaviour component not found on HUDManager instance.");
            return null;
        }

        cachedCruiserJumpPracticeNetworkBehaviour = cruiserJumpPracticeNetworkBehaviour;

        return cruiserJumpPracticeNetworkBehaviour;
    }
}
