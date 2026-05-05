// SPDX-License-Identifier: Unlicense
#nullable enable

using CruiserJumpPractice.Core.Snapshots;

namespace CruiserJumpPractice.Core.Ports;

internal interface IValidationLogger
{
    void Record(string eventName, params ValidationLogField[] fields);
}

internal readonly struct ValidationLogField
{
    private ValidationLogField(
        string name,
        ValidationLogFieldKind kind,
        string? stringValue,
        bool boolValue,
        int intValue,
        float floatValue,
        Vector3Value vectorValue,
        int decimalPlaces
    )
    {
        Name = name;
        Kind = kind;
        StringValue = stringValue;
        BoolValue = boolValue;
        IntValue = intValue;
        FloatValue = floatValue;
        VectorValue = vectorValue;
        DecimalPlaces = decimalPlaces;
    }

    public string Name { get; }

    internal ValidationLogFieldKind Kind { get; }

    internal string? StringValue { get; }

    internal bool BoolValue { get; }

    internal int IntValue { get; }

    internal float FloatValue { get; }

    internal Vector3Value VectorValue { get; }

    internal int DecimalPlaces { get; }

    public static ValidationLogField String(string name, string value)
    {
        return new ValidationLogField(
            name,
            ValidationLogFieldKind.String,
            value,
            boolValue: false,
            intValue: 0,
            floatValue: 0,
            vectorValue: default,
            decimalPlaces: 0
        );
    }

    public static ValidationLogField Bool(string name, bool value)
    {
        return new ValidationLogField(
            name,
            ValidationLogFieldKind.Bool,
            stringValue: null,
            value,
            intValue: 0,
            floatValue: 0,
            vectorValue: default,
            decimalPlaces: 0
        );
    }

    public static ValidationLogField Int(string name, int value)
    {
        return new ValidationLogField(
            name,
            ValidationLogFieldKind.Int,
            stringValue: null,
            boolValue: false,
            value,
            floatValue: 0,
            vectorValue: default,
            decimalPlaces: 0
        );
    }

    public static ValidationLogField Number(string name, float value, int decimalPlaces)
    {
        return new ValidationLogField(
            name,
            ValidationLogFieldKind.Number,
            stringValue: null,
            boolValue: false,
            intValue: 0,
            value,
            vectorValue: default,
            decimalPlaces
        );
    }

    public static ValidationLogField Vector3(string name, Vector3Value value, int decimalPlaces)
    {
        return new ValidationLogField(
            name,
            ValidationLogFieldKind.Vector3,
            stringValue: null,
            boolValue: false,
            intValue: 0,
            floatValue: 0,
            value,
            decimalPlaces
        );
    }
}

internal enum ValidationLogFieldKind
{
    String,
    Bool,
    Int,
    Number,
    Vector3
}
