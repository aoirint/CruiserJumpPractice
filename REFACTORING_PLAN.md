# CruiserJumpPractice Refactoring Plan

## Scope and Constraints

- Project structure remains a single `.csproj` (`CruiserJumpPractice/CruiserJumpPractice.csproj`).
- Naming is fixed to PascalCase and English only.
- `Client` / `Server` vocabulary is retained (no forced rename to `Local` / `Host`).
- This plan prioritizes structure-first renaming, then namespace alignment, then wiring clarity.
- `CruiserJumpPractice.cs` and `InputActions.cs` are fixed and excluded from rename scope.
- The section `## Target Organization (Simplified, Single Project)` is authoritative. Other sections must conform to it.

## Refactoring Goals

1. Make the directory layout the source of truth for naming.
2. Remove over-fragmented folders that increase navigation cost.
3. Establish version-aware GameInterop naming for Lethal Company V73 and future versions.
4. Keep runtime behavior stable while refactoring structure and symbols.

## Directory Granularity Policy

- `UseCases` classification is intentionally retained as a semantic boundary for application actions.
- New policy: split only when a folder contains 4 or more files or clearly different lifecycle concerns.
- Default is feature-level grouping with shallow layers.
- Exception rule: even with fewer than 4 files, independent folders are allowed for lifecycle boundaries (`Runtime`, `Network`, `Patches`, `Notifications`).

## Target Organization

```text
CruiserJumpPractice/
  CruiserJumpPractice.cs
  InputActions.cs
  Domain/
    CruiserStateStore.cs
    CruiserStateResults.cs
    MagnetResults.cs
  Runtime/
    FrameHandler.cs
    StartupHandler.cs
  Services/
    ClientNotificationService.cs
    ClientMagnetService.cs
    ClientCruiserStateService.cs
    ServerCruiserStateService.cs
    ServiceComposition.cs
  UseCases/
    SaveCruiserStateUseCase.cs
    LoadCruiserStateUseCase.cs
    RequestSaveCruiserStateUseCase.cs
    RequestLoadCruiserStateUseCase.cs
    ToggleMagnetUseCase.cs
  Utilities/
    CruiserStateTipUtility.cs
  Interop/
    IGameInterop.cs
    GameInteropV73.cs
    Adapters/
      V73/
        CruiserAdapterV73.cs
        GameObjectAdapterV73.cs
        HudAdapterV73.cs
        NetworkAdapterV73.cs
        PlayerAdapterV73.cs
        RpcSurrogateAdapterV73.cs
        ShipMagnetAdapterV73.cs
    Behaviours/
      RpcSurrogateBehaviour.cs
    Domain/
      CruiserSnapshot.cs
      GameInteropException.cs
    Patches/
      HudManagerPatch.cs
```

Notes:
- Folders are shallow by default.
- `Features` is product-facing and stable in meaning.
- `UseCases` folders are preserved in feature scope.
- Technical version-specific implementation is isolated under `Interop/Adapters/V73`.

## Structure-First Rename Rules (Authoritative)

### Naming Principle

- Do not inherit legacy names when they no longer match target folder responsibility.
- Rename from directory intent first, then adjust dependencies.
- File name equals type name exactly.
- If this section conflicts with Target Organization, Target Organization wins.

### Role Suffix Policy

- Keep `UseCase` suffix for application action classes.
- Use `Service` for orchestration and shared feature utilities.
- Use `Handler` for frame/startup lifecycle entry points.
- Use `Presenter` for user-facing message composition.
- Use `Patch` only for Harmony patch types.
- Use `AdapterV<MajorVersion>` only for version-specific interop adapters.

### Planned Renames (Bounded by Phase)

Phase 2 (non-Interop renames only):
- `FrameHandler` -> `FrameInputHandler`

Phase 3 (Interop V73 renames only):
- `CurrentGameInterop` -> `GameInteropV73`
- `CruiserInterop` -> `CruiserAdapterV73`
- `GameObjectInterop` -> `GameObjectAdapterV73`
- `HudInterop` -> `HudAdapterV73`
- `NetworkInterop` -> `NetworkAdapterV73`
- `PlayerInterop` -> `PlayerAdapterV73`
- `RpcSurrogateInterop` -> `RpcSurrogateAdapterV73`
- `ShipMagnetInterop` -> `ShipMagnetAdapterV73`

## GameInterop Versioning Convention

### Why

