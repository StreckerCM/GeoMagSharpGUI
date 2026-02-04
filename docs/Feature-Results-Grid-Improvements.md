# Feature Specification: Results Grid Improvements

## Summary

Enhance the results grid with three improvements:
1. Display uncertainty values for magnetic field components (like NOAA calculator)
2. Add "/yr" suffix to secular variation units
3. Add context menu for copying cell values and entire table

## Current State

### Results Grid Structure

The grid currently displays 8 columns:

| Column | Header | Unit Display |
|--------|--------|--------------|
| Date | "Date" | MM/dd/yyyy |
| Declination | "Declination (+E/-W)" | value + "°" |
| Inclination | "Inclination (+D/-U)" | value + "°" |
| Horizontal Intensity | "Horizontal Intensity" | value + " nT" |
| North Comp | "North Comp (+N/-S)" | value + " nT" |
| East Comp | "East Comp (+E/-W)" | value + " nT" |
| Vertical Comp | "Vertical Comp (+D/-U)" | value + " nT" |
| Total Field | "Total Field" | value + " nT" |

A separate "Change per year" row is appended with secular variation values, but units don't indicate "/yr".

### Missing Features

- No uncertainty/error estimates displayed
- Secular variation units don't indicate per-year nature
- No way to copy data from the grid

---

## Improvement 1: Uncertainty Display

### NOAA WMM Uncertainty Values

Based on WMM2025 documentation, the uncertainty values are:

| Component | Symbol | Uncertainty | Unit | Type |
|-----------|--------|-------------|------|------|
| North (X) | X | 137 | nT | Constant |
| East (Y) | Y | 89 | nT | Constant |
| Down (Z) | Z | 141 | nT | Constant |
| Horizontal | H | 133 | nT | Constant |
| Total Field | F | 138 | nT | Constant |
| Inclination | I | 0.20 | degrees | Constant |
| Declination | D | sqrt(0.26² + (5417/H)²) | degrees | **Variable** |

**Key Point**: Declination uncertainty depends on local horizontal field intensity (H). Near magnetic poles where H is weak, uncertainty becomes very large.

### Uncertainty Formula for Declination

```
σ_D = sqrt(0.26² + (5417/H)²) degrees
```

Where:
- `0.26` degrees is the base declination error
- `5417` nT is derived from error propagation
- `H` is computed horizontal intensity in nT

### Blackout and Caution Zones

| Zone | Condition | Meaning |
|------|-----------|---------|
| **Normal** | H ≥ 6000 nT | Compass reliable, uncertainty reasonable |
| **Caution** | 2000 ≤ H < 6000 nT | Compass less reliable, higher uncertainty |
| **Blackout** | H < 2000 nT | Compass unreliable, uncertainty very high |

### Proposed UI

Add an "Uncertainty (±)" row after the main values row:

```
┌──────────┬────────────┬────────────┬────────────┬────────────┐
│ Date     │ Declination│ Inclination│ Horiz Int  │ Total Field│
├──────────┼────────────┼────────────┼────────────┼────────────┤
│ 2/3/2026 │ -8.1234°   │ 62.5678°   │ 19234.56 nT│ 50123.45 nT│
│ ± Uncert │ ± 0.34°    │ ± 0.20°    │ ± 133 nT   │ ± 138 nT   │
├──────────┼────────────┼────────────┼────────────┼────────────┤
│ Change/yr│ 0.0123°/yr │ -0.0045°/yr│ 12.34 nT/yr│ -5.67 nT/yr│
└──────────┴────────────┴────────────┴────────────┴────────────┘
```

### When to Omit Uncertainty Row

- If model type doesn't have published uncertainty values (non-WMM/IGRF)
- If uncertainty constants are not defined for the loaded model
- Add a `HasUncertaintyData` property to `MagneticModelSet`

### Implementation: Uncertainty Constants

```csharp
// GeoMagSharp/GeoConstants.cs

public static class WMMUncertainty
{
    /// <summary>North component (X) uncertainty in nT</summary>
    public const double X = 137;

    /// <summary>East component (Y) uncertainty in nT</summary>
    public const double Y = 89;

    /// <summary>Down component (Z) uncertainty in nT</summary>
    public const double Z = 141;

    /// <summary>Horizontal intensity (H) uncertainty in nT</summary>
    public const double H = 133;

    /// <summary>Total field (F) uncertainty in nT</summary>
    public const double F = 138;

    /// <summary>Inclination (I) uncertainty in degrees</summary>
    public const double Inclination = 0.20;

    /// <summary>Base declination uncertainty component in degrees</summary>
    public const double DeclinationBase = 0.26;

    /// <summary>Declination uncertainty coefficient in nT</summary>
    public const double DeclinationCoefficient = 5417;

    /// <summary>Calculates declination uncertainty based on horizontal intensity</summary>
    /// <param name="horizontalIntensity">H value in nT</param>
    /// <returns>Uncertainty in degrees</returns>
    public static double CalculateDeclinationUncertainty(double horizontalIntensity)
    {
        if (horizontalIntensity <= 0)
            return double.PositiveInfinity;

        double term = DeclinationCoefficient / horizontalIntensity;
        return Math.Sqrt(DeclinationBase * DeclinationBase + term * term);
    }

    /// <summary>Blackout zone threshold in nT</summary>
    public const double BlackoutThreshold = 2000;

    /// <summary>Caution zone threshold in nT</summary>
    public const double CautionThreshold = 6000;
}
```

