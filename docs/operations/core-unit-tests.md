# Core Unit Tests

## Scope and prerequisites

Install the SDK selected by `global.json` (currently 10.0.302) and run commands
from the repository root. Restore needs access to the explicit NuGet feeds in
each project's `nuget.config`; running tests needs no game installation,
Steam, Unity editor, BepInEx process, or filesystem fixtures.

The test project targets `net10.0` and references the production assembly.
The plugin remains `netstandard2.1` with C# 13. Tests receive access to internal
types through `InternalsVisibleTo("CruiserJumpPractice.Tests")`.
See [layer boundaries](../architecture/layer-boundaries.md) for Core ownership.

## Framework and runner

Use xUnit.net v3 3.2.2 with the VSTest runner supplied by
`Microsoft.NET.Test.Sdk` 18.0.1 and `xunit.runner.visualstudio` 3.1.5.
This supports the existing solution-level `dotnet test` workflow and Visual
Studio Test Explorer without changing the SDK's runner selection.
The executable output type is required by xUnit v3.
The [xUnit getting-started guide](https://xunit.net/docs/getting-started/v3/getting-started)
describes the framework and VSTest support.

Hand-written fakes implement `IGameInterop`, `IPracticeInput`, `IPluginLogger`,
and `IValidationLogger`. Each test owns fresh state. Assertions cover observable
results, saved snapshots, game requests, HUD tokens, errors, and validation
events. Ordering assertions protect precondition priority, acceptance before
host-local RPC dispatch, and restore observations before success.

The suite covers host authorization, save/load failure paths, deferred magnet
synchronization feedback, all input combinations, all busy-state combinations,
and result presentation including unknown enum values.
Unity adapters, Harmony callbacks, and actual multiplayer synchronization need
separate integration or in-game verification. These tests establish Core
behavior, not game-version compatibility. No coverage threshold is imposed.

## Run and verify

```shell
dotnet restore --locked-mode
dotnet format --no-restore --verify-no-changes
dotnet build --no-restore
dotnet test --no-restore --no-build
```

The last command discovers and runs the test project through the solution and
returns a nonzero exit code on failure. For focused iteration:

```shell
dotnet test CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj --no-restore
```

Restore and build create ignored `obj/` and `bin/` directories. `dotnet clean`
removes build outputs. No game state is changed by these commands.

Both pull-request and main workflows call `check-dotnet-bepinex-source`, which
runs the solution test command after build. Packaging depends on that gate and
copies only the production plugin DLL; test binaries stay out of the archive.
The setup action reserves both projects' build directories and caches using
both lockfiles.

## Dependency maintenance

Test references are exact versions and private development dependencies.
The test project's `nuget.config` clears implicit sources and maps the test
framework to nuget.org, while retaining mappings for production dependencies.
Each resolving project has its own committed `packages.lock.json`.

Before changing a test dependency, review its publisher, immutable version,
release age of at least seven days, license, content hash, and transitive graph.
Update the project reference intentionally, regenerate the test lockfile, then
repeat the locked verification commands above:

```shell
dotnet restore CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj --force-evaluate
```

Review every lockfile diff and keep the production graph unchanged unless that
update is separately intended. Reverting the project and lockfile changes
restores the previous dependency selection. Update this guide when the runner,
SDK, test scope, commands, or CI ownership changes.
