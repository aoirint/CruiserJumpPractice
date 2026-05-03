#nullable enable

namespace CruiserJumpPractice.Core.Ports;

// Input is represented as one-frame intentions rather than InputUtils actions. This lets the
// frame handler decide practice behavior without knowing which keybinding library produced it.
internal interface IPracticeInput
{
    bool SaveCruiserTriggered { get; }

    bool LoadCruiserTriggered { get; }

    bool ToggleMagnetTriggered { get; }
}
