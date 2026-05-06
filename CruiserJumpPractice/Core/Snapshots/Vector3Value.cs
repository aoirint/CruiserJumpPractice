// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

/// <summary>
/// Plain vector value used for positions and rotations crossing the Core boundary.
/// </summary>
/// <remarks>
/// Snapshot data remains free of UnityEngine.Vector3; Unity conversion stays in adapters.
/// </remarks>
internal readonly struct Vector3Value
{
    public float X { get; }

    public float Y { get; }

    public float Z { get; }

    public Vector3Value(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
