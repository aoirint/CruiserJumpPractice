---
name: git-worktree-workflow
description: Use Git worktrees for implementation tasks unless explicitly told not to. Use when asked to implement code changes, create a branch, or prepare a pull request from the latest main branch.
---

# Git Worktree Workflow

## When to Use

- Use this skill when implementing repository changes unless the user explicitly instructs you not to use Git worktrees.
- Use this skill when implementation work should be isolated from the user's current working tree.
- Use this skill when the user asks to create a pull request from the latest `main` branch.
- Use this skill when the user explicitly asks to work in a Git worktree.

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

Start from the latest `main` branch unless the user names another base:

```powershell
git fetch origin main
git worktree add -b <branch-name> .agents/worktrees/<branch-name> origin/main
```

If network access or Git metadata writes require approval, request it and continue after approval.

## Safety Rules

- Check the original worktree with `git status --short --branch` before creating a new worktree.
- Treat uncommitted or untracked files in the original worktree as user work.
- Do not edit implementation files in the original worktree after creating the task worktree.
- Do not remove another worktree unless the user explicitly asks.
- If the branch or worktree path already exists, choose a new descriptive name or inspect it before reuse.
- If Git reports dubious ownership for the new worktree, add only that worktree path as a `safe.directory`.

## Implementation Flow

1. Fetch the latest base branch.
2. Create a branch and worktree under `.agents/worktrees/`.
3. Do all implementation, formatting, and verification inside the new worktree.
4. Commit changes using `commit-message-quality-check`.
5. Push the branch.
6. Create or update the pull request using `pull-request-quality-check`.

## Verification

Run checks that match the change risk and repository conventions. For this repository, useful checks often include:

```powershell
dotnet build CruiserJumpPractice.sln
```

If verification is skipped, state why in the pull request body.

## Pull Request Notes

- Keep pull request titles and bodies consistent with `pull-request-quality-check`.
- Mention the worktree path only when it helps the user find the local work.
- Remove temporary PR body files from the worktree after creating or editing the pull request.
