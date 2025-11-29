#nullable enable

using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace CruiserJumpPractice;

class InputActions : LcInputActions
{
    // NOTE: JP109 @, US [
    [InputAction(KeyboardControl.LeftBracket, Name = "Load Cruiser")]
    public InputAction? LoadCruiserKey { get; set; }

    // NOTE: JP109 [, US ]
    [InputAction(KeyboardControl.RightBracket, Name = "Save Cruiser")]
    public InputAction? SaveCruiserKey { get; set; }

    // NOTE: JP109 ], US \
    [InputAction(KeyboardControl.Backslash, Name = "Toggle Magnet")]
    public InputAction? ToggleMagnetKey { get; set; }
}
