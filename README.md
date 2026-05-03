<!-- SPDX-License-Identifier: Unlicense -->

# CruiserJumpPractice

A Lethal Company mod that saves/loads cruiser position, rotation, and condition, and lets you toggle the magnet remotely.

- [User guide](./assets/README.md)

## Development

Install .NET SDK 10.0 or later.

- <https://dotnet.microsoft.com/en-us/download/dotnet/10.0>

Install PowerShell 7.

- <https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-windows>

Install Visual Studio 2022.

- <https://visualstudio.microsoft.com/en-us/vs/>

Restore NuGet packages.

```powershell
dotnet restore --locked-mode
```

Open `CruiserJumpPractice.sln` in Visual Studio.

## Code format

- Language version: [C# 13.0](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13)
- Target framework: [.NET standard 2.1](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-1)

```powershell
dotnet format
```

## Package management

To update the lock file after modifying your package references, run:

```powershell
dotnet restore --use-lock-file
```

## GitHub Actions

The repository uses GitHub Actions for CI.

### Action pinning

The version of the actions are pinned with [pinact](https://github.com/suzuki-shunsuke/pinact).

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
| `THUNDERSTORE_TOKEN` | `.github/workflows/build.yml` | Thunderstore service account token used by `.github/actions/publish-thunderstore` to publish stable releases to the `aoirint` team. |

The current workflow publishes stable releases to the `lethal-company`
community with the `Mods`, `Tweaks & Quality Of Life`, and `AI Generated`
categories.

## Build

```powershell
# Debug build
DOTNET_CLI_UI_LANGUAGE=en dotnet build

# Release build
DOTNET_CLI_UI_LANGUAGE=en dotnet build --configuration Release
```

## Release

1. Update the canonical developer changelog in `CHANGELOG.md`.
2. For a stable release, derive the Thunderstore-facing release notes in `assets/CHANGELOG.md` from stable entries in
   `CHANGELOG.md`.
3. Replace version in `CruiserJumpPractice/CruiserJumpPractice.csproj` as semver format, e.g. `1.2.3`.
4. Verify that `.github/workflows/build.yml` packages `assets/CHANGELOG.md` and that the `generate-version`
   action updates `assets/manifest.json` from the project version.
5. Commit and push the changes.
6. CI will create a GitHub Release automatically.
7. For stable releases, CI will upload the release artifact to Thunderstore automatically.
   **NOTE: prerelease version is not supported, e.g. `1.2.3-beta.1`.**

### AI Disclosure

Some parts of this project were developed with the assistance of AI tools based on large language models (LLMs), including agent-based tools.
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

1. Install BepInEx: https://docs.bepinex.dev/articles/user_guide/installation/index.html
2. Launch `Lethal Company.exe` and exit to generate the BepInEx config files.
3. Open `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/config/BepInEx.cfg`.
4. Copy the DLL file into `C:/Program Files (x86)/Steam/steamapps/common/Lethal Company/BepInEx/plugins/` from `bin/Debug/netstandard2.1/`.
5. Set `Logging.Console.Enabled` to `true`.
6. Set `Logging.Console.LogLevels` to `All`.
7. Launch `Lethal Company.exe` again.
