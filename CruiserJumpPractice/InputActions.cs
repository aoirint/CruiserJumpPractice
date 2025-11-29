#nullable enable

using LethalCompanyInputUtils.Api;
using LethalCompanyInputUtils.BindingPathEnums;
using UnityEngine.InputSystem;

namespace CruiserJumpPractice;

class InputActions : LcInputActions
{
    [InputAction(KeyboardControl.LeftBracket, Name = "Save Cruiser")]
    public InputAction? SaveCruiserKey { get; set; }

    [InputAction(KeyboardControl.RightBracket, Name = "Load Cruiser")]
    public InputAction? LoadCruiserKey { get; set; }
}
