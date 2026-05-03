<!-- SPDX-License-Identifier: Unlicense -->

# Contributing

Thank you for your interest in improving CruiserJumpPractice. This project welcomes focused bug reports,
documentation improvements, compatibility notes, and small code changes that are easy to review.

## Before you start

- Check the existing issues and pull requests to avoid duplicate work.
- Open an issue first for larger behavior changes, compatibility changes, or anything that may affect release
  packaging.
- Keep changes focused. Separate unrelated fixes, refactors, and documentation updates into separate pull requests
  when practical.

## Development setup

Follow the setup, formatting, build, package management, debugging, and release notes in [README.md](./README.md).
At minimum, install the documented .NET SDK and restore packages before building:

```powershell
dotnet restore --locked-mode
```

## Making changes

- Prefer the existing project structure and naming conventions.
- Keep user-facing behavior explicit in code, documentation, or changelog entries when the behavior changes.
- Update `CHANGELOG.md` for developer-facing changes that should appear in release history.
- Update files under `assets/` when the Thunderstore package metadata, icon, README, or release notes change.
- Do not commit build output, downloaded game files, local mod manager profiles, or local machine configuration.

## Contribution License Agreement

By submitting a contribution to this project, you agree to the following terms:

- You have the right to submit the contribution and to grant the permissions described here.
- You grant the maintainer and downstream recipients a perpetual, worldwide, non-exclusive, no-charge, royalty-free,
  irrevocable license to use, copy, modify, merge, publish, distribute, sublicense, and otherwise use your contribution
  as part of this project.
- Unless you clearly state otherwise before the contribution is accepted, your contribution may be distributed under
  the same license as this project.
- If your contribution includes patentable material, you grant any patent license you are able to grant that is needed
  to use your contribution as part of this project.
- You understand that you keep any copyright you hold in your contribution. This agreement is a license grant, not a
  copyright assignment.
- Do not submit code, documentation, assets, generated output, or other materials if you do not have the right to
  contribute them under these terms.

## Verification

Run the checks that match your change before opening a pull request:

```powershell
dotnet format
DOTNET_CLI_UI_LANGUAGE=en dotnet build
```

For package or release changes, also verify the release documentation in [README.md](./README.md) and confirm that
the Thunderstore-facing files under `assets/` are still correct.

## Pull requests

- Use a clear title that summarizes the change.
- Describe what changed and how you verified it.
- Link related issues when applicable.
- Keep the pull request small enough for maintainers to review without guessing at unrelated intent.
- Mention any skipped checks and why they were skipped.

## AI-assisted contributions

If you used AI tools or LLM-based agents to create or substantially modify a contribution, disclose that assistance in
the pull request description. The contributor is responsible for reviewing the result, checking licenses, and verifying
that the change is appropriate for the project.

## Reporting security issues

If you believe you found a security issue, avoid posting exploit details in a public issue. Contact the maintainer
privately when possible, or open a minimal public issue that asks how to report a sensitive problem.
