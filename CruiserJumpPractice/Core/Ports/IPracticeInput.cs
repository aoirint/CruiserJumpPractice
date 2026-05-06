// SPDX-License-Identifier: MIT
#nullable enable

namespace CruiserJumpPractice.Core.Ports;

/// <summary>
/// Provides one-frame practice input intentions to Core.
/// </summary>
/// <remarks>
/// The keybinding package, key names, and nullability quirks stay outside the
/// frame handler.
/// </remarks>
internal interface IPracticeInput
{
    bool SaveCruiserTriggered { get; }

    bool LoadCruiserTriggered { get; }

    bool ToggleMagnetTriggered { get; }
}
