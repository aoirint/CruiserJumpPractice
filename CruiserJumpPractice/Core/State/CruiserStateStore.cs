// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.State;

// Practice saves are session-scoped. The store keeps only the latest snapshot instead of implying
// file persistence or multiple named save slots.
internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}
