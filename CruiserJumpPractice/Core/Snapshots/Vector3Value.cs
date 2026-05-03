#nullable enable

namespace CruiserJumpPractice.Core.Snapshots;

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
