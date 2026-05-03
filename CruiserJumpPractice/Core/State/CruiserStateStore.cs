#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.State;

// State is intentionally in-memory and plugin-lifetime scoped. Persistence is not part of this
// practice feature; the store only remembers the most recent cruiser snapshot for load requests.
internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}