### Implementation: MagneticUncertainty Class

```csharp
// GeoMagSharp/Models/Results/MagneticUncertainty.cs

namespace GeoMagSharp
{
    public class MagneticUncertainty
    {
        public double Declination { get; set; }
        public double Inclination { get; set; }
        public double HorizontalIntensity { get; set; }
        public double NorthComp { get; set; }
        public double EastComp { get; set; }
        public double VerticalComp { get; set; }
        public double TotalField { get; set; }

        public bool IsBlackoutZone { get; set; }
        public bool IsCautionZone { get; set; }

        public static MagneticUncertainty Calculate(double horizontalIntensity)
        {
            return new MagneticUncertainty
            {
                Declination = WMMUncertainty.CalculateDeclinationUncertainty(horizontalIntensity),
                Inclination = WMMUncertainty.Inclination,
                HorizontalIntensity = WMMUncertainty.H,
                NorthComp = WMMUncertainty.X,
                EastComp = WMMUncertainty.Y,
                VerticalComp = WMMUncertainty.Z,
                TotalField = WMMUncertainty.F,
                IsBlackoutZone = horizontalIntensity < WMMUncertainty.BlackoutThreshold,
                IsCautionZone = horizontalIntensity >= WMMUncertainty.BlackoutThreshold
                             && horizontalIntensity < WMMUncertainty.CautionThreshold
            };
        }
    }
}
```

---

## Improvement 2: Secular Variation Units ("/yr")

### Current Display

The "Change per year" row shows values without indicating the per-year nature:

```
Change per year │ 0.0123° │ -0.0045° │ 12.34 nT │ ...
```

### Proposed Display

Add "/yr" suffix to all secular variation values:

```
Change per year │ 0.0123°/yr │ -0.0045°/yr │ 12.34 nT/yr │ ...
```

### Implementation

Modify the grid population code in `frmMain.cs`:

```csharp
// Current (lines 250-268)
var changePerYearRow = new string[]
{
    "Change per year",
    lastCalc.Declination.ChangePerYear.ToString("F4") + "°",
    lastCalc.Inclination.ChangePerYear.ToString("F4") + "°",
    ...
};

// Proposed
var changePerYearRow = new string[]
{
    "Change per year",
    lastCalc.Declination.ChangePerYear.ToString("F4") + "°/yr",
    lastCalc.Inclination.ChangePerYear.ToString("F4") + "°/yr",
    lastCalc.HorizontalIntensity.ChangePerYear.ToString("F2") + " nT/yr",
    lastCalc.NorthComp.ChangePerYear.ToString("F2") + " nT/yr",
    lastCalc.EastComp.ChangePerYear.ToString("F2") + " nT/yr",
    lastCalc.VerticalComp.ChangePerYear.ToString("F2") + " nT/yr",
    lastCalc.TotalField.ChangePerYear.ToString("F2") + " nT/yr"
};
```

Also update column headers for consistency:

```csharp
// Column header changes
"Horizontal Intensity (nT)"      // base value
"North Comp (nT)"
// etc.
```

---

## Improvement 3: Context Menu (Copy)

### Proposed Context Menu

Right-click on the DataGridView shows:

```
┌─────────────────────────┐
│ Copy Cell Value    Ctrl+C│
│ Copy Row                 │
│ ────────────────────────│
│ Copy All (Tab-separated)│
│ Copy All (CSV)          │
│ ────────────────────────│
│ Select All         Ctrl+A│
└─────────────────────────┘
```

### Implementation

