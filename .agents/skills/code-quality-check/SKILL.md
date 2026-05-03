---
name: code-quality-check
description: Review code changes for maintainability, clarity, design intent, comments, tests, and verification before committing or requesting review.
---

# Code Quality Check

## When to Use

- Use this skill when reviewing source-code changes before committing, pushing, or opening a pull request.
- Use this skill when adding or revising comments, XML documentation, adapters, boundary types, result types, exception handling, reflection, lifecycle-sensitive code, or other code that future readers must navigate.
- Use this skill when a review should start from a blank reading of the diff instead of only checking whether the implementation matches the original plan.

## Goals

- Confirm that the change is correct, scoped, and understandable to a first-time reader.
- Make design boundaries, ownership, data shapes, exceptions, and external constraints visible where they matter.
- Prefer comments that explain why a boundary, exception, adapter, or data shape exists, not comments that restate what the code already says.
- Prefer XML documentation for types and members whose design intent should appear in IDE navigation or completion, even when they are not public APIs.
- Avoid mechanical comment templates. Repeated table-like documentation is acceptable only when the repeated shape is the clearest form, such as attribute lists, key assignments, or input-action tables like `InputUtilsActions`; keep the reason for that shape nearby.
- Keep comments concentrated at entry points: classes, interfaces, adapters, boundary/result types, and non-obvious integration surfaces. Use method-body comments only for non-obvious synchronization, side effects, reflection, lifecycle dependencies, or external API constraints.
- Verify behavior with the checks that fit the risk of the change, and make skipped checks explicit.

## Workflow

1. Re-read the skill trigger and the changed files. Check that this review is about code quality, not only formatting or commit-message style.
2. Build a blank-reader checklist before judging the diff:
   - What problem or boundary is this change responsible for?
   - Which files are the main entry points for a future reader?
   - Which behavior, lifecycle, external API, or data-shape constraints are non-obvious?
   - Which tests, builds, searches, or manual checks would prove the change is safe?
3. Review correctness and scope:
   - Look for behavioral regressions, missed edge cases, null or lifecycle hazards, ordering assumptions, reflection fragility, concurrency assumptions, and error handling gaps.
   - Check that the implementation follows existing local patterns and does not introduce unrelated refactors.
   - Check that names describe current responsibilities and do not preserve obsolete design terms.
4. Review comments and documentation:
   - Prefer XML documentation on navigable types and members when the intent should be visible from call sites or IDE hover text.
   - Replace implementation paraphrases with design-intent comments, or remove them when the code is already clear.
   - Check for mechanical repetition across files. If a repeated shape is intentional table-like documentation, confirm the surrounding text explains why repetition helps.
   - Do not require comments in every file. Missing comments are a problem only when a first-time reader cannot infer responsibility, boundary, or reason from code and names.
5. Review tests and verification:
   - Run the repository checks appropriate to the risk, such as builds, tests, formatters, or targeted manual checks.
   - Search for duplicated comment templates, obsolete design names, banned wording, and stale references introduced by the change.
   - Exclude intentional table-like repetition from duplicate-comment findings only when its purpose is documented near the table.
6. Iterate on findings:
   - Fix the smallest coherent set of issues, then re-run the relevant checks.
   - After each revision, re-read the diff as if it were new and confirm the original checklist still passes.
   - Stop when no new ambiguity, template repetition, scope creep, or verification gap remains, or record the residual risk explicitly.
7. Report results in review order:
   - Lead with bugs, regressions, missing tests, or maintainability risks, with file and line references when available.
   - Then list open questions or assumptions.
   - Summarize the changes and verification only after the findings.
