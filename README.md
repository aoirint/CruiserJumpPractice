<!-- SPDX-License-Identifier: MIT -->

# CruiserJumpPractice

A [Lethal Company][lethal-company-steam] mod that saves and loads cruiser position,
rotation, and condition, and lets you toggle the magnet remotely.

- [User guide](./assets/README.md)

## Development

Install [.NET SDK 10.0][dotnet-sdk-download] or later.

Install [PowerShell 7][powershell-install].

Install [Visual Studio 2022][visual-studio-download].

Install [Docker][docker-install] if you plan to use the documented local
Markdown lint command.

Install [`actionlint`][actionlint-repo] and [pinact][pinact-repo] if you plan to
run GitHub Actions quality checks locally.

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

### Markdown lint

Markdown is checked with
[`markdownlint-cli2`][markdownlint-cli2-repo].
The pinned Docker image below is the documented local command so contributors do
not need a local Node.js project, but Docker is not required.
Other installation methods are acceptable when they run the same
`markdownlint-cli2` version with this repository's configuration.
The image's default working directory is `/workdir`, so mount the repository
there. Run the Docker command without network access and as a non-root user.

On Windows with PowerShell, use UID/GID `1000:1000`:

```powershell
docker run --rm --network none --user 1000:1000 -v ".:/workdir" davidanson/markdownlint-cli2:v0.22.1@sha256:0ed9a5f4c77ef447da2a2ac6e67caf74b214a7f80288819565e8b7d2ac148fe5
```

On Linux, use `sudo docker` and pass the host user's UID and GID:

```bash
sudo docker run --rm --network none --user "$(id -u):$(id -g)" -v ".:/workdir" davidanson/markdownlint-cli2:v0.22.1@sha256:0ed9a5f4c77ef447da2a2ac6e67caf74b214a7f80288819565e8b7d2ac148fe5
```

When updating Markdown lint tooling, update the documented local command and the
CI action together after the repository cooldown period has elapsed.

### GitHub Actions lint

GitHub Actions workflows and composite actions are checked with
[`actionlint`][actionlint-repo].
The CI check runs only actionlint's built-in workflow, expression, and action
metadata checks so it does not require extra tools such as ShellCheck or
pyflakes.

```powershell
actionlint -shellcheck= -pyflakes=
```

Install `actionlint` from its upstream releases, package-manager integrations,
Docker image, or another trusted distribution.
Use a cooldown-compliant pinned release when updating CI.
The CI workflow downloads the Linux amd64 release archive directly and verifies
its SHA256 before running it. CI caches the archive, not the extracted
executable, so cached downloads are still verified before use.

### GitHub Actions pinning

GitHub Actions and reusable workflows are checked with [pinact][pinact-repo] so
external actions stay pinned to full commit SHAs with synchronized version
comments.

```powershell
pinact run --check --min-age 7
```

For local fixes or updates, use the documented pinact commands:

```powershell
# Pin or refresh version comments.
pinact run --min-age 7

# Update pinned actions after the repository cooldown period.
pinact run --update --min-age 7
```

Set `GITHUB_TOKEN` when possible so pinact can query GitHub's API with normal
authenticated rate limits.
Install pinact from its upstream releases, package-manager integrations, or
another trusted distribution.
The CI workflow downloads the Linux amd64 release archive directly and verifies
its SHA256 before running it. CI caches the archive, not the extracted
executable, so cached downloads are still verified before use.

## Package management

### Dependency updates

To update the lock file after modifying your package references, run:

```powershell
dotnet restore --use-lock-file
```

### Thunderstore manifest dependency updates

When changing `assets/manifest.json` dependency strings:

- Compare them with the current documented test environment in
  `assets/README.md` and `CHANGELOG.md`.
- Treat the change as Thunderstore install metadata, not only documentation:
  fresh installs or profile resolution may pull the listed dependency versions.
- Treat dependency version increases as possible practical minimum runtime
  baseline changes for Thunderstore installs.
- Document the reason, install impact, compatibility impact, and rollback risk
  in the pull request and `CHANGELOG.md`.
- Remove or revise changelog compatibility notes that no longer match the
  dependency baseline.
- Keep dependency string updates separate from manifest description or
  compatibility-marker prose when practical.

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

## GitHub Actions

The repository uses [GitHub Actions][github-actions-docs] for CI.

### Action pinning

Action versions are pinned with [pinact][pinact-repo].
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
2. For a stable release, derive the Thunderstore-facing release notes in
   `assets/CHANGELOG.md` from stable entries in `CHANGELOG.md`.
