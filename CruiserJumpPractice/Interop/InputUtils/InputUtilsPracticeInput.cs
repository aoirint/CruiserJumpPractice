#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Interop.InputUtils;

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
