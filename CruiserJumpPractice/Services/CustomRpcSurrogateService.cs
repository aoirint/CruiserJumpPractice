#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.Services;

internal class CustomRpcSurrogateService
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal void Spawn(HUDManager hudManager)
    {
        var gameObject = hudManager.gameObject;
        if (gameObject == null)
        {
            Logger.LogError("HUDManager.gameObject is null.");
            return;
        }

        if (gameObject.GetComponent<CustomRpcSurrogateNetworkBehaviour>() != null)
        {
            Logger.LogDebug("Custom RPC surrogate already exists on HUDManager.");
            return;
        }

        gameObject.AddComponent<CustomRpcSurrogateNetworkBehaviour>();
        Logger.LogInfo("Spawned custom RPC surrogate on HUDManager.");
    }
}
