# CruiserJumpPractice Refactoring Plan

## Scope and Constraints

- Project structure remains a single `.csproj` (`CruiserJumpPractice/CruiserJumpPractice.csproj`).
- Naming is fixed to PascalCase and English only.
- `Client` / `Server` vocabulary is retained (no forced rename to `Local` / `Host`).
- This plan prioritizes structure-first renaming, then namespace alignment, then wiring clarity.

## Refactoring Goals

1. Make the directory layout the source of truth for naming.
2. Remove over-fragmented folders that increase navigation cost.
3. Establish version-aware GameInterop naming for Lethal Company V73 and future versions.
4. Keep runtime behavior stable while refactoring structure and symbols.

## Directory Granularity Policy

- Current draft was too fine-grained (`UseCases`, `Results`, `Services`, `Presenters`, `Handlers` under every area).
- New policy: split only when a folder contains 4 or more files or clearly different lifecycle concerns.
- Default is feature-level grouping with shallow layers.

## Target Organization (Simplified, Single Project)

```text
CruiserJumpPractice/
  Bootstrap/
    ModEntryPoint.cs
    InputActions.cs
  Composition/
    CompositionRoot.cs
  Features/
    CruiserState/
      SaveCruiserState.cs
      LoadCruiserState.cs
      RequestSaveCruiserState.cs
      RequestLoadCruiserState.cs
      CruiserStateResults.cs
      ClientCruiserStateCoordinator.cs
      ServerCruiserStateCoordinator.cs
      CruiserStateStore.cs
    Magnet/
      ToggleMagnet.cs
      MagnetResults.cs
      ClientMagnetCoordinator.cs
    Notifications/
      ClientNotificationService.cs
    Runtime/
      FrameInputHandler.cs
      StartupHandler.cs
      ClientCruiserResultPresenter.cs
  Interop/
    Contracts/
      IGameInterop.cs
    Models/
      CruiserSnapshot.cs
    Exceptions/
      GameInteropException.cs
    Adapters/
      V73/
        GameInteropV73.cs
        CruiserAdapterV73.cs
        GameObjectAdapterV73.cs
        HudAdapterV73.cs
        NetworkAdapterV73.cs
        PlayerAdapterV73.cs
        RpcSurrogateAdapterV73.cs
        ShipMagnetAdapterV73.cs
  Network/
    RpcSurrogateNetworkBehaviour.cs
  Patches/
    HudManagerPatch.cs
```

Notes:
- Folders are shallow by default.
- `Features` is product-facing and stable in meaning.
- Technical version-specific implementation is isolated under `Interop/Adapters/V73`.

## Structure-First Rename Rules (Authoritative)

### Naming Principle

- Do not inherit legacy names when they no longer match target folder responsibility.
- Rename from directory intent first, then adjust dependencies.
- File name equals type name exactly.

### Role Suffix Policy

- Use functional names first (`SaveCruiserState`, `LoadCruiserState`, `ToggleMagnet`).
- Add suffix only when disambiguation is required:
  - `Coordinator` for orchestration classes that call multiple operations.
  - `Handler` for frame/startup lifecycle entry points.
  - `Presenter` for user-facing message composition.
  - `Service` only for truly shared cross-feature utilities.
  - `Patch` only for Harmony patch types.

### Planned Renames (Core Examples)

- `CruiserJumpPractice` -> `ModEntryPoint`
- `SaveCruiserStateUseCase` -> `SaveCruiserState`
- `LoadCruiserStateUseCase` -> `LoadCruiserState`
- `RequestSaveCruiserStateUseCase` -> `RequestSaveCruiserState`
- `RequestLoadCruiserStateUseCase` -> `RequestLoadCruiserState`
- `ServerCruiserStateService` -> `ServerCruiserStateCoordinator`
- `ClientCruiserStateService` -> `ClientCruiserStateCoordinator`
- `ClientMagnetService` -> `ClientMagnetCoordinator`
- `FrameHandler` -> `FrameInputHandler`
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
- No generic technical bucket names like `Features` inside interop.

## Migration Phases

## Phase 1: Simplify Folders and Move Files (No behavior change)

1. Create simplified target folders.
2. Move files based on feature ownership.
3. Keep old symbol names temporarily.
4. Build and verify.

Exit Criteria:
- `dotnet build` succeeds.
- No runtime logic edits.

## Phase 2: Structure-First Renaming

1. Apply planned class/file renames.
2. Update namespaces and using directives.
3. Keep behavior unchanged.
4. Build and verify.

Exit Criteria:
- Renamed symbols align with folder responsibility.
- No legacy naming drift remains.

## Phase 3: Interop Versioning Foundation

1. Rename `CurrentGameInterop` family to `V73` series.
2. Move all V73-specific implementations under `Interop/Adapters/V73`.
3. Keep `IGameInterop` contract stable.
4. Build and verify.

Exit Criteria:
- Version-specific code is physically isolated.
- Contract remains version-agnostic.

## Phase 4: Composition and Runtime Validation

1. Keep `CompositionRoot` as single binding point.
2. Verify startup, frame input, save/load, and magnet toggle flows.
3. Update README architecture map.

Exit Criteria:
- Debug startup works.
- Save/load/toggle paths still function.
- Documentation reflects real structure and naming.

## Risk Controls

- Do not mix behavior refactor with folder/symbol refactor in the same commit.
- Keep commits phase-scoped.
- Run build after each phase.
- Preserve `Client` / `Server` terms only where they represent runtime role, not historical naming.

## Suggested Commit Strategy

1. `refactor(structure): simplify folders and align file placement`
2. `refactor(naming): apply structure-first symbol renaming`
3. `refactor(interop): introduce v73-based interop naming`
4. `refactor(composition): rebind to versioned interop implementation`
5. `docs: update README architecture and naming rules`

## Task Checklist

- [ ] Phase 1 folder simplification completed
- [ ] Phase 1 build passes
- [ ] Phase 2 symbol/file renaming completed
- [ ] Phase 2 build passes
- [ ] Phase 3 interop versioning isolation completed
- [ ] Phase 3 build passes
- [ ] Phase 4 runtime validation completed
- [ ] README updated

## Decision Log

- 2026-04-25: Keep single project architecture.
- 2026-04-25: Enforce PascalCase + English-only naming.
- 2026-04-25: Preserve `Client` / `Server` terminology.
- 2026-04-25: Prioritize structure-first renaming over legacy name inheritance.
- 2026-04-25: Adopt V73-explicit naming for current interop implementation.
