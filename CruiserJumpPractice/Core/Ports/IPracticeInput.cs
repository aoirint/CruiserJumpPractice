// SPDX-License-Identifier: Unlicense
#nullable enable

namespace CruiserJumpPractice.Core.Ports;

// Core consumes input as one-frame practice intentions. The keybinding package, key names, and
// nullability quirks stay outside the frame handler.
internal interface IPracticeInput
{
    bool SaveCruiserTriggered { get; }

    bool LoadCruiserTriggered { get; }

    bool ToggleMagnetTriggered { get; }
}
