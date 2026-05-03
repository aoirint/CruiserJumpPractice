#nullable enable

namespace CruiserJumpPractice.Interop;

internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
