# Repository Guidelines

## Project Structure & Module Organization

`GeoMagGUI.sln` orchestrates three projects:

| Project | Type | Framework | Platform | Purpose |
|---------|------|-----------|----------|---------|
| **GeoMagGUI** | WinForms App | .NET 4.0 Client | x86 | User interface |
| **GeoMagSharp** | Class Library | .NET 4.0 | AnyCPU | Core calculations |
| **GeoMagSharp-UnitTests** | MSTest | .NET 4.5.2 | AnyCPU | Unit tests |

**Key Directories:**
- `GeoMagGUI/coefficient/` - Magnetic model files (.COF, .DAT) and MagneticModels.json
- `GeoMagGUI/assets/` - Icons and images
- `GeoMagSharp/` - All calculation logic and data models
- `docs/prompts/` - Claude personas and prompt templates
- `docs/features/` - Feature specifications and plans

**Design Principle:** Keep UI code lightweight; shared logic belongs in the GeoMagSharp library. New data files should live beside existing `.COF` files to flow through copy-to-output rules.

---

## Build & Test Commands

Use Developer Command Prompt for Visual Studio. Typical workflows:

```bash
# Debug build
msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"

# Release build
msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"

# Run unit tests
vstest.console.exe GeoMagSharp-UnitTests\bin\Debug\GeoMagSharp-UnitTests.dll
```

**Build Verification:** Always run `msbuild` after changes to verify compilation. Run tests after modifying anything in `GeoMagSharp`.

---

## Coding Style & Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Types | PascalCase | `MagneticModelSet` |
| Methods | PascalCase | `SpotCalculation()` |
| Properties | PascalCase | `Declination` |
| Local variables | camelCase | `decimalYear` |
| Parameters | camelCase | `latitude` |
| Private fields | _camelCase | `_modelCollection` |
| Constants | PascalCase | `MaxDeg` |

**UI Forms:** Retain historical `frm*` prefix (`frmMain`, `frmPreferences`). Keep designer files untouched by hand edits.

**General Rules:**
- Four-space indentation
- Use `Newtonsoft.Json` (already referenced) for serialization
- Leverage existing `ExtensionMethods` and `Helper` utilities before creating new ones
- Use XML documentation comments for public APIs
- Prefer explicit types over `var` for clarity in complex code

---

## Key Classes & Architecture

### GeoMagSharp Library

| Class | Purpose |
|-------|---------|
| `GeoMag` | Main calculation orchestrator - `LoadModel()`, `MagneticCalculations()`, `SaveResults()` |
| `Calculator` | Spherical harmonic calculations - `SpotCalculation()`, `GetField()` |
| `ModelReader` | Parses .COF and .DAT coefficient files |
| `DataModel` | All data structures, enums, and model classes |
| `Units` | Unit conversion utilities |
| `ExtensionMethods` | DateTime and numeric extensions |
| `GeoMagException` | Custom exception hierarchy |

### Data Models

| Class | Purpose |
|-------|---------|
| `Preferences` | User settings (coordinate format, units) - JSON serializable |
| `CalculationOptions` | Input parameters for calculations |
| `MagneticCalculations` | Output results with secular variation |
| `MagneticModelSet` | Coefficient data for a model |
| `MagneticModelCollection` | Collection of available models |
| `Latitude`, `Longitude` | Coordinate classes with DMS conversion |

---

## Testing Guidelines

All unit tests reside in `GeoMagSharp-UnitTests`, organized per feature.

**Naming Convention:** `MethodUnderTest_State_Outcome`

```csharp
[TestMethod]
public void ToDecimal_LeapYear_CalculatesCorrectly()
{
    // Arrange
    DateTime input = new DateTime(2020, 7, 1);

    // Act
    decimal result = input.ToDecimal();

    // Assert
    Assert.AreEqual(2020.5m, result, 0.01m);
}
```

**Test Coverage Priority:**
1. `Calculator` methods - numerical accuracy is critical
2. `ModelReader` - file parsing edge cases
3. `ExtensionMethods` - date/number conversions
4. `DataModel` - serialization round-trips

**Tolerance:** Use appropriate tolerances for floating-point comparisons (typically 1e-6 for calculations).

---

## Commit & Pull Request Guidelines

**Commit Messages:**
- Concise, action-led style: "Add EMM reader support"
- Limit subject lines to ~70 characters
- Group related edits per commit

**PR Requirements:**
1. Summarize intent and link tracking issues
2. List build/test commands executed
3. Include screenshots for UI changes
4. Verify:
   - Solution builds in Debug and Release
   - Dependencies remain in `packages/`
   - Coefficient files load correctly

---

## Extension Methods Reference

**DateTime Extensions:**
- `ToDecimal()` - Convert to decimal year (accounts for leap years)
- `ToDateTime()` - Convert decimal year back to DateTime
- `IsValidYear()` - Validate year range (1900-9999)

**Numeric Extensions:**
- `ToDegree()` - Radians to degrees
- `ToRadian()` - Degrees to radians
- `Truncate()` - Precision truncation

**String Extensions:**
- `CheckStringForModel()` - Detect model type from string

---

## Common Enumerations

```csharp
enum CoordinateSystem { Geodetic = 1, Geocentric = 2 }
enum Algorithm { BGS = 1, NOAA = 2, MAGVAR = 3 }
enum MagneticFieldUnit { NanoTesla = 1, Gauss = 2 }
enum knownModels { NONE = 0, DGRF = 1, EMM = 2, IGRF = 3, WMM = 4 }
```

---

## File Format Reference

### COF File Format (Coefficient File)
- Fixed 80-character record length
- Header with model name, epoch, valid date range
- Coefficients in Gauss format
- Max degree typically 13

### MagneticModels.json
- JSON array of model metadata
- Properties: Name, Type, FileNames, MinDate, MaxDate

### Preferences.json
- User settings stored in application directory
- Includes coordinate format, elevation type, field units

---

## Security Considerations

- **Input Validation:** Validate coordinate ranges (-90 to 90 lat, -180 to 180 long)
- **File Access:** Use safe file reading with proper exception handling
- **No Hardcoded Paths:** Use relative paths from application directory

---

## Claude Code Integration

For AI-assisted development:

1. **Personas:** See `docs/prompts/PERSONAS.md` for 11 development personas
2. **Templates:** See `docs/prompts/templates/` for Ralph Wiggum loop templates
3. **Features:** Track planned work in `docs/features/`

**Recommended Workflow:**
```bash
# Feature implementation with rotating personas
/ralph-loop "Feature: [name]
ROTATING PERSONA (ITERATION MOD 6):
[0] #5 IMPLEMENTER: Complete tasks
[1] #9 REVIEWER: Review code
[2] #7 TESTER: Verify functionality
[3] #3 UI_UX_DESIGNER: Check UI/UX
[4] #10 SECURITY_AUDITOR: Security review
[5] #2 PROJECT_MANAGER: Check requirements

OUTPUT <promise>FEATURE COMPLETE</promise> when done.
" --completion-promise "FEATURE COMPLETE" --max-iterations 30
```
