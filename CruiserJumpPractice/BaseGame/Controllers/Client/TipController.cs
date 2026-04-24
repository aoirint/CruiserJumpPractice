#nullable enable

using BepInEx.Logging;

namespace CruiserJumpPractice.BaseGame.Controllers.Client;

class TipControllerException : System.Exception
{
    public TipControllerException(string message) : base(message) { }
}

class TipController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected HUDManager hudManager;

    public TipController(HUDManager hudManager)
    {
        this.hudManager = hudManager;
    }

    public void DisplayTip(string headerText, string bodyText)
    {
        try
        {
            hudManager.DisplayTip(headerText, bodyText);
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while displaying tip: {error}");
            throw new TipControllerException($"Exception while displaying tip: {error}");
        }
    }
}
