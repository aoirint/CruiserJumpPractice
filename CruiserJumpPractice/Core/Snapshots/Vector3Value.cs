// SPDX-License-Identifier: Unlicense
#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

// Position and rotation values cross the Core boundary without UnityEngine.Vector3 so snapshot
// data remains plain and Unity conversion stays in adapters.
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
