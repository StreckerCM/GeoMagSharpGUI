# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GeoMagSharpGUI is a Windows desktop application for geomagnetic field calculations using spherical harmonic models (IGRF, WMM, EMM). It provides a GUI for calculating magnetic declination, inclination, and field strength at any location and date.

**Tech Stack:** .NET Framework 4.0 WinForms application (C#), x86 platform

## Build Commands

```bash
# Release build
msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"

# Debug build
msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"

# Run unit tests
vstest.console.exe GeoMagSharp-UnitTests\bin\Debug\GeoMagSharp-UnitTests.dll
```

## Session Start Protocol

At the start of every session:
1. Read `docs/features/ACTIVE_WORK.md` if it exists
2. Check auto memory (`MEMORY.md`) for prior context
3. Run `git log --oneline -10` and `git status` to understand current state
4. Ask the user what they'd like to work on before making assumptions

Before ending a session or when context is getting long:
1. Update `docs/features/ACTIVE_WORK.md` with current progress, decisions made, and next steps
2. Save any important patterns or learnings to auto memory

## Branching Strategy

4-branch flow: `feature/*` -> `development` -> `preview` -> `master`

| Branch | Purpose | Version Format | Workflow |
|--------|---------|----------------|----------|
| `master` | Production releases | `X.Y.Z` | `production-release.yml` |
| `preview` | Pre-release testing | `X.Y.Z-preview.N` | `preview-release.yml` |
| `development` | Integration | `X.Y.Z-dev.N` | `build.yml` |
| `feature/*` | Development work | `X.Y.Z-dev.N` | `build.yml` |

Feature branches are created from `development`. PRs flow: `feature/*` -> `development` -> `preview` -> `master`.

### Branch Protection Rules

- **NEVER** commit directly to `master`, `preview`, or `development`. All changes via PRs only.
- **NEVER** force-push to protected branches.
- **NEVER** create or merge a PR without explicit user confirmation. Draft PRs are acceptable without confirmation.
- All development work happens on `feature/*` branches (only place direct commits are allowed).

## Development Workflow

**MANDATORY:** Every `feature/*` branch MUST use Ralph Loop (`/ralph-loop`) with rotating personas before any code is written.

**Before writing ANY code on a feature branch:**
1. Ensure a GitHub issue exists
2. Create `docs/features/<feature>/tasks.md` with task breakdown
3. Start a Ralph Loop with rotating persona pattern

See `docs/prompts/` for full Ralph Loop documentation and persona definitions.

## Platform Constraints

- x86 architecture (set in project files)
- Windows-only (.NET Framework 4.0)
- Requires Visual Studio Developer Command Prompt for builds

## Extended Documentation

For detailed information, read these files on demand (not loaded every session):

- **Architecture & Conventions:** `@AGENTS.md` -- Solution structure, coding conventions, key classes
- **Ralph Loop:** `@docs/prompts/README.md` -- Iterative development workflow with rotating personas
- **Personas:** `@docs/prompts/PERSONAS.md` -- 11 development personas for Ralph Loop rotation
- **Feature Templates:** `@docs/prompts/templates/ROTATING_FEATURE.md` -- Ralph Loop prompt templates
