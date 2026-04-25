#nullable enable

namespace CruiserJumpPractice.Domain;

internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
