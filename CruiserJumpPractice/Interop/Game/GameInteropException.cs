#nullable enable

namespace CruiserJumpPractice.Interop.Game;

/// <summary>
/// Interop-specific exception for unexpected game access failures.
/// </summary>
/// <remarks>
/// Use cases can catch game access problems through IGameInterop without taking
/// a dependency on reflection or Unity errors.
/// </remarks>
internal sealed class GameInteropException : System.Exception
{
    public GameInteropException(string message) : base(message)
    {
    }
}
