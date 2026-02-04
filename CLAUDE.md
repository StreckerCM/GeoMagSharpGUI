# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GeoMagSharpGUI is a Windows desktop application for geomagnetic field calculations using spherical harmonic models (IGRF, WMM, EMM). It provides a GUI for calculating magnetic declination, inclination, and field strength at any location and date.

**Tech Stack:** .NET Framework 4.0 WinForms application (C#), Visual Studio

For detailed project structure, architecture, coding conventions, and build commands, see `AGENTS.md`.

## Build Commands

```bash
# Debug build
msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"

# Release build
msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"

# Run unit tests
vstest.console.exe GeoMagSharp-UnitTests\bin\Debug\GeoMagSharp-UnitTests.dll
```

## Branching Strategy

### Branch Protection Rules — NEVER VIOLATE

- **NEVER commit directly to `master`.** All changes to `master` must come through reviewed and approved PRs.
- **NEVER push directly to protected branches.** No force-pushes, no direct commits, no exceptions.
- **NEVER create or merge a PR without explicit user confirmation.** Always ask the user before creating a PR and before merging one. Draft PRs are acceptable without confirmation, but converting to ready-for-review or merging requires approval.
- **All development work happens on `feature/*` branches.** This is the only place where direct commits are allowed.

## Development Workflow

### MANDATORY: Ralph Loop for ALL Feature Work

**This is NON-NEGOTIABLE.** Every feature branch (`feature/*`) MUST use the Ralph Wiggum loop (`/ralph-loop`) with rotating personas. There are NO exceptions to this rule, regardless of how simple the task appears.

**Before writing ANY code on a feature branch, you MUST:**

1. Verify a `docs/features/<feature>/tasks.md` file exists
2. If it doesn't exist, create one before proceeding
3. Start a Ralph Loop with the rotating persona pattern

**If you find yourself on a feature branch writing code without an active Ralph Loop, STOP and start one.**

### Step 1: Create a GitHub Issue (if one doesn't exist)

Every feature must have a corresponding GitHub issue before work begins.

### Step 2: Create and Switch to a Feature Branch

```bash
git checkout master
git pull origin master
git checkout -b feature/<issue-number>-<short-description>
```

### Step 3: Create or Verify tasks.md (GATE - Required Before Any Code)

Every feature MUST have a `docs/features/<feature>/tasks.md` file. This file is the **single source of truth** for what work needs to be done. No code should be written until this file exists and has been reviewed.

**tasks.md format:**
```markdown
# Feature: <Feature Name>
Issue: #<issue-number>
Branch: feature/<issue-number>-<short-description>

## Tasks
- [ ] Task 1 description
- [ ] Task 2 description
- [ ] Task 3 description

## Completion Criteria
- [ ] All tasks checked
- [ ] Build succeeds
- [ ] Tests pass
- [ ] 2 clean Ralph Loop cycles
```

### Step 4: Start a Ralph Loop (MANDATORY)

Use the Ralph Wiggum loop with the rotating persona pattern defined in `docs/prompts/PERSONAS.md`. See the "Ralph Loop / Iterative Development" section below for the full pattern and completion criteria.

## Ralph Loop / Iterative Development

**MANDATORY — NO EXCEPTIONS:** ALL feature branch work (`feature/*`) MUST use Ralph loops with rotating personas. This applies regardless of feature size, complexity, or urgency. Skipping the Ralph Loop is never acceptable.

### Pre-Flight Checklist (Before Starting Ralph Loop)

Before starting any Ralph Loop, verify:

- [ ] GitHub issue exists for this feature
- [ ] Feature branch created
- [ ] `docs/features/<feature>/tasks.md` exists with task breakdown
- [ ] PR created (draft is fine) to track work

If any of these are missing, create them first. **Do NOT start coding without tasks.md.**

### Required Persona Rotation

```
Iteration % 6 determines the current persona:

[0] #5 IMPLEMENTER   - Complete tasks, write code
[1] #9 REVIEWER      - Review for bugs, code quality
[2] #7 TESTER        - Verify functionality, add tests
[3] #3 UI_UX_DESIGNER - Review UI/UX, accessibility
[4] #10 SECURITY     - Security review, input validation
[5] #2 PROJECT_MGR   - Check requirements, update tasks
```

### Each Iteration Must:
1. Identify the current persona based on iteration number
2. Follow that persona's mindset and output format from `docs/prompts/PERSONAS.md`
3. Commit with persona prefix: `[IMPLEMENTER]`, `[REVIEWER]`, etc.
4. Reference the feature's `tasks.md` file and mark tasks complete
5. Post a PR comment summarizing findings and changes

### Completion Criteria
- All tasks in `docs/features/[feature]/tasks.md` marked complete
- Build succeeds with no errors
- Tests pass
- **2 clean cycles** (all 6 personas find no issues twice)

### Why This Matters

The Ralph Loop ensures:
- Multiple perspectives review every change (code quality, security, UX, testing)
- Issues are caught early through systematic rotation
- Progress is tracked via tasks.md
- An audit trail exists via PR comments from each persona
- Features meet a consistent quality bar before merging

See `docs/prompts/README.md` and `docs/prompts/templates/ROTATING_FEATURE.md` for full documentation.
