#nullable enable

extern alias LethalCompany;
extern alias LethalCompanyInputUtils;

using LethalCompany::UnityEngine.InputSystem;
using LethalCompanyInputUtils::LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils::LethalCompanyInputUtils.BindingPathEnums;

namespace CruiserJumpPractice.Interop.InputUtils;

// Keep the layout notes beside the attributes because these bindings are easier to audit as a table.
internal sealed class InputUtilsActions : LcInputActions
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
