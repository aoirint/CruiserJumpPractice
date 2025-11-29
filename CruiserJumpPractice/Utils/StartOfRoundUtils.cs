#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.Utils;

internal static class StartOfRoundUtils
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
}
