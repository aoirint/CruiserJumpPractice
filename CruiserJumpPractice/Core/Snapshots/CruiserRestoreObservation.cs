// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

/// <summary>
/// Numeric before/after observation captured while applying a cruiser restore.
/// </summary>
/// <remarks>
/// Restore validation can use this data without exposing Unity objects or
/// environment-specific identifiers.
/// </remarks>
internal sealed class CruiserRestoreObservation
{
    public Vector3Value SavedCarPosition { get; }

    public Vector3Value SavedCarRotation { get; }

    public Vector3Value BeforeCarPosition { get; }

    public Vector3Value AfterCarPosition { get; }

    public int SavedCarHP { get; }

    public int BeforeCarHP { get; }

    public int AfterCarHP { get; }

    public int SavedTurboBoosts { get; }

    public int BeforeTurboBoosts { get; }

    public int AfterTurboBoosts { get; }

    public CruiserRestoreObservation(
        Vector3Value savedCarPosition,
        Vector3Value savedCarRotation,
        Vector3Value beforeCarPosition,
        Vector3Value afterCarPosition,
        int savedCarHP,
        int beforeCarHP,
        int afterCarHP,
        int savedTurboBoosts,
        int beforeTurboBoosts,
        int afterTurboBoosts
    )
    {
        SavedCarPosition = savedCarPosition;
        SavedCarRotation = savedCarRotation;
        BeforeCarPosition = beforeCarPosition;
        AfterCarPosition = afterCarPosition;
        SavedCarHP = savedCarHP;
        BeforeCarHP = beforeCarHP;
        AfterCarHP = afterCarHP;
        SavedTurboBoosts = savedTurboBoosts;
        BeforeTurboBoosts = beforeTurboBoosts;
        AfterTurboBoosts = afterTurboBoosts;
    }
}
