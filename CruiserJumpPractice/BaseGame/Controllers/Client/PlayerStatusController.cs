#nullable enable

using BepInEx.Logging;
using GameNetcodeStuff;

namespace CruiserJumpPractice.BaseGame.Controllers.Client;

class PlayerStatusControllerException : System.Exception
{
    public PlayerStatusControllerException(string message) : base(message) { }
}

class PlayerStatusController
{
    protected static ManualLogSource Logger => CruiserJumpPractice.Logger!;

    protected PlayerControllerB player;

    public PlayerStatusController(PlayerControllerB player)
    {
        this.player = player;
    }

    public bool IsPlayerBusy()
    {
        return IsPlayerMenuOpen() || IsPlayerInTerminalMenu() || IsPlayerTypingChat();
    }

    public bool IsPlayerInTerminalMenu()
    {
        try
        {
            return player.inTerminalMenu;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'inTerminalMenu': {error}");
            throw new PlayerStatusControllerException($"Exception while getting 'inTerminalMenu': {error}");
        }
    }

    public bool IsPlayerTypingChat()
    {
        try
        {
            return player.isTypingChat;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'isTypingChat': {error}");
            throw new PlayerStatusControllerException($"Exception while getting 'isTypingChat': {error}");
        }
    }

    public bool IsPlayerMenuOpen()
    {
        try
        {
            var quickMenuManager = player.quickMenuManager;
            if (quickMenuManager == null)
            {
                throw new PlayerStatusControllerException("quickMenuManager is null.");
            }

            return quickMenuManager.isMenuOpen;
        }
        catch (System.Exception error)
        {
            Logger.LogError($"Exception while getting 'isMenuOpen': {error}");
            throw new PlayerStatusControllerException($"Exception while getting 'isMenuOpen': {error}");
        }
    }
}
