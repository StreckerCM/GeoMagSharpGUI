# Feature Specification: CSV Export Format

## Summary

Replace the current tab-separated text file export with standard CSV (Comma-Separated Values) format for better compatibility with spreadsheet applications and data analysis tools.

## Current State

### Current Export Format

The current export in `GeoMag.SaveResults()` produces a tab-separated text file:

```
MODEL NAME: WMM2025

Latitude: 38.8977000
Longitude: -77.0365000
Altitude: 100.0 ft

Date                     	Declination (+E/W)       	Inclination (+D/-U)      	Horizontal Intensity     	...
                         	deg                      	deg                      	nT                       	...

02/03/2026               	-10.123                  	62.456                   	19234.56                 	...
Change Per year          	0.012                    	-0.005                   	12.34                    	...
```

### Problems with Current Format

1. **Not standard CSV** - Uses tabs and fixed-width columns
2. **Metadata mixed with data** - Model name, coordinates in same file as results
3. **Inconsistent structure** - Hard for automated tools to parse
4. **No header row for data section** - Unit row separated from column headers
5. **File extension mismatch** - Saved as `.txt` despite tabular structure

---

## Proposed Solution

### Standard RFC 4180 CSV Format

Create a proper CSV file following the RFC 4180 standard:

```csv
Date,Declination (deg),Inclination (deg),Horizontal Intensity (nT),North Comp (nT),East Comp (nT),Vertical Comp (nT),Total Field (nT)
2026-02-03,-10.1234,62.4567,19234.56,18123.45,3421.23,38765.43,50123.45
```

### Export Options

Provide two export modes:

1. **Data Only** - Just the results table
2. **Full Report** - Metadata + results in structured format

### Data Only Export (.csv)

```csv
Date,Declination (deg),Declination Change (deg/yr),Inclination (deg),Inclination Change (deg/yr),Horizontal Intensity (nT),Horizontal Intensity Change (nT/yr),North Comp (nT),North Comp Change (nT/yr),East Comp (nT),East Comp Change (nT/yr),Vertical Comp (nT),Vertical Comp Change (nT/yr),Total Field (nT),Total Field Change (nT/yr)
2026-02-03,-10.1234,0.0123,62.4567,-0.0045,19234.56,12.34,18123.45,10.23,3421.23,5.67,38765.43,-8.90,50123.45,-5.67
2026-02-04,-10.1233,0.0123,62.4566,-0.0045,19234.68,12.34,18123.57,10.23,3421.28,5.67,38765.35,-8.90,50123.40,-5.67
```

### Full Report Export (.csv)

```csv
# GeoMag Calculation Results
# Generated: 2026-02-03T15:30:00Z
# Model: WMM2025
# Model Date Range: 2025.0 - 2030.0

# Input Parameters
Parameter,Value,Unit
Latitude,38.8977000,degrees
Longitude,-77.0365000,degrees
Elevation,100.0,ft
Elevation Type,Altitude,
Calculation Date,2026-02-03,

# Results
Date,Declination (deg),Declination Change (deg/yr),Inclination (deg),Inclination Change (deg/yr),Horizontal Intensity (nT),Horizontal Intensity Change (nT/yr),North Comp (nT),North Comp Change (nT/yr),East Comp (nT),East Comp Change (nT/yr),Vertical Comp (nT),Vertical Comp Change (nT/yr),Total Field (nT),Total Field Change (nT/yr)
2026-02-03,-10.1234,0.0123,62.4567,-0.0045,19234.56,12.34,18123.45,10.23,3421.23,5.67,38765.43,-8.90,50123.45,-5.67
```

Note: Lines starting with `#` are comments (supported by many CSV tools but ignored by others).

---

## Implementation Details

### SaveResultsCsv Method

