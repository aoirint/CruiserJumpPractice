#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.Utils;

internal static class MagnetUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal static bool? IsMagnetOn()
    {
        var startOfRound = StartOfRoundUtils.GetStartOfRound();
        if (startOfRound == null)
        {
            Logger.LogError("StartOfRound is null.");
            return null;
        }

        return startOfRound.magnetOn;
    }

    internal static void ToggleMagnet()
    {
        var startOfRound = StartOfRoundUtils.GetStartOfRound();
        if (startOfRound == null)
        {
            Logger.LogError("StartOfRound is null.");
            return;
        }

        var magnetLever = startOfRound.magnetLever;
        if (magnetLever == null)
        {
            Logger.LogError("StartOfRound.magnetLever is null.");
            return;
        }

        var localPlayer = PlayerUtils.GetLocalPlayer();
        if (localPlayer == null)
        {
            Logger.LogError("Local player is null.");
            return;
        }

        // NOTE: This AnimatedObjectTrigger method calls StartOfRound.SetMagnetOn and sends a ServerRpc internally.
        magnetLever.TriggerAnimation(localPlayer);
    }
}
