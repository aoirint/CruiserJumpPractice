# HUD Feedback

## Target

- Game: Lethal Company v81
- Steam manifest ID: `6423525044216269478`

## Patch and access targets

### `HUDManager`

| Member | Declaration | Role |
| --- | --- | --- |
| Singleton | `public static HUDManager Instance { get; private set; }` | Resolve the active local HUD manager. |
| Setup | `private void Awake()` | Postfix boundary after base HUD initialization. |
| Frame update | `private void Update()` | Postfix boundary for frame-driven mod input handling. |
| Tip API | `public void DisplayTip(string headerText, string bodyText, bool isWarning = false, bool useSave = false, string prefsKey = "LC_Tip1")` | Shows a game-styled, transient result message. |

## Lifecycle and presentation

`HUDManager.Instance` is assigned during the manager's base `Awake()` path.
A callback that needs the singleton must therefore run after that path, and an
interop adapter must still treat a null instance as unavailable rather than as
a permanent missing HUD.

`DisplayTip` owns its own display policy, including warning and saved-tip
options. Calling it with only the header and body uses the base defaults:
`isWarning = false`, `useSave = false`, and `prefsKey = "LC_Tip1"`.

## Implementation choices

### Choose a startup hook

#### Patch `HUDManager.Awake()` with a postfix — recommended

The postfix runs after the base manager has initialized the singleton and HUD
state. It is the earliest game-defined point for startup work that depends on
HUD availability.

#### Run HUD-dependent startup from plugin initialization

Plugin initialization can occur before the game's HUD manager exists, so it
does not establish a usable `HUDManager.Instance`.

### Choose a frame hook

#### Patch `HUDManager.Update()` with a postfix — recommended

This follows the active HUD's frame lifecycle and provides a game-timed point
for input-driven presentation work.

#### Use a detached `MonoBehaviour.Update()` or an arbitrary coroutine

Those can be useful for independent work, but they do not establish that the
HUD manager for the active scene has completed its own update.

### Present a command result

#### Call `HUDManager.DisplayTip(header, body)` — recommended

The method uses the game's existing tip surface and defaults, so a short
success or rejection message needs no mod-owned canvas, text component, or
teardown lifecycle.

#### Mutate HUD children directly

This couples the mod to base HUD object layout and creates ownership and
cleanup responsibilities that a transient command result does not need.

### Handle an unavailable HUD

#### Treat the current HUD as unavailable

A null singleton means the game object needed for presentation is unavailable
at that moment. The caller must not claim that a tip was displayed. Whether to
fail, retry, or queue work is a mod-specific architecture decision.

#### Cache an older `HUDManager` instance or silently discard the result

An older instance can belong to a replaced HUD. Silent loss makes a command
appear to have completed without user feedback.

## Change checklist

1. Patch the exact parameterless `Awake()` and `Update()` methods with
   postfices when using them as lifecycle hooks.
2. Resolve `HUDManager.Instance` at the point of use; do not retain a stale
   manager across a HUD replacement.
3. Use `DisplayTip` for transient feedback and keep its default arguments
   explicit in the documentation when a different policy is required.
4. Keep the HUD-manager dependency at the game-integration boundary rather than
   passing a HUD object through unrelated application logic.