```csharp
// frmMain.cs - Add context menu

private ContextMenuStrip CreateResultsContextMenu()
{
    var menu = new ContextMenuStrip();

    var copyCell = new ToolStripMenuItem("Copy Cell Value", null, CopyCell_Click);
    copyCell.ShortcutKeys = Keys.Control | Keys.C;

    var copyRow = new ToolStripMenuItem("Copy Row", null, CopyRow_Click);

    var copySeparator = new ToolStripSeparator();

    var copyAllTab = new ToolStripMenuItem("Copy All (Tab-separated)", null, CopyAllTab_Click);

    var copyAllCsv = new ToolStripMenuItem("Copy All (CSV)", null, CopyAllCsv_Click);

    var selectSeparator = new ToolStripSeparator();

    var selectAll = new ToolStripMenuItem("Select All", null, SelectAll_Click);
    selectAll.ShortcutKeys = Keys.Control | Keys.A;

    menu.Items.AddRange(new ToolStripItem[]
    {
        copyCell, copyRow, copySeparator,
        copyAllTab, copyAllCsv, selectSeparator,
        selectAll
    });

    return menu;
}

private void CopyCell_Click(object sender, EventArgs e)
{
    if (dataGridViewResults.CurrentCell != null)
    {
        Clipboard.SetText(dataGridViewResults.CurrentCell.Value?.ToString() ?? "");
    }
}

private void CopyRow_Click(object sender, EventArgs e)
{
    if (dataGridViewResults.CurrentRow != null)
    {
        var values = new List<string>();
        foreach (DataGridViewCell cell in dataGridViewResults.CurrentRow.Cells)
        {
            values.Add(cell.Value?.ToString() ?? "");
        }
        Clipboard.SetText(string.Join("\t", values));
    }
}

private void CopyAllTab_Click(object sender, EventArgs e)
{
    CopyGridToClipboard("\t");
}

private void CopyAllCsv_Click(object sender, EventArgs e)
{
    CopyGridToClipboard(",");
}

private void CopyGridToClipboard(string delimiter)
{
    var sb = new StringBuilder();

    // Headers
    var headers = new List<string>();
    foreach (DataGridViewColumn col in dataGridViewResults.Columns)
    {
        headers.Add(col.HeaderText);
    }
    sb.AppendLine(string.Join(delimiter, headers));

    // Rows
    foreach (DataGridViewRow row in dataGridViewResults.Rows)
    {
        var values = new List<string>();
        foreach (DataGridViewCell cell in row.Cells)
        {
            var value = cell.Value?.ToString() ?? "";
            // Escape for CSV if needed
            if (delimiter == "," && value.Contains(","))
            {
                value = $"\"{value}\"";
            }
            values.Add(value);
        }
        sb.AppendLine(string.Join(delimiter, values));
    }

    Clipboard.SetText(sb.ToString());
}

private void SelectAll_Click(object sender, EventArgs e)
{
    dataGridViewResults.SelectAll();
}
```

### Wire Up Context Menu

```csharp
// In frmMain constructor or InitializeComponent
dataGridViewResults.ContextMenuStrip = CreateResultsContextMenu();
```

---

## Files to Modify

| File | Changes |
|------|---------|
| `GeoMagSharp/GeoConstants.cs` | Add `WMMUncertainty` class with constants |
| `GeoMagSharp/Models/Results/MagneticUncertainty.cs` | New file for uncertainty data |
| `GeoMagSharp/Models/Results/MagneticCalculations.cs` | Add `Uncertainty` property |
| `GeoMagSharp/GeoMagSharp.csproj` | Include new file |
| `GeoMagGUI/frmMain.cs` | Update grid population, add context menu |
| `GeoMagGUI/frmMain.Designer.cs` | Context menu components |

---

## Acceptance Criteria

### Uncertainty Display
- [ ] Uncertainty row displays after main calculation row
- [ ] Declination uncertainty calculated from H value
- [ ] Constant uncertainties shown for other components
- [ ] Uncertainty row omitted for non-WMM/IGRF models
- [ ] Blackout zone warning when H < 2000 nT
- [ ] Caution zone indication when 2000 ≤ H < 6000 nT

### Unit Suffixes
- [ ] All secular variation values show "/yr" suffix
- [ ] Angle units: "°/yr"
- [ ] Field intensity units: "nT/yr"
- [ ] Export also includes updated units

### Context Menu
- [ ] Right-click shows context menu
- [ ] "Copy Cell Value" copies current cell
- [ ] "Copy Row" copies entire row (tab-separated)
- [ ] "Copy All (Tab-separated)" copies entire grid
- [ ] "Copy All (CSV)" copies as comma-separated values
- [ ] Ctrl+C works for cell copy
- [ ] Ctrl+A selects all

---

## Visual Mockup

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ Results                                                                      │
├──────────┬────────────────┬────────────────┬──────────────┬─────────────────┤
│ Date     │ Declination    │ Inclination    │ Horiz Int    │ Total Field     │
│          │ (+E/-W)        │ (+D/-U)        │ (nT)         │ (nT)            │
├──────────┼────────────────┼────────────────┼──────────────┼─────────────────┤
│ 2/3/2026 │ -8.1234°       │ 62.5678°       │ 19234.56     │ 50123.45        │
│ ± Uncert │ ± 0.34°        │ ± 0.20°        │ ± 133        │ ± 138           │
├──────────┼────────────────┼────────────────┼──────────────┼─────────────────┤
│ Change/yr│ 0.0123°/yr     │ -0.0045°/yr    │ 12.34 nT/yr  │ -5.67 nT/yr     │
└──────────┴────────────────┴────────────────┴──────────────┴─────────────────┘

⚠ Caution: Low horizontal intensity (H = 4500 nT). Compass accuracy reduced.
```

---

## Related Issues

- Issue #6: Split DataModel.cs (completed - provides `Models/Results/` folder)
- Issue #10: Centralize constants (completed - `GeoConstants.cs` exists)
