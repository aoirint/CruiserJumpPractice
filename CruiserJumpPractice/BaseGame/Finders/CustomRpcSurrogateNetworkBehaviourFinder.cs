#nullable enable

using BepInEx.Logging;
using CruiserJumpPractice.NetworkBehaviours;

namespace CruiserJumpPractice.BaseGame.Finders;

class CustomRpcSurrogateNetworkBehaviourFinderException : System.Exception
{
    public CustomRpcSurrogateNetworkBehaviourFinderException(string message) : base(message) { }
}

class CustomRpcSurrogateNetworkBehaviourFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    private static CustomRpcSurrogateNetworkBehaviour? cachedCustomRpcSurrogateNetworkBehaviour;

    public CustomRpcSurrogateNetworkBehaviour GetCustomRpcSurrogateNetworkBehaviour()
    {
        if (cachedCustomRpcSurrogateNetworkBehaviour != null)
        {
            return cachedCustomRpcSurrogateNetworkBehaviour;
        }

        try
        {
            var hudManagerFinder = new HUDManagerFinder();
            var hudManager = hudManagerFinder.GetHUDManager();
            var customRpcSurrogateNetworkBehaviour = hudManager.GetComponent<CustomRpcSurrogateNetworkBehaviour>();
            if (customRpcSurrogateNetworkBehaviour == null)
            {
                throw new CustomRpcSurrogateNetworkBehaviourFinderException(
                    "CustomRpcSurrogateNetworkBehaviour component not found on HUDManager instance."
                );
            }

            cachedCustomRpcSurrogateNetworkBehaviour = customRpcSurrogateNetworkBehaviour;

            return customRpcSurrogateNetworkBehaviour;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting CustomRpcSurrogateNetworkBehaviour: {error}");
            throw new CustomRpcSurrogateNetworkBehaviourFinderException(
                $"Exception while getting CustomRpcSurrogateNetworkBehaviour: {error}"
            );
        }
    }
}
