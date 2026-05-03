---
name: skill-quality-check
description: Quality-check Agent Skills for trigger clarity, scope, structure, progressive disclosure, domain separation, validation, and scenario-readiness. Use when creating, updating, reviewing, or splitting Agent Skills, SKILL.md files, skill references, bundled scripts, or skill metadata.
---

# Skill Quality Check

## When to Use

- Use this skill when creating, updating, or reviewing an Agent Skill.
- Use this skill before committing changes to any `SKILL.md`, `references/`, `scripts/`,
  `assets/`, or `agents/openai.yaml` file inside a skill folder.
- Use this skill with scenario-based validation after creating or substantially revising a skill.

## Goals

- Keep each skill focused on one reusable job with clear trigger boundaries.
- Make `description` concise, specific, and useful for implicit skill selection.
- Keep `SKILL.md` lean: core workflow in the body, detailed variants in directly linked
  `references/`, deterministic helpers in `scripts/`, reusable output materials in `assets/`.
- Separate project-specific or domain-specific knowledge into dedicated domain skills or reference
  files instead of mixing it into general workflow skills.
- Preserve a consistent top-level structure: `When to Use`, `Goals`, and `Workflow` unless a local
  skill has a stronger established pattern.
- Require scenario-based validation for new or materially revised skills.

## Workflow

1. Read the changed skill files and nearby related skills.
2. Check frontmatter:
   - `name` uses lowercase letters, digits, and hyphens.
   - `description` states what the skill does and when to use it.
   - Trigger words are front-loaded enough to survive shortened skill lists.
3. Check scope:
   - One primary job per skill.
   - No unrelated project policy, domain knowledge, or historical notes in a general-purpose skill.
   - Split domain knowledge into a dedicated skill or a directly linked reference file when it would
     otherwise make the skill broad or stale.
4. Check structure:
   - Prefer `When to Use`, `Goals`, and `Workflow` for repository skills.
   - Keep required steps explicit, ordered, and written as imperatives.
   - Match specificity to risk: flexible guidance for judgment-heavy work, exact commands or scripts
     for fragile operations.
5. Check progressive disclosure:
   - Keep `SKILL.md` short enough to scan quickly.
   - Link every optional reference directly from `SKILL.md`; avoid nested reference chains.
   - Add a table of contents to reference files longer than 100 lines.
   - Do not duplicate detailed guidance between `SKILL.md` and references.
6. Check bundled resources:
   - Include `scripts/` only for repeatable or fragile automation, and test representative scripts.
   - Include `assets/` only for files used in outputs.
   - Remove placeholder or auxiliary files that do not directly support the skill.
   - For copied, adapted, generated, vendored, or reusable example files, preserve upstream
     copyright/license notices and apply the SPDX guidance from `code-quality-check`.
7. Validate and iterate:
   - Run the available skill validator, if the repository has one.
   - Run spelling, formatting, or repository checks appropriate to Markdown-only changes.
   - For each new or substantially revised skill, prepare realistic validation scenarios, evaluate
     whether the skill produces the intended behavior, apply one theme of fixes per iteration, and
     stop only when the scenarios converge or a stated cutoff is reached.
8. Record verification:
   - Note external sources consulted, why they were needed, and how their guidance was applied.
   - Note whether docs, changelog, PR notes, or follow-up domain skills are needed.

## Reference Checks

Read [references/authoring-best-practices.md](references/authoring-best-practices.md) when a change
touches structure, trigger design, bundled resources, domain separation, or scenario validation.
