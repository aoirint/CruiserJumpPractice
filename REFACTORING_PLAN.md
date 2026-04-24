# CruiserJumpPractice Refactoring Plan

## Scope and Constraints

- Project structure remains a single `.csproj` (`CruiserJumpPractice/CruiserJumpPractice.csproj`).
- Naming is fixed to PascalCase and English only.
- `Client` / `Server` vocabulary is retained (no forced rename to `Local` / `Host`).
- Directory structure is fixed: no folder moves, no folder renames, no file relocation across directories.
- `CruiserJumpPractice.cs` and `InputActions.cs` stay at current locations.

## Refactoring Goals

1. Keep current directory layout intact.
2. Improve naming consistency within existing directories.
3. Establish version-aware GameInterop naming for Lethal Company V73 and future versions.
4. Keep runtime behavior stable while refactoring symbols only.

## Directory Granularity Policy

- `UseCases` classification is intentionally retained as a semantic boundary for application actions.
- Existing folder granularity is preserved as-is.
- No new top-level grouping (for example `Features` or `Interop`) is introduced.
- Refinement is done via type/file naming and namespace consistency, not structural migration.

## Target Organization (Fixed Current Structure)

```text
CruiserJumpPractice/
  CruiserJumpPractice.cs
  InputActions.cs
  Application/
    ClientOperationResults.cs
    CruiserStateOperationResults.cs
    Services/
      ServerCruiserStateService.cs
    UseCases/
      SaveCruiserStateUseCase.cs
      LoadCruiserStateUseCase.cs
      RequestSaveCruiserStateUseCase.cs
      RequestLoadCruiserStateUseCase.cs
      ToggleMagnetUseCase.cs
  Composition/
    CompositionRoot.cs
  Domain/
    CruiserStateStore.cs
  GameInterop/
    IGameInterop.cs
    CurrentGameInterop.cs
    CruiserSnapshot.cs
    GameInteropException.cs
    Features/
      CruiserInterop.cs
      GameObjectInterop.cs
      HudInterop.cs
      NetworkInterop.cs
      PlayerInterop.cs
      RpcSurrogateInterop.cs
      ShipMagnetInterop.cs
  NetworkBehaviours/
    RpcSurrogateNetworkBehaviour.cs
  Presentation/
    FrameHandler.cs
    StartupHandler.cs
    ClientCruiserStateService.cs
    ClientMagnetService.cs
    ClientNotificationService.cs
    ClientCruiserResultPresenter.cs
  Patches/
    HudManagerPatch.cs
```

Notes:
- `UseCases` is preserved in `Application/UseCases`.
- `NetworkBehaviours` folder name is preserved (no folder rename in this plan).
- Version-aware naming is applied without moving files to new directories.

## Naming Rules (Authoritative)

### Naming Principle

- Keep directory locations unchanged.
- Rename only type/file symbols that improve clarity or version awareness.
- File name equals type name exactly.

### Role Suffix Policy

- Keep existing role suffixes and normalize only when inconsistent.
- Add suffix only when disambiguation is required:
  - `UseCase` for application action classes.
  - `Service` for orchestration and shared feature utilities.
  - `Handler` for frame/startup lifecycle entry points.
  - `Presenter` for user-facing message composition.
  - `Patch` only for Harmony patch types.

### Planned Renames (Core Examples)

- `CruiserJumpPractice` -> `CruiserJumpPractice` (kept)
- `InputActions` -> `InputActions` (kept)
- `SaveCruiserStateUseCase` -> `SaveCruiserStateUseCase` (kept)
- `LoadCruiserStateUseCase` -> `LoadCruiserStateUseCase` (kept)
- `RequestSaveCruiserStateUseCase` -> `RequestSaveCruiserStateUseCase` (kept)
- `RequestLoadCruiserStateUseCase` -> `RequestLoadCruiserStateUseCase` (kept)
- `ServerCruiserStateService` -> `ServerCruiserStateService` (kept)
- `ClientCruiserStateService` -> `ClientCruiserStateService` (kept)
- `ClientMagnetService` -> `ClientMagnetService` (kept)
- `FrameHandler` -> `FrameInputHandler`
- `CurrentGameInterop` -> `GameInteropV73`
- `CruiserInterop` -> `CruiserInteropV73`
- `GameObjectInterop` -> `GameObjectInteropV73`
- `HudInterop` -> `HudInteropV73`
- `NetworkInterop` -> `NetworkInteropV73`
- `PlayerInterop` -> `PlayerInteropV73`
- `RpcSurrogateInterop` -> `RpcSurrogateInteropV73`
- `ShipMagnetInterop` -> `ShipMagnetInteropV73`

