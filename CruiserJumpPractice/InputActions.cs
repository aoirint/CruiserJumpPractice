#nullable enable

extern alias LethalCompany;
extern alias LethalCompanyInputUtils;

using LethalCompany::UnityEngine.InputSystem;
using LethalCompanyInputUtils::LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils::LethalCompanyInputUtils.BindingPathEnums;

namespace CruiserJumpPractice;

class InputActions : LcInputActions
{
    // Keymap: JP109 @, US [
    [InputAction(KeyboardControl.LeftBracket, Name = "Load Cruiser")]
    public InputAction? LoadCruiserKey { get; set; }

    // Keymap: JP109 [, US ]
    [InputAction(KeyboardControl.RightBracket, Name = "Save Cruiser")]
    public InputAction? SaveCruiserKey { get; set; }

    // Keymap: JP109 ], US \
    [InputAction(KeyboardControl.Backslash, Name = "Toggle Magnet")]
    public InputAction? ToggleMagnetKey { get; set; }
}
