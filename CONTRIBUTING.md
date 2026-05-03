<!-- SPDX-License-Identifier: Unlicense -->

# Contributing

Thank you for your interest in improving CruiserJumpPractice. This project welcomes focused bug reports,
documentation improvements, compatibility notes, and small code changes that are easy to review.
The project maintainer is listed in [CODEOWNERS](./.github/CODEOWNERS).

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
- Pull requests must include the pull request template checkbox confirmation for the Contribution License Agreement
  before they can be merged. Pull requests without that confirmation may be closed without further notice.

## Stalled Pull Requests

- Respond to maintainer feedback when possible. If you need more time, are blocked, or no longer plan to continue the
  pull request, leave a short comment so maintainers know what to expect. Even if a long time has passed, it is always
  fine to reply with an update.
- If you want to continue work from a stalled pull request, leave a short comment for the maintainer and the original
  contributor before opening a new pull request. The original contributor may not be available to respond, but the
  maintainer can confirm whether the change is still wanted and coordinate attribution or next steps.
- To keep work moving, maintainers may accept another contribution for the same issue without first rejecting an
  inactive pull request.
- If a pull request stalls, maintainers or another contributor may continue the work in a separate pull request,
  including by reusing or adapting the stalled pull request's commits, patch, tests, documentation, or ideas under the
  Contribution License Agreement.
- If your pull request reuses substantial work from another pull request, credit the original pull request and
  contributor in your pull request description.
- To keep maintainer work manageable and the review queue current, pull requests that remain inactive for a reasonable
  period may be closed. This is not a judgment on the contributor, and it does not prevent you from opening a new pull
  request later if the change is still useful.

## Contribution License Agreement

By submitting a contribution to this project, you agree to this Contribution License Agreement.
If this agreement changes, new pull requests must use the current agreement.

For this agreement, "you" means the person or organization submitting the contribution, and "contribution" means code,
documentation, assets, patches, generated output, or other material that you intentionally submit for inclusion in this
project. Ordinary issue reports, pull request discussion, questions, and suggestions are not contributions under this
agreement unless you clearly submit them for inclusion in the project.

By submitting a contribution, you represent and agree that:

- You have the legal right to submit the contribution and to grant the rights described in this agreement.
- Your contribution may be distributed under the same license as this project, without additional terms or conditions.
- You grant the maintainer and downstream recipients a perpetual, worldwide, non-exclusive, no-charge, royalty-free,
  irrevocable copyright license to use, copy, modify, merge, publish, distribute, sublicense, and otherwise use your
  contribution as part of this project.
- You grant the maintainer and downstream recipients a perpetual, worldwide, non-exclusive, no-charge, royalty-free,
  irrevocable patent license to make, have made, use, offer to sell, sell, import, and otherwise transfer your
  contribution as part of this project. This patent license applies only to patent claims that you can license and that
  are necessarily infringed by your contribution alone or by combining your contribution with the project.
- You keep any copyright you hold in your contribution. This agreement is a license grant, not a copyright assignment.
- The maintainer is not required to accept, publish, retain, or distribute any contribution.
- Do not submit code, documentation, assets, generated output, or other materials if you do not have the right to
  contribute them under this agreement.

## AI-assisted contributions

AI tools may be used as aids, but the human contributor remains responsible for the contribution.
There is no exact percentage or universal rule for when assistance significantly affects a pull request. Use the
following common workflows as practical examples, not as an exhaustive list.

Examples that should be disclosed:

- In the pull request description, disclose AI or tool assistance that significantly affected the pull request,
  but keep the disclosure practical. A short summary of the workflow is enough; you do not need to provide a
  file-by-file table of AI involvement.
- For example, say that you gave an agent a rough design prompt and had it implement the change, note the areas you
  remember asking it to revise, and mention what you focused on when reviewing or adapting the result.
- Disclose AI-generated or tool-generated output when you submit it as project code, documentation, assets, tests, or
  other project content.
- Disclose agent-written or AI-written commits, patches, tests, documentation, release notes, package metadata, or
  assets that you edited and submitted.
- Disclose workflows where AI proposed the implementation approach, debugging steps, compatibility decisions, or review
  response that shaped the final contribution.

Examples that normally do not need disclosure:

- Ordinary spell-checking, formatting, search, translation used only for your own understanding, or small completion
  suggestions normally do not need disclosure when they did not shape the substance of the contribution.

When in doubt:

- If you are unsure whether assistance was significant, disclose it briefly.

Contributor responsibilities:

- Review every AI-assisted change yourself. Do not assume generated code, documentation, tests, or explanations are
  correct.
- Provide verification evidence that matches the change, such as test output, build output, screenshots, or a clear
  manual test description.
- Agent-generated pull requests are allowed only when a human contributor understands the change, adapts it to this
  codebase, verifies it, discloses the assistance, and can personally explain and maintain it.
- Do not submit low-effort AI-generated or "vibe-coded" pull requests that do not meet those requirements.
- Write pull request descriptions and review replies in your own words. Use AI for editing help only if you still
  review and stand behind the final text.
- Maintainers may close undisclosed, unverified, low-quality, or spam-like AI-assisted contributions.

## Reporting security issues

If you believe you found a security issue, avoid posting exploit details in a public issue. Contact the maintainer
privately when possible, or open a minimal public issue that asks how to report a sensitive problem.
