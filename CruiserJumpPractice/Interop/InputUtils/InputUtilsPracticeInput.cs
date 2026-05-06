// SPDX-License-Identifier: MIT
#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop.InputUtils;

/// <summary>
/// Adapts the InputUtils action declaration table to Core practice input.
/// </summary>
/// <remarks>
/// Missing InputAction objects are treated as not triggered, so frame handling
/// sees only simple one-frame practice commands.
/// </remarks>
internal sealed class InputUtilsPracticeInput : IPracticeInput
{
    private readonly InputUtilsActions inputActions;

    public InputUtilsPracticeInput(InputUtilsActions inputActions)
    {
        this.inputActions = inputActions;
    }

    public bool SaveCruiserTriggered =>
        inputActions.SaveCruiserKey?.triggered ?? false;

    public bool LoadCruiserTriggered =>
        inputActions.LoadCruiserKey?.triggered ?? false;

    public bool ToggleMagnetTriggered =>
        inputActions.ToggleMagnetKey?.triggered ?? false;
}
