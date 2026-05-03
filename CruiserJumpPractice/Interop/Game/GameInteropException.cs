#nullable enable

namespace CruiserJumpPractice.Interop.Game;

internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
