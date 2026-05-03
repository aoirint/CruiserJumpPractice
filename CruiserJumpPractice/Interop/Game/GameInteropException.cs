#nullable enable

namespace CruiserJumpPractice.Interop.Game;

// Adapter failures use an Interop-specific exception so use cases can catch unexpected game
// access problems through IGameInterop without taking a dependency on reflection or Unity errors.
internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
