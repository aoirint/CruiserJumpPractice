#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.Utils;

internal static class HUDManagerUtils
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal static HUDManager? GetHUDManager()
    {
        var hudManager = HUDManager.Instance;
        if (hudManager == null)
        {
            Logger.LogError("HUDManager.Instance is null.");
            return null;
        }

        return hudManager;
    }

    internal static void DisplayTip(string headerText, string bodyText)
    {
        var hudManager = GetHUDManager();
        if (hudManager == null)
        {
            Logger.LogError("Cannot display tip because HUDManager is null.");
            return;
        }

        hudManager.DisplayTip(headerText, bodyText);
    }
}
