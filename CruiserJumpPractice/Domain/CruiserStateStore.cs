#nullable enable

using CruiserJumpPractice.GameInterop;

namespace CruiserJumpPractice.Domain;

internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}