- Current implementation is tied to Lethal Company V73 internals.
- Future upgrades should coexist during migration, not force in-place rewrite.

### Rules

- Versioned adapter class naming: `<Concern>AdapterV<MajorVersion>`.
  - Example: `HudAdapterV73`.
- Versioned aggregate interop naming: `GameInteropV<MajorVersion>`.
  - Example: `GameInteropV73`.
- Contracts remain unversioned in `Interop/Contracts`.
  - `IGameInterop` should describe stable capabilities.
- Composition decides active version.
  - `CompositionRoot` maps `IGameInterop` to `GameInteropV73` for now.
- Future version introduction pattern:
  1. Add `Interop/Adapters/VXX`.
  2. Implement `GameInteropVXX` and adapters.
  3. Switch composition binding.
  4. Remove older version after validation window.

## Namespace Rules

- Namespace must mirror physical path from `CruiserJumpPractice/`.
- For versioned interop:
  - `CruiserJumpPractice.Interop.Adapters.V73`
- Namespace updates are allowed in the same phase as file moves when needed for compile consistency.
- No generic technical bucket names like `Features` inside interop.

## Migration Phases

## Phase 1: Simplify Folders and Move Files (No type renames)

1. Create target folders exactly as defined in Target Organization.
2. Move files to target folders.
3. Update namespaces/usings only as required to keep the build passing after moves.
4. Do not rename types in this phase.
5. Build and verify.

Exit Criteria:
- `dotnet build` succeeds.
- File placement matches Target Organization.
- No type renames are included.

## Phase 2: Structure-First Renaming (Non-Interop only)

1. Apply only non-Interop renames from Planned Renames.
2. Update namespaces/usings and references.
3. Keep behavior unchanged.
4. Build and verify.

Exit Criteria:
- Non-Interop renamed symbols align with folder responsibility.
- Interop legacy names are still untouched.
- `dotnet build` succeeds.

## Phase 3: Interop Versioning Foundation

1. Apply all Interop V73 renames from Planned Renames.
2. Ensure all V73-specific implementations are under `Interop/Adapters/V73`.
3. Keep `IGameInterop` contract stable.
4. Keep behavior unchanged.
5. Build and verify.

Exit Criteria:
- Version-specific code is physically isolated.
- Contract remains version-agnostic.
- `dotnet build` succeeds.

## Phase 4: Composition and Runtime Validation

1. Keep `CompositionRoot` as single binding point.
2. Confirm `IGameInterop` binding targets `GameInteropV73`.
3. Run runtime smoke checks.
4. Update README architecture map.

Runtime Smoke Checks:
- Startup path works in debug profile.
- Frame input handling triggers expected flow.
- Save Cruiser State path works end-to-end.
- Load Cruiser State path works end-to-end.
- Magnet toggle path works end-to-end.
- Notifications and HUD-related messaging still appear as expected.

Exit Criteria:
- Startup works.
- Save/load/toggle paths function.
- Composition binding is correct.
- Documentation reflects actual structure and naming.

## Risk Controls

- Do not mix behavior refactor with folder/symbol refactor in the same commit.
- Keep commits phase-scoped.
- Run build after each phase.
- Preserve `Client` / `Server` terms only where they represent runtime role.
- If a phase introduces behavior changes, stop and split that change into a separate follow-up commit.

## Suggested Commit Strategy

1. `refactor(structure): align folders with target organization`
2. `refactor(naming): apply non-interop structure-first renames`
3. `refactor(interop): apply v73 interop naming and placement`
4. `refactor(composition): bind interop contract to gameinteropv73`
5. `docs: update readme architecture and validation notes`

## Task Checklist

- [ ] Phase 1 folder simplification completed
- [ ] Phase 1 build passes
- [ ] Phase 2 non-Interop renaming completed
- [ ] Phase 2 build passes
- [ ] Phase 3 Interop V73 renaming completed
- [ ] Phase 3 build passes
- [ ] Phase 4 runtime smoke checks completed
- [ ] README updated

## Decision Log

- 2026-04-25: Keep single project architecture.
- 2026-04-25: Enforce PascalCase + English-only naming.
- 2026-04-25: Preserve `Client` / `Server` terminology.
- 2026-04-25: Prioritize structure-first renaming over legacy name inheritance.
- 2026-04-25: Adopt V73-explicit naming for current interop implementation.
- 2026-04-25: Treat Target Organization as the single source of truth for all other sections.
