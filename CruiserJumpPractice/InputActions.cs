#nullable enable

using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace CruiserJumpPractice;

class InputActions : LcInputActions
{
    [InputAction(KeyboardControl.Backslash, Name = "Save Cruiser")]
    public InputAction? SaveCruiserKey { get; set; }

    [InputAction(KeyboardControl.LeftBracket, Name = "Load Cruiser")]
    public InputAction? LoadCruiserKey { get; set; }
}
