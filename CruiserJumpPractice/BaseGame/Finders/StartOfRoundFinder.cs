#nullable enable

using BepInEx.Logging;
using UnityEngine;

namespace CruiserJumpPractice.BaseGame.Finders;

class StartOfRoundFinderException : System.Exception
{
    public StartOfRoundFinderException(string message) : base(message) { }
}

class StartOfRoundFinder
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    public StartOfRound GetStartOfRound()
    {
        try
        {
            var startOfRound = StartOfRound.Instance;
            if (startOfRound == null)
            {
                throw new StartOfRoundFinderException("StartOfRound.Instance is null.");
            }

            return startOfRound;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting StartOfRound: {error}");
            throw new StartOfRoundFinderException($"Exception while getting StartOfRound: {error}");
        }
    }
}
