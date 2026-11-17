# Development Operations

## Dependency updates

Run these commands from the repository root in PowerShell.

Wait seven days after a package version is published unless a trusted
security advisory warrants an earlier update.

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
