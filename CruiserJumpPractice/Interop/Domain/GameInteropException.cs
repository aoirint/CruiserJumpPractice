#nullable enable

namespace CruiserJumpPractice.Interop.Domain;

internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
