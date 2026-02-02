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

**IMPORTANT:** When working on any feature or issue, follow these steps:

### 1. Create a GitHub Issue (if one doesn't exist)

Every feature must have a corresponding GitHub issue before work begins. This ensures proper tracking and documentation.

### 2. Create and Switch to a Feature Branch

Create a new branch from `preview` before starting any work:

```bash
git checkout preview
git pull origin preview
git checkout -b feature/<issue-number>-<short-description>
```

### 3. Start a Ralph Loop with Personas

Use the Ralph Wiggum loop (`/ralph-loop`) with the rotating persona pattern defined in `docs/prompts/PERSONAS.md`. This ensures comprehensive development coverage through multiple perspectives.

See the "Ralph Loop / Iterative Development" section below for the specific persona rotation pattern and completion criteria.

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

**IMPORTANT:** When using Ralph loops (`/ralph-loop`) for feature development, **always use the rotating persona pattern** defined in `docs/prompts/PERSONAS.md`.

### Required Pattern

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

### Completion Criteria
- All tasks in `docs/features/[feature]/tasks.md` marked complete
- Build succeeds with no errors
- Tests pass
- **2 clean cycles** (all 6 personas find no issues twice)

See `docs/prompts/README.md` and `docs/prompts/templates/ROTATING_FEATURE.md` for full documentation.
