# Core Unit Tests

## Prerequisites

Install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0),
which includes the .NET 10 runtime used by the tests.
Run the commands below from the repository root.

Package restore needs network access to nuget.org and the BepInEx NuGet feed.
The tests run locally with hand-written fakes and need no game installation,
Steam, Unity editor, or BepInEx process.

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
removes build outputs.

## Test scope and framework

The test project targets `net10.0` and references the production assembly.
The plugin remains `netstandard2.1` with C# 13. Tests receive access to internal
types through `InternalsVisibleTo("CruiserJumpPractice.Tests")`.
See [layer boundaries](../architecture/layer-boundaries.md) for Core ownership.

NuGet restore installs xUnit.net v3 and the VSTest integration from
`Microsoft.NET.Test.Sdk` and `xunit.runner.visualstudio`.
The suite runs through `dotnet test` or Visual Studio Test Explorer.
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

## CI

Both pull-request and main workflows call `check-dotnet-bepinex-source`, which
runs the solution test command after build. Packaging depends on that gate and
copies only the production plugin DLL; test binaries stay out of the archive.

## Updating test dependencies

The test framework and runner are private development dependencies.
`dotnet restore --locked-mode` installs the committed dependency graph for both
projects. Use the procedure below when intentionally updating a test package.

Before changing a test dependency, review its publisher, immutable version,
release age of at least seven days, license, content hash, and transitive graph.
Edit the package version in
`CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj`, then regenerate its
lockfile:

```shell
dotnet restore CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj --force-evaluate
```

Review the resulting lockfile diff, then run the verification commands above.
Keep the production dependency graph unchanged unless its update is separately
intended. Reverting the project and lockfile changes restores the previous
dependency selection. Update this guide when the runtime requirements, test
scope, commands, or CI ownership change.
