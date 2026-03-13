# CSV and JSON Export — Design Spec

**Issue:** #26
**Date:** 2026-03-12

## Goal

Replace the legacy tab-delimited text export with CSV and JSON formats, including metadata, calculation results, secular variation, and ISCWSA uncertainty when available.

## Architecture

Export logic lives entirely in the GUI project as a new `ResultsExporter` static class. The GUI passes calculation results, options, and the model name to the exporter — no changes to the GeoMagSharp library. The library's `SaveResultsAsync` is no longer called; the GUI handles all file writing.

Two new class-level fields are added to `FrmMain` to preserve data across the calculate→save boundary:
- `private CalculationOptions _lastCalculationOptions;` — set in `buttonCalculate_Click` after building `calcOptions`
- `private string _lastModelName;` — set in `buttonCalculate_Click` from `selectedModel.Name`

## Design

### Files changed

| File | Change |
|------|--------|
| `GeoMagGUI/ResultsExporter.cs` | **New** — static class with `ExportCsvAsync` and `ExportJsonAsync` methods |
| `GeoMagGUI/frmMain.cs` | **Modified** — add `_lastCalculationOptions` and `_lastModelName` fields; update `buttonCalculate_Click` to store them; update `saveToolStripMenuItem_Click` to use new exporter |
| `GeoMagGUI/Properties/Resources.resx` | **Modified** — add file filter strings for CSV and JSON |
| `GeoMagGUI/Properties/Resources.Designer.cs` | **Auto-generated** — updated by resource tooling |

### Save dialog

The `SaveFileDialog` filter changes from:

```
Text File (Tab delimited) (*.txt)|*.txt
```

To:

```
CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json
```

The exporter method is selected based on the file extension of the chosen filename. CSV is the default (first in filter list).

### Data sources

Data comes from the `_MagCalculator` instance and two new class-level fields on `FrmMain`:

- **Results:** `_MagCalculator.ResultsOfCalculation` — `IEnumerable<MagneticCalculations>`, one entry per date
- **Options:** `_lastCalculationOptions` — a new `FrmMain` field storing the `CalculationOptions` from the last calculation (latitude, longitude, elevation via `AltitudeInKm`, date range). Set in `buttonCalculate_Click` right after building `calcOptions`.
- **Model name:** `_lastModelName` — a new `FrmMain` field storing `selectedModel.Name` from the last calculation. Set in `buttonCalculate_Click`.
- **Secular variation:** `ResultsOfCalculation.Last()` — each `MagneticValue` has a `ChangePerYear` property
- **Uncertainty:** `ResultsOfCalculation.Last().Uncertainty` — nullable `GeomagneticUncertainty`. Read `Declination`, `DipAngle` (output as `inclination`), and `TotalField`.

### CSV format

