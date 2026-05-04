---
# SPDX-License-Identifier: Unlicense
name: code-quality-check
description: Language-agnostic code quality review for implementation, SPDX/license notices, and supply-chain-sensitive changes. Use when editing source code, generated code, tests, scripts, configuration-as-code, examples, dependencies, downloaded tools, or CI actions to improve readability, preserve maintainability, decide when design intent should be captured in comments, and apply shared verification discipline.
---

# Code Quality Check

## When to Use

Use this skill for any implementation change, regardless of language or framework, before
committing or preparing a pull request.

When the changed implementation is an Agent Skill, use `skill-quality-check` together with this
skill for trigger design, structure, progressive disclosure, domain separation, and scenario
validation expectations.

## Goals

- Make the changed code easy to read, review, debug, and safely modify later.
- Prefer simple structure and clear names over explanatory comments.
- Preserve important design decisions where future maintainers will need the context.
- Avoid comment noise that repeats obvious code behavior.
- Run the smallest meaningful executable checks first, then widen only when needed.
- Keep verification notes concise and reusable for commit summaries, PR bodies, or handoff notes.
- Reduce supply-chain risk when changes introduce or update external dependencies, downloaded tools,
  or CI actions.
- Keep license notices machine-readable, accurate to the file's provenance, and consistent with
  SPDX best practices.

## Workflow

1. Re-read the changed files and nearby call sites, not just the patch.
2. Identify code that is hard to scan, overly nested, duplicated, misleadingly named, or coupled to
   hidden assumptions.
3. Improve readability directly when a safe local refactor is practical.
4. Add or update comments only when the design intent is not obvious from the code and cannot be
   made obvious with a small refactor.
5. Remove stale, redundant, or misleading comments.
6. Classify changed text as developer-facing, user-facing, or external-contract text before
   recommending wording changes.
7. Re-run the project's language-specific quality checks after edits.
8. Summarize which checks ran, which passed, and why any relevant check was skipped.

When reviewing an abstract scenario or a proposed change without concrete files, produce a review
plan instead of pretending to inspect code. State the assumptions, the readability changes you would
try first, the comments you would keep or add, the text-audience classifications that must be made,
and the checks you would run once real files exist.

Separate missing input from skill ambiguity. If exact files, commands, release metadata, or
provenance are unavailable, report them as assumptions, verification blockers, or target-change
risks. Do not treat them as unclear points in this skill unless the workflow itself fails to say how
to proceed.

## Comment Policy

Use comments to explain **why**, not **what**, unless the code is constrained by an external system
or a non-obvious algorithm.

Good reasons to leave a comment:

- A trade-off was chosen and another reasonable approach was rejected.
- The code works around a platform, API, data, timing, or compatibility constraint.
- The code depends on ordering, lifetime, concurrency, precision, security, or performance behavior
  that is easy to break later.
- A compact algorithm or protocol step would be difficult to infer from names alone.
- A fallback, validation rule, or migration path preserves behavior for older data.

Prefer refactoring instead of commenting when:

- A better variable, function, type, or module name would make the intent clear.
- Extracting a small helper would remove nesting or repeated logic.
- Reordering code, narrowing scope, or simplifying a condition would make the flow obvious.
- The comment would merely restate assignments, branches, or function calls.

When adding a comment:

- Keep it close to the code it explains.
- Keep it short and specific.
- Name the constraint, trade-off, or invariant.
- Make each comment an accurate explanation of the specific item it annotates. Do not use one
  vague or generic comment to justify multiple unrelated lines, files, suppressions, exceptions, or
  generated edits.
- During bulk edits, review comments one by one after generation. Replace templated filler such as
  "required for tests" or "framework compatibility" with the concrete reason for that exact site,
  or remove the comment if no site-specific reason exists.
- When introducing or tightening a comment policy, search the affected scope for existing comments
  of that kind and apply the policy consistently. Do not update only the new examples while leaving
  matching existing comments below the new standard.
- Place comments where they do not break syntax-aware tooling or formatter grouping. If an adjacent
  standalone comment would split an import block, list, mapping, or generated section in a way tools
  rewrite or reject, use a same-line comment or another nearby location that preserves the group.
- Update nearby tests or verification notes if the comment documents behavior that must stay true.

## Documentation and Comment Wording

When editing documentation, comments, release notes, PR text, or other explanatory prose, improve
readability without flattening meaning.

- Split overloaded sentences when they carry multiple ideas, conditions, time references, confidence
  levels, or relationships.
- Split or restructure paragraphs and list items when they become too dense to scan.
  - Use separate paragraphs, parent bullets with indented child bullets, or another local document
    pattern that makes each idea easy to review.
- Preserve the nuance that made the original wording important. Keep distinctions such as certainty,
  scope, timing, exception status, dependency relationships, and whether a statement is original,
  backfilled, inferred, or superseded.
- Use as many short sentences or nested bullets as needed to make the relationship readable. Do not
  force a fixed sentence count when the content needs a different shape.
- After splitting text, re-read the result as a whole and confirm it still answers the same question
  as the original wording.

### CI and Configuration Comments

For configuration-as-code, CI workflows, build files, and tool configuration, intent comments are
most useful near non-obvious fixed values: pinned tool versions, runtime images, lockfile paths,
ordering constraints, suppressions, generated-file exclusions, timeout values, matrices, or external
action references.

