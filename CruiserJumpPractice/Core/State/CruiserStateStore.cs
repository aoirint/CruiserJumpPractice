// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.State;

/// <summary>
/// Session-scoped store for the latest saved cruiser snapshot.
/// </summary>
/// <remarks>
/// The store keeps only the latest snapshot instead of implying file
/// persistence or multiple named save slots.
/// </remarks>
internal sealed class CruiserStateStore
{
    public CruiserSnapshot? SavedCruiserState { get; set; }
}
