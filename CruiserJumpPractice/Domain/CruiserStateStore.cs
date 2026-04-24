#nullable enable

namespace CruiserJumpPractice.Domain;

internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}