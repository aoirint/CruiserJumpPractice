#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.Utils;

internal static class MagnetUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal static StartOfRound? GetStartOfRound()
    {
        var startOfRound = StartOfRound.Instance;
        if (startOfRound == null)
        {
            Logger.LogError("StartOfRound.Instance is null.");
            return null;
        }

        return startOfRound;
    }

    internal static bool? IsMagnetOn()
    {
        var startOfRound = GetStartOfRound();
        if (startOfRound == null)
        {
            Logger.LogError("StartOfRound is null.");
            return null;
        }

        return startOfRound.magnetOn;
    }

    internal static void SetMagnet(bool on)
    {
        var startOfRound = GetStartOfRound();
        if (startOfRound == null)
        {
            Logger.LogError("StartOfRound is null.");
            return;
        }

        // NOTE: This method sends a ServerRpc internally.
        startOfRound.SetMagnetOn(on);
    }
}
