// SPDX-License-Identifier: Unlicense
#nullable enable

extern alias LethalCompany;
extern alias LethalCompanyInputUtils;

using LethalCompany::UnityEngine.InputSystem;
using LethalCompanyInputUtils::LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils::LethalCompanyInputUtils.BindingPathEnums;

namespace CruiserJumpPractice.Interop.InputUtils;

// InputUtilsActions declares the keybindings that InputUtils registers for the plugin.
// Practice-facing input behavior is adapted in InputUtilsPracticeInput, so this file stays as a
// small attribute table with layout notes beside each binding.
// The repeated "Keymap:" rows are intentional table data, not prose that needs varied wording.
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
