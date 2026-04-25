#nullable enable

extern alias LethalCompany73;
extern alias LethalCompanyInputUtils73;

using LethalCompany73::UnityEngine.InputSystem;
using LethalCompanyInputUtils73::LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils73::LethalCompanyInputUtils.BindingPathEnums;

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
