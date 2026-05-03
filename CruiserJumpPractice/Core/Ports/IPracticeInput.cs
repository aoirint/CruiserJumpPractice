#nullable enable

namespace CruiserJumpPractice.Core.Ports;

internal interface IPracticeInput
{
    bool SaveCruiserTriggered { get; }

    bool LoadCruiserTriggered { get; }

    bool ToggleMagnetTriggered { get; }
}