3. Update the Thunderstore package description compatibility marker in
   `assets/manifest.json` when needed.
4. Verify Thunderstore package metadata in `assets/manifest.json`:
    - Confirm dependency strings match the intended release baseline.
    - Confirm dependency string changes have documented reason, install impact,
      compatibility impact, rollback risk, and test-environment evidence.
    - Confirm the manifest description still matches the Thunderstore-facing
      release intent.
5. Replace version in `CruiserJumpPractice/CruiserJumpPractice.csproj` with a
   SemVer version such as `1.2.3`.
6. Verify the release packaging flow:
    - `.github/workflows/build.yml` packages `assets/CHANGELOG.md`.
    - The `generate-version` action updates `assets/manifest.json` from the
      project version.
7. Commit and push the changes.
8. CI will create a GitHub Release automatically.
9. For stable releases, CI will upload the release artifact to Thunderstore
   automatically.

### Compatibility marker

Update the leading compatibility marker in `assets/manifest.json` only when the
`assets/README.md` `Compatibility` section records a newly tested or
maintainer-confirmed Lethal Company version.

Use the compact marker format at the start of the description:

- `[v<version>]`
- `[v<older-version>/<newer-version>]`

For example, use `[v73/v81.5]` when the marker intentionally covers both Lethal
Company versions.

Marker versions mean tested or maintainer-confirmed Lethal Company versions
from `assets/README.md`.
List slash-separated versions from older to newer.
Keep best-effort or lower-confidence compatibility notes out of the marker;
document those in `assets/README.md` or `CHANGELOG.md` instead.

When updating the marker:

- Replace any existing leading marker with the new marker.
- Preserve the base description after the marker:
  `[v<version-or-versions>] <description without the compatibility marker>`.
- Treat single-version markers such as `[v81.5]` and slash-separated markers
  such as `[v73/v81.5]` as the same leading compatibility marker.
- Keep exactly one leading compatibility marker group in the manifest
  description.

Keep detailed compatibility and test-environment information in
`assets/README.md` and `CHANGELOG.md`.
Treat `CHANGELOG.md` as the developer-facing compatibility history.

Handle dependency string changes in `assets/manifest.json` as separate
dependency maintenance.
Document the reason and compatibility impact in that change.

### Thunderstore publishing

The current workflow deploys to the Thunderstore `aoirint` team and publishes to
the [`lethal-company`][thunderstore-lethal-company-community] community with these
categories:

- `Mods`
- `Tweaks & Quality Of Life`
- `AI Generated`

The `THUNDERSTORE_TOKEN` secret must belong to a Thunderstore service account
that can publish to that team.

**NOTE: Prerelease versions such as `1.2.3-alpha.1` are uploaded only to GitHub,
because Thunderstore does not support them.**

## Debugging

### Validation logging

Structured validation logs are disabled by default. To enable them for release
validation, open the generated CruiserJumpPractice config file and set:

```ini
[Debug]
ValidationLogging = true
```

Enabled validation records use one JSON object per log line after the stable
`[CJP_VALIDATION]` prefix and following space. Prefer sharing extracted
`[CJP_VALIDATION]` lines plus nearby error lines when asking for validation
help. Share full BepInEx logs only when needed, because other mods or BepInEx
can include unrelated local environment details.

Harmony callback failures that are intentionally swallowed may emit
`callback_exception` validation records. These records include only the stable
callback token and exception type.

### r2modman

1. Open [r2modman][r2modman-package].
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

## AI Disclosure

Some parts of this project were developed with AI tools based on large language
models (LLMs), including agent-based tools.
The project maintainer reviews the code.
This disclosure is made in compliance with Thunderstore and community policies.

[docker-install]: https://docs.docker.com/get-started/get-docker/
[dotnet-sdk-download]: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
[github-actions-docs]: https://docs.github.com/en/actions
[actionlint-repo]: https://github.com/rhysd/actionlint
[lethal-company-steam]: https://store.steampowered.com/app/1966720/Lethal_Company/
[markdownlint-cli2-repo]: https://github.com/DavidAnson/markdownlint-cli2
[pinact-repo]: https://github.com/suzuki-shunsuke/pinact
[powershell-install]: https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-windows
[r2modman-package]: https://thunderstore.io/c/lethal-company/p/ebkr/r2modman/
[thunderstore-lethal-company-community]: https://thunderstore.io/c/lethal-company/
[visual-studio-download]: https://visualstudio.microsoft.com/en-us/vs/