RFC 4180 compliant for data rows. Metadata is prepended as `#` comment lines (non-standard extension; CSV parsers that don't support comments will see them as data rows, but this is an acceptable trade-off for including useful context). All numeric values use `CultureInfo.InvariantCulture` to ensure dot decimal separator regardless of system locale.

```csv
# Model: WMM2025
# Latitude: 41.3098100
# Longitude: -81.3322900
# Elevation: 0.0000 km
Date,Declination (deg),Inclination (deg),Horizontal Intensity (nT),North Comp (nT),East Comp (nT),Vertical Comp (nT),Total Field (nT)
2026-03-12,-8.3532,67.4122,19981.70,19769.70,-2902.80,48031.80,52022.30
Change Per Year,0.0007,-0.0933,33.50,33.20,-4.60,-140.10,-116.50
Uncertainty (1σ),0.3000,0.1600,,,,,107.00
```

**Column details:**
- Date column: ISO 8601 format (`yyyy-MM-dd`) for data rows; label text for summary rows
- Angular values (Declination, Inclination): 4 decimal places
- Intensity values (all nT columns): 2 decimal places
- Uncertainty row: only Declination, Inclination, TotalField have values; others are empty (consecutive commas)
- Uncertainty row omitted entirely when `Uncertainty` is null
- Secular variation uses `ChangePerYear` from the last result (same as the existing tab-delimited export uses `First()` — we'll match by using `Last()` consistent with the grid display)

**Quoting:** Values containing commas, quotes, or newlines are wrapped in double quotes per RFC 4180. In practice, only the metadata comment lines and column headers contain such characters, but the exporter handles it generically.

### JSON format

Clean nested structure using Newtonsoft.Json (already a project dependency). All numeric values written with `CultureInfo.InvariantCulture`.

```json
{
  "model": "WMM2025",
  "version": "1.3.0",
  "latitude": 41.30981,
  "longitude": -81.33229,
  "elevation": {
    "value": 0.0,
    "units": "km"
  },
  "results": [
    {
      "date": "2026-03-12",
      "declination": -8.3532,
      "inclination": 67.4122,
      "horizontalIntensity": 19981.70,
      "northComp": 19769.70,
      "eastComp": -2902.80,
      "verticalComp": 48031.80,
      "totalField": 52022.30
    }
  ],
  "secularVariation": {
    "declination": 0.0007,
    "inclination": -0.0933,
    "horizontalIntensity": 33.50,
    "northComp": 33.20,
    "eastComp": -4.60,
    "verticalComp": -140.10,
    "totalField": -116.50
  },
  "uncertainty": {
    "source": "ISCWSA",
    "sigma": 1,
    "declination": 0.3000,
    "inclination": 0.1600,
    "totalField": 107.00
  },
  "units": {
    "declination": "degrees",
    "inclination": "degrees",
    "horizontalIntensity": "nT",
    "northComp": "nT",
    "eastComp": "nT",
    "verticalComp": "nT",
    "totalField": "nT"
  }
}
```

**JSON details:**
- `model`: `_lastModelName` value (already set without extension by `MagneticModelSet`). Use as-is (no `.ToUpperInvariant()` — the model files use uppercase names natively).
- `version`: `Assembly.GetExecutingAssembly().GetName().Version.ToString(3)` (produces `"1.3.0"`)
- `latitude`/`longitude`: decimal degrees, 5+ decimal places (raw double from `_lastCalculationOptions`)
- `elevation`: object with `value` = `_lastCalculationOptions.AltitudeInKm` and `units` = `"km"` (always normalized to km regardless of user's input unit)
- `results`: array of objects, one per calculated date. Date is ISO 8601 string.
- `secularVariation`: single object from the last result's `ChangePerYear` values. Present for all calculations.
- `uncertainty`: single object, only present when `Uncertainty` is non-null. Source property `GeomagneticUncertainty.DipAngle` is output as `inclination` (GUI naming convention for consistency with the rest of the JSON).
- `units`: describes the unit for each field name. Always present.
- Pretty-printed with `Formatting.Indented` for human readability.

### ResultsExporter class

```csharp
public static class ResultsExporter
{
    public static async Task ExportCsvAsync(
        string fileName,
        IEnumerable<MagneticCalculations> results,
        CalculationOptions options,
        string modelName,
        CancellationToken cancellationToken = default);

    public static async Task ExportJsonAsync(
        string fileName,
        IEnumerable<MagneticCalculations> results,
        CalculationOptions options,
        string modelName,
        CancellationToken cancellationToken = default);
}
```

Both methods:
- Build the output string on the calling thread (fast)
- Write to file via `Task.Run(() => File.WriteAllText(fileName, content))` (.NET Framework 4.8 does not have `File.WriteAllTextAsync`)
- Use `CancellationToken` for cancellation support (checked before writing)
- Use `CultureInfo.InvariantCulture` for all numeric formatting
- Handle file-locked and file-exists scenarios (same pattern as existing `SaveResultsAsync`)

### frmMain.cs changes

**New fields** (add near existing `_isSaving` field):
```csharp
private CalculationOptions _lastCalculationOptions;
private string _lastModelName;
```

**In `buttonCalculate_Click`**, after building `calcOptions` and before calling `MagneticCalculationsAsync`:
```csharp
_lastCalculationOptions = calcOptions;
_lastModelName = selectedModel.Name;
```

**The `saveToolStripMenuItem_Click` method** changes to:
1. Show `SaveFileDialog` with CSV|JSON filter
2. Determine format from file extension (`.csv` or `.json`)
3. Call `ResultsExporter.ExportCsvAsync(...)` or `ResultsExporter.ExportJsonAsync(...)`
4. Pass `_MagCalculator.ResultsOfCalculation`, `_lastCalculationOptions`, and `_lastModelName`

The existing call to `_MagCalculator.SaveResultsAsync(fldlg.FileName)` is removed.

### Secular variation source

The existing library export uses `ResultsOfCalculation.First()` for secular variation. The grid display uses `ResultsOfCalculation.Last()`. Both produce the same values when there's a single result. For date ranges, `Last()` is more semantically correct (secular variation at the end of the range). The new exporters will use `Last()` to match the grid.

## Scope

- New file: `ResultsExporter.cs` (~150-200 lines)
- Modified: `frmMain.cs` — 2 new fields, 2 lines in calculate handler, save handler rewrite (~25 lines changed)
- Modified: `Resources.resx` (2 new string resources)
- No GeoMagSharp library changes
- No new NuGet dependencies (Newtonsoft.Json already referenced)

## Out of Scope

- Importing/reading CSV or JSON files
- Configurable export options (metadata on/off, column selection)
- Clipboard copy (deferred to issue #25)
- Batch export of multiple locations