```csharp
// GeoMagSharp/GeoMag.cs

public bool SaveResultsCsv(string filePath, bool includeMetadata = false)
{
    if (string.IsNullOrEmpty(filePath)) return false;
    if (ResultsOfCalculation == null || !ResultsOfCalculation.Any()) return false;

    try
    {
        using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            if (includeMetadata)
            {
                WriteMetadataSection(sw);
            }

            WriteResultsSection(sw);
        }
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"SaveResultsCsv Error: {ex}");
        return false;
    }
}

private void WriteMetadataSection(StreamWriter sw)
{
    sw.WriteLine("# GeoMag Calculation Results");
    sw.WriteLine($"# Generated: {DateTime.UtcNow:O}");
    sw.WriteLine($"# Model: {CurrentModel?.Name ?? "Unknown"}");
    sw.WriteLine($"# Model Date Range: {CurrentModel?.MinDate:F1} - {CurrentModel?.MaxDate:F1}");
    sw.WriteLine();
    sw.WriteLine("# Input Parameters");
    sw.WriteLine("Parameter,Value,Unit");
    sw.WriteLine($"Latitude,{_CalculationOptions.Latitude:F7},degrees");
    sw.WriteLine($"Longitude,{_CalculationOptions.Longitude:F7},degrees");

    var elevation = _CalculationOptions.GetElevation;
    sw.WriteLine($"Elevation,{elevation[1]},{elevation[2]}");
    sw.WriteLine($"Elevation Type,{elevation[0]},");
    sw.WriteLine($"Calculation Date,{_CalculationOptions.StartDate:yyyy-MM-dd},");
    sw.WriteLine();
    sw.WriteLine("# Results");
}

private void WriteResultsSection(StreamWriter sw)
{
    // Header row
    sw.WriteLine(string.Join(",", new[]
    {
        "Date",
        "Declination (deg)",
        "Declination Change (deg/yr)",
        "Inclination (deg)",
        "Inclination Change (deg/yr)",
        "Horizontal Intensity (nT)",
        "Horizontal Intensity Change (nT/yr)",
        "North Comp (nT)",
        "North Comp Change (nT/yr)",
        "East Comp (nT)",
        "East Comp Change (nT/yr)",
        "Vertical Comp (nT)",
        "Vertical Comp Change (nT/yr)",
        "Total Field (nT)",
        "Total Field Change (nT/yr)"
    }));

    // Data rows
    foreach (var calc in ResultsOfCalculation)
    {
        sw.WriteLine(string.Join(",", new[]
        {
            calc.Date.ToString("yyyy-MM-dd"),
            calc.Declination.Value.ToString("F4"),
            calc.Declination.ChangePerYear.ToString("F4"),
            calc.Inclination.Value.ToString("F4"),
            calc.Inclination.ChangePerYear.ToString("F4"),
            calc.HorizontalIntensity.Value.ToString("F2"),
            calc.HorizontalIntensity.ChangePerYear.ToString("F2"),
            calc.NorthComp.Value.ToString("F2"),
            calc.NorthComp.ChangePerYear.ToString("F2"),
            calc.EastComp.Value.ToString("F2"),
            calc.EastComp.ChangePerYear.ToString("F2"),
            calc.VerticalComp.Value.ToString("F2"),
            calc.VerticalComp.ChangePerYear.ToString("F2"),
            calc.TotalField.Value.ToString("F2"),
            calc.TotalField.ChangePerYear.ToString("F2")
        }));
    }
}
```

### UI Changes (frmMain.cs)

Update the Save dialog to offer CSV format:

```csharp
private void saveResultsToolStripMenuItem_Click(object sender, EventArgs e)
{
    using (var saveDialog = new SaveFileDialog())
    {
        saveDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        saveDialog.DefaultExt = "csv";
        saveDialog.Title = "Export Results";
        saveDialog.FileName = $"GeoMag_Results_{DateTime.Now:yyyyMMdd}";

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            var extension = Path.GetExtension(saveDialog.FileName).ToLower();

            if (extension == ".csv")
            {
                // Ask about metadata
                var includeMetadata = MessageBox.Show(
                    "Include metadata (model info, coordinates) in export?",
                    "Export Options",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes;

                _MagCalculator.SaveResultsCsv(saveDialog.FileName, includeMetadata);
            }
            else
            {
                // Legacy text format
                _MagCalculator.SaveResults(saveDialog.FileName);
            }
        }
    }
}
```

### Alternative: Export Options Dialog

For more control, create an export options dialog:

```
┌─────────────────────────────────────────────────────────────┐
│                    Export Results                           │
├─────────────────────────────────────────────────────────────┤
│ Format:                                                     │
│   ○ CSV (recommended)                                       │
│   ○ Tab-separated text (legacy)                            │
│                                                             │
│ CSV Options:                                                │
│   ☑ Include metadata header                                │
│   ☐ Include uncertainty values (if available)              │
│   ☑ Include secular variation columns                      │
│                                                             │
│ Date Format:                                                │
│   ○ ISO 8601 (2026-02-03)                                  │
│   ○ US format (02/03/2026)                                 │
│   ○ European format (03/02/2026)                           │
│                                                             │
│                              [Cancel]  [Export]             │
└─────────────────────────────────────────────────────────────┘
```

---

## CSV Specification Details

### RFC 4180 Compliance

| Rule | Implementation |
|------|----------------|
| Fields separated by commas | ✓ |
| Records separated by CRLF | ✓ (StreamWriter default on Windows) |
| Optional header row | ✓ (always included) |
| Double-quote escaping | ✓ (for values containing commas) |
| Embedded quotes escaped | ✓ (double the quote character) |

### Value Formatting

| Data Type | Format | Example |
|-----------|--------|---------|
| Date | ISO 8601 | `2026-02-03` |
| Angles | F4 (4 decimal places) | `-10.1234` |
| Field values | F2 (2 decimal places) | `19234.56` |
| Coordinates | F7 (7 decimal places) | `38.8977000` |

### Escaping Logic

```csharp
private string EscapeCsvValue(string value)
{
    if (string.IsNullOrEmpty(value))
        return "";

    // Need to quote if contains comma, quote, or newline
    if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
    {
        // Escape internal quotes by doubling them
        value = value.Replace("\"", "\"\"");
        return $"\"{value}\"";
    }

    return value;
}
```

---

## Files to Modify

| File | Changes |
|------|---------|
| `GeoMagSharp/GeoMag.cs` | Add `SaveResultsCsv()` method |
| `GeoMagGUI/frmMain.cs` | Update save dialog filter, add format selection |
| `GeoMagGUI/Resources.Designer.cs` | Add filter string resource |

---

## Backward Compatibility

- Keep existing `SaveResults()` method for legacy text format
- Default to CSV but allow text export
- Existing workflows can continue using `.txt` format

---

## Acceptance Criteria

- [ ] Export produces valid RFC 4180 CSV format
- [ ] CSV opens correctly in Excel, Google Sheets, LibreOffice
- [ ] Header row contains all column names with units
- [ ] Each result row on single line
- [ ] Values properly escaped (commas, quotes)
- [ ] Date format is ISO 8601 (sortable)
- [ ] Optional metadata section with model info
- [ ] File extension defaults to `.csv`
- [ ] Legacy `.txt` export still available
- [ ] Secular variation values included
- [ ] Numeric precision matches NOAA calculator

---

## Testing

### Manual Testing

1. Export to CSV, open in Excel - verify all columns parse correctly
2. Export to CSV, open in Google Sheets - verify import
3. Export with metadata, verify header comments
4. Export large date range (100+ rows) - verify performance
5. Export with special characters in model name - verify escaping

### Unit Tests

```csharp
[TestMethod]
public void SaveResultsCsv_ValidResults_CreatesValidCsv()
{
    // Setup calculation with known results
    _geoMag.MagneticCalculations();

    var tempFile = Path.GetTempFileName() + ".csv";
    try
    {
        _geoMag.SaveResultsCsv(tempFile);

        var lines = File.ReadAllLines(tempFile);
        Assert.IsTrue(lines.Length >= 2); // Header + at least 1 data row

        // Verify header
        Assert.IsTrue(lines[0].StartsWith("Date,"));

        // Verify data row format
        var fields = lines[1].Split(',');
        Assert.AreEqual(15, fields.Length); // All columns present
    }
    finally
    {
        File.Delete(tempFile);
    }
}

[TestMethod]
public void SaveResultsCsv_WithMetadata_IncludesModelInfo()
{
    _geoMag.MagneticCalculations();

    var tempFile = Path.GetTempFileName() + ".csv";
    try
    {
        _geoMag.SaveResultsCsv(tempFile, includeMetadata: true);

        var content = File.ReadAllText(tempFile);
        Assert.IsTrue(content.Contains("# Model:"));
        Assert.IsTrue(content.Contains("Latitude,"));
    }
    finally
    {
        File.Delete(tempFile);
    }
}
```

---

## Related Issues

- Results Grid Improvements (parallel effort for consistent data format)
- Async Operations (consider async file writing)
