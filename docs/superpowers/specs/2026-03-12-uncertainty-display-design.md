# Display Uncertainty Values in Results Grid — Design Spec

**Issue:** #48
**Date:** 2026-03-12

## Goal

Add a summary row to the results DataGridView showing ISCWSA 1-sigma geomagnetic uncertainty values from GeoMagSharp 1.5.0, giving users immediate visibility into the confidence of their calculations.

## Architecture

The existing `MagneticCalculations` result object already exposes a `GeomagneticUncertainty Uncertainty` property (added in GeoMagSharp 1.5.0). No new library calls are needed — we read `_MagCalculator.ResultsOfCalculation.Last().Uncertainty` and display it in a new summary row appended after the existing "Change per year" row.

## Design

### What changes

**Single file modified:** `GeoMagGUI/frmMain.cs`

After the "Change per year" summary row (lines ~299–323), add a new "Uncertainty (1σ)" summary row with:

| Grid Column | Display Value | Data Source | Format |
|-------------|---------------|-------------|--------|
| ColumnDate | `Uncertainty (1σ)` | Label | — |
| ColumnDeclination | `±X.XXXX°` | `Uncertainty.Declination` | F4 |
| ColumnInclination | `±X.XXXX°` | `Uncertainty.DipAngle` | F4 |
| ColumnHorizontalIntensity | `—` | No ISCWSA value | — |
| ColumnNorthComp | `—` | No ISCWSA value | — |
| ColumnEastComp | `—` | No ISCWSA value | — |
| ColumnVerticalComp | `—` | No ISCWSA value | — |
| ColumnTotalField | `±X.XX nT` | `Uncertainty.TotalField` | F2 |

### Field mapping note

The grid column "Inclination" maps to `Uncertainty.DipAngle` — these are the same physical quantity (magnetic dip angle). The GeoMagSharp library uses the ISCWSA term "DipAngle" while the GUI uses the geophysics convention "Inclination".

### Visual treatment

- Every cell in the row gets `Style.BackColor = LightGoldenrodYellow` individually (including the label cell and em-dash cells), matching the cell-by-cell pattern used by the "Change per year" row
- Prefix uncertainty values with `±` (literal character in C# string, consistent with existing use of `°` in format strings)
- Columns without ISCWSA values show an em dash (`—`) rather than blank

### Conditional display

- The uncertainty block is placed inside the existing results guard (`if ResultsOfCalculation != null && Any()`), after the "Change per year" row
- Wrapped in `if (_MagCalculator.ResultsOfCalculation.Last().Uncertainty != null)` — only adds the row when uncertainty data exists
- Models with `GeomagneticModelCategory.Unknown` and no `ModelCategoryOverride` will have null uncertainty — no row is shown
- This means legacy/unsupported models gracefully degrade with no visible change

### What does NOT change

- No new columns added to the grid
- No changes to the grid control type (stays `DataGridView`)
- No changes to calculation logic or GeoMagSharp API calls
- No new dependencies
- No changes to file save/export (uncertainty row is display-only)
- Issue #25 (Results Grid Improvements) remains separate for future work

## Scope

- ~30 lines of new code in `frmMain.cs`
- Pattern follows the existing "Change per year" row exactly (same style of cell-by-cell assignment with colored background)
- No breaking changes, no new files, no new UI controls

## Out of Scope

- Displaying `BhDependentDec` (requires dividing by Bh, which would need horizontal intensity — can be added later)
- Displaying `DepthAzimuthUncertainty` (depth-dependent, not relevant for surface calculations)
- Displaying `Revision` or `ModelCategory` metadata
- Grid layout improvements (deferred to issue #25)
- Export/save of uncertainty values
