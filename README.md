<!-- SPDX-License-Identifier: Unlicense -->

# CruiserJumpPractice

A Lethal Company mod that saves/loads cruiser position, rotation, and condition, and lets you toggle the magnet
remotely.

- [User guide](./assets/README.md)

## Development

Install .NET SDK 10.0 or later.

- <https://dotnet.microsoft.com/en-us/download/dotnet/10.0>

Install PowerShell 7.

- <https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-windows>

Install Visual Studio 2022.

- <https://visualstudio.microsoft.com/en-us/vs/>

Install Docker for local Markdown linting.

- <https://docs.docker.com/get-started/get-docker/>

Restore NuGet packages.

```powershell
dotnet restore --locked-mode
```

Open `CruiserJumpPractice.sln` in Visual Studio.

## Quality checks

Run the relevant checks before opening a pull request.

### C# format

- Language version:
  [C# 13.0](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13)
- Target framework:
  [.NET standard 2.1](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1)

```powershell
dotnet format --no-restore --verify-no-changes
```

`dotnet format` is an aggregate formatter that checks whitespace, built-in code
style, and fixable analyzer diagnostics. Roslyn analyzers also run during build,
including diagnostics that cannot be automatically fixed.

### .NET and C# tooling updates

This project separates the SDK used to build and format the mod from the target
framework that controls runtime compatibility.

- Keep `TargetFramework` on `netstandard2.1` unless Lethal Company, BepInEx,
  Unity, or compile-only dependencies require a compatibility change.
- Prefer supported LTS SDKs for routine maintenance. Use an STS or newer SDK
  major only when it solves a specific compiler, formatter, analyzer, CI, or
  Visual Studio problem.
- Keep SDK updates in maintenance-only pull requests. Update the README SDK
  requirement and both workflow `dotnet-version` values together.
- Keep `LangVersion` explicit. Before increasing it, confirm SDK, Visual Studio
  2022, and dependency compatibility, then update the C# format summary above.
- For analyzer updates, update `packages.lock.json`, review new diagnostics,
  and separate mechanical formatting from intentional rule or code changes when
  practical.
- Preserve existing restore, format, build, and Markdown lint behavior by
  default. Record compatibility checks and verification commands in the pull
  request, and defer the update when the impact is unclear.

Maintenance references:

- [.NET releases and support](https://learn.microsoft.com/en-us/dotnet/core/releases-and-support)
- [.NET SDK, MSBuild, and Visual Studio versioning](https://learn.microsoft.com/en-us/dotnet/core/porting/versioning-sdk-msbuild-vs)
- [Configure C# language version](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version)
- [`dotnet format`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-format)

### Markdown lint

Markdown is checked with `markdownlint-cli2`. The project uses the pinned Docker
image below so contributors do not need a local Node.js project.
The image's default working directory is `/workdir`, so mount the repository
there. Run it without network access and as a non-root user.

On Windows with PowerShell, use UID/GID `1000:1000`:

```powershell
docker run --rm --network none --user 1000:1000 -v ".:/workdir" davidanson/markdownlint-cli2:v0.22.1@sha256:0ed9a5f4c77ef447da2a2ac6e67caf74b214a7f80288819565e8b7d2ac148fe5
```

On Linux, use `sudo docker` and pass the host user's UID and GID:

```bash
sudo docker run --rm --network none --user "$(id -u):$(id -g)" -v ".:/workdir" davidanson/markdownlint-cli2:v0.22.1@sha256:0ed9a5f4c77ef447da2a2ac6e67caf74b214a7f80288819565e8b7d2ac148fe5
```

When updating Markdown lint tooling, update both the local Docker image and the
CI action together after the repository cooldown period has elapsed.

## Package management

To update the lock file after modifying your package references, run:

```powershell
dotnet restore --use-lock-file
```

## GitHub Actions

The repository uses GitHub Actions for CI.

### Action pinning

The version of the actions are pinned with [pinact](https://github.com/suzuki-shunsuke/pinact).
Actions and other executable CI tooling should be updated after the repository
cooldown period has elapsed. Keep SHA pins and version comments synchronized
when updating pinned actions.

```powershell
# Pin
pinact run --min-age 7

# Update
pinact run --update --min-age 7
```

### GitHub Actions configuration

#### GitHub Variables

This repository currently does not use GitHub Actions variables.

| Name | Used by | Description |
| :--- | :------ | :---------- |
| None | Not applicable | No repository variables are currently used. |

#### GitHub Secrets

| Name | Used by | Description |
| :--- | :------ | :---------- |
| `THUNDERSTORE_TOKEN` | `.github/workflows/build.yml` | Thunderstore service account token used by `.github/actions/publish-thunderstore`. |

## Build

```powershell
# Debug build
DOTNET_CLI_UI_LANGUAGE=en dotnet build

# Release build
DOTNET_CLI_UI_LANGUAGE=en dotnet build --configuration Release
```

## Release

1. Update the canonical developer changelog in `CHANGELOG.md`.
2. For a stable release, derive the Thunderstore-facing release notes in `assets/CHANGELOG.md` from stable entries
   in `CHANGELOG.md`.
3. Replace version in `CruiserJumpPractice/CruiserJumpPractice.csproj` as semver format, e.g. `1.2.3`.
4. Verify that `.github/workflows/build.yml` packages `assets/CHANGELOG.md` and that the `generate-version` action
   updates `assets/manifest.json` from the project version.
5. Commit and push the changes.
6. CI will create a GitHub Release automatically.
7. For stable releases, CI will upload the release artifact to Thunderstore automatically.

   The current workflow deploys to the Thunderstore `aoirint` team and publishes to the `lethal-company`
   community with the `Mods`, `Tweaks & Quality Of Life`, and `AI Generated` categories.
   The `THUNDERSTORE_TOKEN` secret must belong to a Thunderstore service account that can publish to that team.

   **NOTE: prerelease version is not supported by Thunderstore, e.g. `1.2.3-beta.1`.**

### AI Disclosure

Some parts of this project were developed with the assistance of AI tools based on large language models (LLMs),
including agent-based tools.
The code is reviewed by the author.
This disclosure is made in compliance with Thunderstore policies.

## Debugging

### r2modman

1. Open r2modman.
2. Open `Config editor`.
3. Open `BepInEx\config\BepInEx.cfg` in the config list.
4. Set `Logging.Console.LogLevels` to `All`.
5. Open `Settings > Import local mod`.
6. Select the DLL file from `bin/Debug/netstandard2.1/`.
7. Click `Start modded`.

### Manual

1. Install BepInEx:
   <https://docs.bepinex.dev/articles/user_guide/installation/index.html>
2. Launch `Lethal Company.exe` and exit to generate the BepInEx config files.
3. Open `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/config/BepInEx.cfg`.
4. Copy the DLL file from `bin/Debug/netstandard2.1/` into
   `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/plugins/`.
5. Set `Logging.Console.Enabled` to `true`.
6. Set `Logging.Console.LogLevels` to `All`.
7. Launch `Lethal Company.exe` again.
