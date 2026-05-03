#nullable enable

using CruiserJumpPractice.Core.Ports;

namespace CruiserJumpPractice.Composition;

internal sealed class InputActionsPracticeInput : IPracticeInput
{
    private readonly InputActions inputActions;

    public InputActionsPracticeInput(InputActions inputActions)
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
