#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.State;

internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}
