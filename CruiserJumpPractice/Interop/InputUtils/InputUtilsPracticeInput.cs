#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop.InputUtils;

// InputUtils-specific action objects are translated into Core's practice input port here.
// Null actions are treated as not-triggered so Core does not need to handle partially initialized
// keybinding state during plugin startup or InputUtils reloads.
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
