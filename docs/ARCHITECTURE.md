# CruiserJumpPractice Architecture

## Layer Overview

### Presentation

- Runtime input handling: `Runtime/FrameInputCoordinator.cs`
- Harmony entry points: `Patches/HUDManagerPatch.cs`
- Purpose: react to frame/input events and call application-facing coordinators.

### Application

- Operation results: `Application/CruiserStateOperationResults.cs`, `Application/ClientOperationResults.cs`
- Use cases: `Application/UseCases/*.cs`
- Purpose: implement business rules and return explicit operation results.

### Domain

- State store: `Domain/CruiserStateStore.cs`
- Purpose: hold business state independent from transport or UI concerns.

### Infrastructure

- Game API adapters: `GameInterop/*`
- RPC transport: `NetworkBehaviours/RpcSurrogateNetworkBehaviour.cs`
- Composition root: `Services/CompositionRoot.cs`
- Purpose: integrate with Lethal Company objects and Netcode runtime.

## Design Rules

1. Use cases return result enums instead of throwing for expected conditions.
2. RPC layer transports results, while result-to-message mapping is handled by client presenters.
3. UI tip output is centralized in `ClientNotificationService`.
4. Shared cruiser saved state is managed in `CruiserStateStore`.