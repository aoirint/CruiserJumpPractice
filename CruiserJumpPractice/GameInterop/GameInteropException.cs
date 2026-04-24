#nullable enable

namespace CruiserJumpPractice.GameInterop;

internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
