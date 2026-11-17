# Development Operations

## Dependency updates

Run from the repository root in PowerShell. The plugin and test project have
separate NuGet configurations and lockfiles.
NuGet applies a seven-day cooldown to automatic version selection.

For a plugin dependency update:

```powershell
dotnet restore CruiserJumpPractice/CruiserJumpPractice.csproj `
  --configfile CruiserJumpPractice/nuget.config --force-evaluate --use-lock-file
```

For a test dependency update:

```powershell
dotnet restore CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj `
  --configfile CruiserJumpPractice.Tests/nuget.config --force-evaluate --use-lock-file
```

Verify the lockfile for the project you updated:

Plugin:

```powershell
dotnet restore CruiserJumpPractice/CruiserJumpPractice.csproj `
  --configfile CruiserJumpPractice/nuget.config --locked-mode
```

Tests:

```powershell
dotnet restore CruiserJumpPractice.Tests/CruiserJumpPractice.Tests.csproj `
  --configfile CruiserJumpPractice.Tests/nuget.config --locked-mode
```
