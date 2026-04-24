# CruiserJumpPractice Refactoring Plan

## Scope and Constraints

- Project structure remains a single `.csproj` (`CruiserJumpPractice/CruiserJumpPractice.csproj`).
- Naming is fixed to PascalCase and English only.
- `Client` / `Server` vocabulary is retained (no forced rename to `Local` / `Host`).
- This plan focuses on directory structure, namespace consistency, and naming consistency.

## Refactoring Goals

1. Make directory structure predictable from class responsibility.
2. Eliminate mixed grouping rules (layer + runtime role + technical detail in the same level).
3. Keep migration safe by splitting into reversible phases.
4. Minimize behavior changes; prioritize structural and naming consistency first.

## Target Organization (Single Project)

```text
CruiserJumpPractice/
  Bootstrap/
    CruiserJumpPractice.cs
    InputActions.cs
  Composition/
    CompositionRoot.cs
  Application/
    CruiserState/
      UseCases/
      Results/
      Services/
    Magnet/
      UseCases/
      Results/
      Services/
    Shared/
      Results/
  Domain/
    CruiserState/
      CruiserStateStore.cs
  GameInterop/
    Adapters/
      Cruiser/
      Hud/
      Network/
      Player/
      Rpc/
      Shared/
    Contracts/
      IGameInterop.cs
    Models/
      CruiserSnapshot.cs
    Exceptions/
      GameInteropException.cs
    CurrentGameInterop.cs
  Network/
    Behaviours/
      RpcSurrogateNetworkBehaviour.cs
  Presentation/
    Handlers/
      FrameHandler.cs
      StartupHandler.cs
    Services/
      ClientCruiserStateService.cs
      ClientMagnetService.cs
      ClientNotificationService.cs
    Presenters/
      ClientCruiserResultPresenter.cs
  Patches/
    HudManagerPatch.cs
```

Notes:
- Keep `Client` / `Server` terms in class names where they represent runtime behavior roles.
- `HUD` acronym normalization: prefer `Hud` in type/file names for consistency in C# naming style.
- `NetworkBehaviours` folder is normalized to `Network/Behaviours` for predictable hierarchy.

## Naming Rules (Authoritative)

### Class and File Naming

- One public/internal top-level type per file.
- File name equals type name exactly.
- PascalCase only; English words only.
- Allowed role suffixes:
  - `UseCase`
  - `Service`
  - `Presenter`
  - `Handler`
  - `Patch`
  - `Interop`
  - `Result`

### Namespace Rules

- Namespace mirrors physical path from `CruiserJumpPractice/`.
- No generic buckets like `Features` unless they represent stable product-level capabilities.
- Prefer explicit technical groupings (`Adapters`, `Contracts`, `Models`, `Exceptions`).

### Result Type Rules

- Group result enums by feature:
  - `Application/CruiserState/Results/*`
  - `Application/Magnet/Results/*`
  - `Application/Shared/Results/*` (only if truly cross-feature)
- Avoid role-based result files such as `ClientOperationResults`.

## Migration Phases

## Phase 1: Directory and Namespace Alignment (No behavior change)

1. Create target folders.
2. Move files without changing class behavior.
3. Update namespaces and using directives.
4. Build and ensure zero compile errors.

Exit Criteria:
- `dotnet build` succeeds.
- No runtime logic edits.

## Phase 2: Naming Consistency Cleanup

1. Normalize acronym casing (`HUDManagerPatch` -> `HudManagerPatch` if safe).
2. Remove ambiguous folder names (`Features` -> explicit adapter folders).
3. Align result file names with feature boundaries.

Exit Criteria:
- No mixed naming styles remain.
- Search by suffix (`*UseCase`, `*Service`) is reliable.

## Phase 3: Composition and Dependency Clarity

1. Keep `CompositionRoot` as single wiring point.
2. Reduce accidental service-locator style exposure where feasible.
3. Keep current runtime behavior unchanged.

Exit Criteria:
- Wiring is explicit and traceable.
- Patches and handlers have clear dependency flow.

## Phase 4: Verification and Documentation

1. Build in Debug and Release.
2. Validate startup flow and input-triggered actions manually.
3. Update `README.md` with the new structure summary.

Exit Criteria:
- Debug startup works.
- Save/load/toggle paths still function.
- Documentation reflects actual structure.

## Risk Controls

- Do not mix behavior refactor with structure refactor in the same commit.
- Keep commits phase-scoped.
- Prefer rename/move operations with immediate namespace fixes.
- Run build after each phase, not only at the end.

## Suggested Commit Strategy

1. `refactor(structure): align folders and namespaces`
2. `refactor(naming): normalize class and file naming`
3. `refactor(composition): clarify dependency wiring`
4. `docs: update README with new architecture map`

## Task Checklist

- [ ] Phase 1 folder moves prepared
- [ ] Phase 1 namespace updates completed
- [ ] Phase 1 build passes
- [ ] Phase 2 naming normalization completed
- [ ] Phase 2 build passes
- [ ] Phase 3 composition cleanup completed
- [ ] Phase 3 build passes
- [ ] Phase 4 docs updated
- [ ] Manual in-game verification completed

## Decision Log

- 2026-04-25: Keep single project architecture.
- 2026-04-25: Enforce PascalCase + English-only naming.
- 2026-04-25: Preserve `Client` / `Server` terminology.
