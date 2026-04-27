---
name: git-worktree-workflow
description: Use Git worktrees for repository implementation tasks unless explicitly told not to.
---

# Git Worktree Workflow

## When to Use

- Use this skill when implementing repository changes unless the user explicitly instructs you not to use Git worktrees.
- This includes code, tests, build files, documentation, repository guidance, and pull-request preparation.

## Worktree Location

Create implementation worktrees under:

```text
.agents/worktrees/
```

Use a short, descriptive branch and directory name, such as:

```text
.agents/worktrees/fix-cruiser-state-load
```

## Starting Point

Start from the latest base branch, usually `main`, unless the user names another base:

```powershell
git fetch origin main
git worktree add -b <branch-name> .agents/worktrees/<branch-name> origin/main
```

If network access or Git metadata writes require approval, request it and continue after approval.

## Safety Rules

- Treat uncommitted or untracked files in the original worktree as user work.
- Do not edit implementation files in the original worktree after creating the task worktree.
- Do not remove another worktree unless the user explicitly asks.
- If the branch or worktree path already exists, choose a new descriptive name or inspect it before reuse.
- If Git reports dubious ownership for the new worktree, add only that worktree path as a `safe.directory`.

## Implementation Flow

1. Check the current repository state.
2. Fetch the latest base branch.
3. Create a branch and worktree under `.agents/worktrees/`.
4. Before editing, make an implementation plan split into practical phases.
5. For each phase, do only that phase's implementation, formatting, and verification inside the new worktree.
6. Commit that phase immediately using `commit-message-quality-check` before starting the next phase.
7. Repeat steps 5 and 6 until the planned phases are complete.
8. Push the branch.
9. Create or update the pull request using `pull-request-quality-check`.

## Verification

Run checks that match the change risk and repository conventions. For this repository, useful checks often include:

```powershell
dotnet build CruiserJumpPractice.sln
```

If verification is skipped, state why in the pull request body.

## Pull Request Notes

- Keep pull request titles and bodies consistent with `pull-request-quality-check`.
- Remove temporary PR body files from the worktree after creating or editing the pull request.