When commenting on those values:

- Explain the maintenance signal as well as the initial reason: when maintainers should revisit the
  value and what compatibility, reproducibility, security, or operational constraint must stay true.
- Keep broad repeated update policy in one maintained document or shared workflow guidance when
  repeating it beside every value would drift.
- Use local comments for site-specific constraints, exceptions, or links to central policy.
- Avoid comments that merely restate the YAML key, tool option, version literal, or file path.

## Developer-Facing Language

Treat developer-facing text separately from user-facing text. This includes comments, log messages,
exception messages, diagnostics, and internal assertions.

Use English for developer-facing text regardless of the application's user-facing language.

When the audience is unclear, classify whether the text is developer-facing, user-facing, or part of
an external contract before changing it.

## User-Facing Language

Treat user-facing text separately from developer-facing text. This includes UI copy, CLI output,
validation messages, accessibility text, and external API text shown to users or consumed by
integrations.

Follow the application's product copy, localization, accessibility, CLI output, and external API
contract rules for user-facing text.

Do not change user-facing text to English only to satisfy the developer-facing language policy. If a
message can be both user-facing and developer-facing, classify its audience and contract first, then
update tests, snapshots, docs, or changelog entries that intentionally cover that surface.

## Design Decision Checklist

Before finishing, check whether the change includes any design decision that future maintainers
would not recover from the code alone:

- Why this boundary, abstraction, data shape, or lifecycle was chosen.
- Why a simpler-looking alternative is unsafe or intentionally avoided.
- Why behavior differs across platforms, versions, modes, providers, or file formats.
- Why an error is swallowed, retried, delayed, cached, normalized, or surfaced.
- Why a broad dependency, global state, mutable state, or escape hatch is acceptable here.

If the decision still matters after the code is cleaned up, record it with a concise comment near
the relevant code. If the decision affects public behavior, configuration, operations, or release
notes, also update the appropriate docs or changelog.

## SPDX License Notices

When adding new source, scripts, examples, generated-code templates, or other reusable text files,
check whether a license notice is expected by the file type or neighboring files.

Use the repository's existing notice convention for straightforward project-local files. If the
file has copied, adapted, generated, vendored, downloaded, dual-licensed, copyleft-sensitive, or
otherwise external provenance, read
[references/spdx-license-notices.md](references/spdx-license-notices.md) before editing the header.

Do not guess when provenance, license compatibility, or required attribution is uncertain. Preserve
existing notices, keep the current project convention where possible, and record the uncertainty in
the final summary or PR notes for maintainer review.

## Verification Discipline

- Start with the narrowest check that exercises the changed behavior, then run the broader
  language-, framework-, or tool-specific checks expected by the project.
- If a check fails, fix the narrow cause and rerun that same check before widening scope.
- Do not skip executable checks only because dependencies or tools are not installed yet; install or
  provision them using the project's documented workflow when that is safe and reproducible.
- If verification is still impossible after setup, state the concrete blocker and the command that
  could not run.
- Keep verification consistent across local fixes, commits, and review or handoff preparation.
- Record skipped checks with a concrete reason, not a generic "not run" note.

## Supply-Chain Baseline

- Treat newly introduced or updated third-party packages, downloaded CI binaries, and GitHub Actions
  as supply-chain-sensitive changes.
- Require a minimum 7-day cooldown before adopting newly released third-party packages, downloaded
  CI binaries, GitHub Actions, or other external executable artifacts, unless the user explicitly
  approves an exception.
- Follow the repository's cooldown, pinning, and lockfile policies before adopting new versions.
- Prefer pinned, reviewable versions over floating references.
- If a package manager, registry, hosting site, or security tool cannot report release age or
  provenance directly, look for another reliable source such as tags, release pages, changelogs,
  signed artifacts, lockfile metadata, or upstream commit history. If release age or provenance
  still cannot be verified, treat the dependency as not satisfying the cooldown principle.
- Record any user-approved exception to the repository policy in the final summary and PR notes.
- For copied, generated, vendored, or downloaded files, verify that the SPDX notice matches the
  upstream license and record the source, version or commit, and validation method when relevant.

## Output Checklist

- Readability issues were either fixed or deliberately left with a reason.
- Important design intent is captured near the code, docs, or PR notes.
- Comments explain non-obvious intent and do not repeat obvious code behavior.
- Stale or misleading comments were removed.
- New reusable files either follow the repository's SPDX notice convention, deliberately inherit an
  existing nearby convention, or have a documented reason for omitting/changing the notice.
- Language-specific formatting, linting, typing, and tests were run or any skipped check has a
  concrete reason.
- Missing files, commands, metadata, or provenance were reported as assumptions, target-change
  risks, or verification blockers rather than as findings invented from unavailable evidence.
- Agent Skill changes were checked with `skill-quality-check` when applicable.
- New or updated dependencies, downloaded tools, and CI actions were checked against the
  repository's supply-chain policy, including the minimum 7-day cooldown or a recorded
  user-approved exception.
- If no concrete code was available, the output clearly says that this was a review plan and lists
  the assumptions instead of presenting file-specific findings.