## GameInterop Versioning Convention

### Why

- Current implementation is tied to Lethal Company V73 internals.
- Future upgrades should coexist during migration, not force in-place rewrite.

### Rules

- Versioned interop class naming: `<Concern>InteropV<MajorVersion>`.
  - Example: `HudInteropV73`.
- Versioned aggregate interop naming: `GameInteropV<MajorVersion>`.
  - Example: `GameInteropV73`.
- Contracts remain unversioned in `GameInterop`.
  - `IGameInterop` should describe stable capabilities.
- Composition decides active version.
  - `CompositionRoot` maps `IGameInterop` to `GameInteropV73` for now.
- Future version introduction pattern:
  1. Add `*InteropVXX` classes in the same existing directories.
  2. Implement `GameInteropVXX` and dependent interops.
  3. Switch composition binding.
  4. Remove older version after validation window.

## Namespace Rules

- Namespace must mirror physical path from `CruiserJumpPractice/`.
- For versioned interop in current structure:
  - `CruiserJumpPractice.GameInterop`
  - `CruiserJumpPractice.GameInterop.Features`

## Migration Phases

## Phase 1: Baseline Naming Cleanup (No behavior change)

1. Keep all files in current directories.
2. Apply low-risk naming cleanups (for example `FrameHandler` -> `FrameInputHandler`).
3. Update namespaces and using directives only when symbol rename requires it.
4. Build and verify.

Exit Criteria:
- `dotnet build` succeeds.
- No runtime logic edits.

## Phase 2: Interop Versioning Naming (In-place)

1. Rename `CurrentGameInterop` and feature interops to `V73`-explicit names.
2. Keep files in current `GameInterop` and `GameInterop/Features` directories.
3. Keep behavior unchanged.
4. Build and verify.

Exit Criteria:
- Version-aware names are applied without any directory migration.

## Phase 3: Composition and Runtime Validation

1. Keep `CompositionRoot` as single binding point.
2. Verify startup, frame input, save/load, and magnet toggle flows.
3. Update README architecture map and naming rules.
4. Build and verify.

Exit Criteria:
- Debug startup works.
- Save/load/toggle paths still function.
- Documentation reflects real structure and naming with unchanged directories.

## Risk Controls

- Do not mix behavior refactor with folder/symbol refactor in the same commit.
- Keep commits phase-scoped.
- Run build after each phase.
- Do not perform directory moves or folder renames.
- Preserve `Client` / `Server` terms only where they represent runtime role, not historical naming.

## Suggested Commit Strategy

1. `refactor(naming): align symbol names in current structure`
2. `refactor(interop): introduce v73-based interop naming in place`
3. `refactor(composition): rebind to versioned interop implementation`
4. `docs: update README architecture and naming rules`

## Task Checklist

- [ ] Existing directory layout preserved
- [ ] Phase 1 build passes
- [ ] Phase 2 interop naming updates completed
- [ ] Phase 2 build passes
- [ ] Phase 3 runtime validation completed
- [ ] Phase 3 build passes
- [ ] README updated

## Decision Log

- 2026-04-25: Keep single project architecture.
- 2026-04-25: Enforce PascalCase + English-only naming.
- 2026-04-25: Preserve `Client` / `Server` terminology.
- 2026-04-25: Keep existing directory structure unchanged.
- 2026-04-25: Adopt V73-explicit naming for current interop implementation.
