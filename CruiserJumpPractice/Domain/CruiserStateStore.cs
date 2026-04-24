#nullable enable

using CruiserJumpPractice.Interop.Domain;

namespace CruiserJumpPractice.Domain;

internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}