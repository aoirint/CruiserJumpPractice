---
name: docker-quality-check
description: Quality-check Dockerfile and Docker Compose changes in this repository. Use when editing Dockerfile, docker-compose.yml files, container startup scripts, Docker image CI, or Docker-related documentation.
---

# Docker Quality Check

Use this skill together with `code-quality-check`: this skill defines Docker-specific commands,
pinning details, and scope decisions, while `code-quality-check` defines shared readability,
verification-discipline, comment, and supply-chain expectations.

## When to Use

- Use this skill when editing Dockerfiles, Docker Compose files, container startup scripts, Docker
  image CI, or Docker-related documentation.
- Use this skill before committing or preparing PR verification notes for Docker-related changes.

## Goals

- Verify Docker behavior with the smallest meaningful Docker command for the changed surface.
- Keep Docker and Compose examples reproducible and easy to review.
- Apply repository supply-chain expectations to Docker-related downloads, binaries, images, and
  GitHub Actions.
- Record skipped Docker checks with a concrete blocker instead of a generic "not run" note.

## Workflow

1. Identify whether the change affects Dockerfile linting, image builds, Compose configuration,
   startup behavior, CI installation, or documentation only.
2. Apply the relevant tool and pinning policy below.
3. Run the narrowest Docker-specific check that exercises the changed behavior.
4. If startup behavior changed, run the service and a minimal health check, then shut it down.
5. Summarize which checks ran, which passed, and why any relevant Docker check was skipped.

## Tool Policy

- Use `hadolint` for Dockerfile linting when a Dockerfile exists.
- Use `docker build` or `docker compose build` for executable validation when Dockerfile behavior changes.
- Use `docker compose config` for Compose samples.
- Apply the repository supply-chain baseline from `code-quality-check` to Docker-related binaries
  and GitHub Actions.
- Pin downloaded CI binaries by version and SHA-256. Do not use floating download URLs.
- Pin GitHub Actions by full commit SHA with a version comment.

## Check Commands

From the repository root, choose checks that match the changed files:

```bash
hadolint Dockerfile
docker build -t <image-name>:test .
docker compose -f <compose-file> config
```

If Compose startup behavior changed, also run the relevant service and a minimal health check:

```bash
docker compose -f <compose-file> up -d <service>
docker compose -f <compose-file> exec <service> python -c "import sys; sys.exit(0)"
docker compose -f <compose-file> down
```

## CI Tool Installation Pattern

When installing `hadolint` in GitHub Actions, match the existing pinned-tool pattern:

```bash
curl -sSfLO https://github.com/hadolint/hadolint/releases/download/v2.14.0/hadolint-linux-x86_64
echo "6bf226944684f56c84dd014e8b979d27425c0148f61b3bd99bcc6f39e9dc5a47  hadolint-linux-x86_64" | sha256sum -c -
install -m 0755 hadolint-linux-x86_64 "$RUNNER_TEMP/bin/hadolint"
```

Before changing this version, verify the release is at least 7 days old.
