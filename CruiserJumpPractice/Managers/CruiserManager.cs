#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.Managers;

internal class CruiserManager
{
    internal static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    internal void SaveCruiser()
    {
        Logger.LogInfo("SaveCruiser called");
    }

    internal void LoadCruiser()
    {
        Logger.LogInfo("LoadCruiser called");
    }
}
