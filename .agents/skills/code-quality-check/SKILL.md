---
name: code-quality-check
description: Review code changes for correctness, maintainability, clarity, design intent, tests, documentation, and verification before committing or requesting review.
---

# Code Quality Check

## When to Use

- Use this skill when reviewing source-code changes before committing, pushing, opening a pull request, or replying to review feedback.
- Use this skill for production code, tests, build glue, generated-code boundaries, adapters, data shapes, result types, exception handling, reflection, concurrency, lifecycle-sensitive code, and other code that future readers must safely change.
- Use this skill when a review should start from a blank reading of the diff instead of only checking whether the implementation matches the original plan.
- When a task asks you to both fix code and quality-check it, use the normal implementation workflow for the edit, then apply this skill before and after the fix as the review loop.
- Do not use this skill as a substitute for repository-specific format, commit-message, issue, or pull-request quality checks. Run those skills separately when they apply.

## Goals

- Confirm that the change is correct, scoped, and understandable to a first-time reader.
- Make behavior, ownership, design boundaries, data shapes, failure modes, and external constraints visible where they matter.
- Keep the implementation aligned with existing local patterns unless the diff clearly justifies a new pattern.
- Find missing tests, verification gaps, stale names, hidden coupling, and accidental scope creep before review.
- Prefer comments that explain why a boundary, exception, adapter, synchronization rule, or data shape exists, not comments that restate what the code already says.
- Prefer XML documentation for types and members whose design intent should appear in IDE navigation or completion, even when they are not public APIs. Prioritize members whose callers need the intent; do not require XML documentation for local implementation details that are clear in place.
- Avoid mechanical comment templates. Repeated table-like documentation is acceptable only when the repeated shape is the clearest form, such as attribute lists, key assignments, protocol fields, or input-action tables; keep the reason close enough that a reader sees why the repeated shape is intentional before or while reading the table.
- Keep comments concentrated at entry points: classes, interfaces, adapters, boundary/result types, and non-obvious integration surfaces. Use method-body comments only for non-obvious synchronization, side effects, reflection, lifecycle dependencies, or external API constraints.
- Verify behavior with the checks that fit the risk of the change, and make skipped checks explicit.

## Workflow

1. Re-read the skill trigger, description, and changed files. Confirm the body of this skill fits the actual task; if the task is only PR text, commit messages, or issue wording, switch to the more specific skill.
   - For mixed tasks, separate the code-quality review from implementation, commit-message, issue, and pull-request checks. Record which parts were handled by this skill and which parts were delegated to another workflow or skill.
2. Build a blank-reader checklist before judging the diff. Fix the checklist before editing so later iterations do not move the target:
   - What problem or boundary is this change responsible for?
   - Which files are the main entry points for a future reader?
   - Which behavior, lifecycle, external API, data-shape, performance, or compatibility constraints are non-obvious?
   - Which edge cases, failure modes, or rollback paths could be missed?
   - Which tests, builds, searches, or manual checks would prove the change is safe?
3. Review correctness and scope:
   - Look for behavioral regressions, missed edge cases, null or lifecycle hazards, ordering assumptions, reflection fragility, concurrency assumptions, and error handling gaps.
   - Check that the implementation follows existing local patterns and does not introduce unrelated refactors.
   - Check that names describe current responsibilities and do not preserve obsolete design terms.
   - Check tests for both meaningful assertions and overfitting to implementation details.
4. Review maintainability and structure:
   - Check whether responsibilities are placed at the right boundary, such as caller versus callee, adapter versus core, or parser versus model.
   - Check whether data shapes make invalid states hard to represent, or at least obvious to validate.
   - Check whether new abstractions remove real complexity instead of hiding a one-off case.
   - Check whether dependencies, side effects, logging, allocation, and error propagation are proportionate to the risk of the code path.
5. Review comments and documentation:
   - Prefer XML documentation on navigable types and members when the intent should be visible from call sites or IDE hover text.
   - Replace implementation paraphrases with design-intent comments, or remove them when the code is already clear.
   - Check for mechanical repetition across files. If a repeated shape is intentional table-like documentation, confirm the surrounding text explains why repetition helps.
   - Do not require comments in every file. Missing comments are a problem only when a first-time reader cannot infer responsibility, boundary, or reason from code and names.
6. Review tests and verification:
   - Run the repository checks appropriate to the risk, such as builds, tests, formatters, or targeted manual checks. Do not invent a universal required command list; choose checks from the repository conventions and the changed behavior.
   - Search for duplicated comment templates, obsolete design names, stale references, and known project-specific wording that the task or repository guidance forbids.
   - Exclude intentional table-like repetition from duplicate-comment findings only when its purpose is documented near the table.
7. Iterate on findings:
   - Fix the smallest coherent set of issues, then re-run the relevant checks.
   - Before each revision, state which checklist item the change is meant to satisfy. This keeps fixes tied to observed gaps instead of general taste.
   - After each revision, re-read the diff as if it were new and confirm the original checklist still passes.
   - Stop when no new ambiguity, template repetition, scope creep, or verification gap remains, or record the residual risk explicitly.
8. Report results in review order:
   - Lead with bugs, regressions, missing tests, or maintainability risks, with file and line references when available. Use the repository or review tool's severity style when one exists; otherwise order findings by practical impact without inventing heavy labels.
   - Then list open questions or assumptions.
   - For recurring ambiguity in the code, task, or review guidance, report `Issue`, `Cause`, and `General Fix Rule` so the finding can become durable guidance instead of a one-off note. If it is not an actionable bug or risk, place it under open questions or assumptions.
   - Summarize the changes and verification only after the findings.
