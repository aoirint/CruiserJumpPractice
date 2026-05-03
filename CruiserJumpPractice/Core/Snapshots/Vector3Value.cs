#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

// Core uses its own vector shape to avoid depending on UnityEngine.Vector3. That keeps snapshot
// data portable across tests and makes Unity conversion an Interop concern.
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
