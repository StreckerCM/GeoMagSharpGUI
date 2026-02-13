# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GeoMagSharpGUI is a Windows desktop application for calculating geomagnetic field values using spherical harmonic models. It provides a graphical interface for computing magnetic declination, inclination, and field intensity at any location and date.

**Tech Stack:** .NET Framework 4.0 WinForms application (C#), x86 platform

## Build Commands

```bash
# Debug build (x86)
msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"

# Release build (x86)
msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"

# Run unit tests
vstest.console.exe GeoMagSharp-UnitTests\bin\Debug\GeoMagSharp-UnitTests.dll

# Run the application (after build)
GeoMagGUI\bin\Debug\GeoMagGUI.exe
```

Note: Use Developer Command Prompt for Visual Studio. This is a legacy .NET Framework project.

## Branching Strategy

```
master ←──────── Stable releases
  ↑
  │ PR merge
  │
preview ←─────── Pre-release testing and development
  ↑
  │ PR merge
  │
feature/* ─────── Feature development work
```

### Branch Guidelines

| Branch | Purpose | Description |
|--------|---------|-------------|
| `master` | Production releases | Stable release builds |
| `preview` | Development | Integration testing before release |
| `feature/*` | Feature work | Development branches for new features |

### Workflow
1. Create feature branches from `preview`
2. PR feature branches to `preview` for integration
3. PR `preview` to `master` for releases
4. See `docs/RELEASE_PROCESS.md` for detailed release instructions

### Branch Protection Rules — NEVER VIOLATE

- **NEVER commit directly to `master`.** All changes to `master` must come through reviewed and approved PRs from `preview`.
- **NEVER commit directly to `preview`.** All changes to `preview` must come through PRs from `feature/*` branches.
- **NEVER push directly to protected branches.** No force-pushes, no direct commits, no exceptions.
- **NEVER create or merge a PR without explicit user confirmation.** Always ask the user before creating a PR and before merging one. Draft PRs are acceptable without confirmation, but converting to ready-for-review or merging requires approval.
- **All development work happens on `feature/*` branches.** This is the only place where direct commits are allowed.

## Architecture

### Solution Structure

- **GeoMagGUI** (`GeoMagGUI/`) - WinForms application (.NET Framework 4.0 Client)
- **GeoMagSharp** (`GeoMagSharp/`) - Core calculation library (.NET Framework 4.0)
- **GeoMagSharp-UnitTests** (`GeoMagSharp-UnitTests/`) - MSTest unit tests (.NET Framework 4.5.2)

### Key Source Files

| File | Purpose |
|------|--------|
| `GeoMagSharp/GeoMag.cs` | Main calculation orchestrator |
| `GeoMagSharp/Calculator.cs` | Spherical harmonic calculations |
| `GeoMagSharp/ModelReader.cs` | Coefficient file parser (.COF, .DAT) |
| `GeoMagSharp/DataModel.cs` | Data structures and model classes |
| `GeoMagSharp/Units.cs` | Unit conversion utilities |
| `GeoMagGUI/frmMain.cs` | Main application window |
| `GeoMagGUI/frmPreferences.cs` | User preferences dialog |
| `GeoMagGUI/frmAddModel.cs` | Add model dialog |

### Data Directories

| Directory | Purpose |
|-----------|--------|
| `GeoMagGUI/coefficient/` | Magnetic model files (.COF, .DAT) and MagneticModels.json |
| `GeoMagGUI/assets/` | Icons and images |
| `GeoMagGUI/documentation/` | License and docs |

### Supported Magnetic Models

- **IGRF** (International Geomagnetic Reference Field)
- **WMM** (World Magnetic Model)
- **DGRF** (Definitive Geomagnetic Reference Field)
- **EMM** (Enhanced Magnetic Model)

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
git checkout preview
git pull origin preview
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

## Key Patterns

- WinForms with minimal code-behind
- Keep calculation logic in GeoMagSharp library
- UI code in GeoMagGUI stays lightweight
- JSON configuration via Newtonsoft.Json
- Coefficient files in fixed 80-character record format
- Use existing `ExtensionMethods` and `Helper` utilities

## Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Forms | `frm*` prefix | `frmMain`, `frmPreferences` |
| Types | PascalCase | `MagneticModelSet` |
| Methods | PascalCase | `SpotCalculation()` |
| Private fields | _camelCase | `_modelCollection` |
| Parameters | camelCase | `latitude` |

## Platform Constraints

- .NET Framework 4.0 required
- Windows-only (WinForms)
- x86 platform target
- Visual Studio 2019 or later recommended

## Dependencies

| Package | Purpose |
|---------|---------|
| Newtonsoft.Json | JSON serialization |
| System.Device | GPS location services (Windows) |

## Ralph Loop / Iterative Development

**MANDATORY — NO EXCEPTIONS:** ALL feature branch work (`feature/*`) MUST use Ralph loops with rotating personas. This applies regardless of feature size, complexity, or urgency. Skipping the Ralph Loop is never acceptable.

### Pre-Flight Checklist (Before Starting Ralph Loop)

Before starting any Ralph Loop, verify:

- [ ] GitHub issue exists for this feature
- [ ] Feature branch created from `preview`
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
