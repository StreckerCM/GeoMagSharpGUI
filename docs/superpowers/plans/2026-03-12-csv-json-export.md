# CSV and JSON Export Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the legacy tab-delimited text export with CSV and JSON export formats, including metadata, calculation results, secular variation, and ISCWSA uncertainty.

**Architecture:** New `ResultsExporter` static class in the GUI project handles all export logic. Two new class-level fields (`_lastCalculationOptions`, `_lastModelName`) on `FrmMain` preserve data across the calculate→save boundary. The library's `SaveResultsAsync` is no longer called.

**Tech Stack:** C# / .NET Framework 4.8 / WinForms / Newtonsoft.Json 13.x

**Note:** This project has no unit tests (tests live in the GeoMagSharp repo). Verification is build + manual run.

**Spec:** `docs/superpowers/specs/2026-03-12-csv-json-export-design.md`

---

## Chunk 1: ResultsExporter and frmMain Changes

### Task 1: Add Newtonsoft.Json PackageReference

Newtonsoft.Json is currently only a transitive dependency via GeoMagSharp. The new `ResultsExporter` class uses it directly, so it needs an explicit PackageReference.

**Files:**
- Modify: `GeoMagGUI/GeoMagGUI.csproj:138-142`

- [ ] **Step 1: Add the PackageReference**

In `GeoMagGUI/GeoMagGUI.csproj`, add a new PackageReference for Newtonsoft.Json inside the existing `<ItemGroup>` that contains GeoMagSharp (lines 138-142). The result should be:

```xml
  <ItemGroup>
    <PackageReference Include="GeoMagSharp">
      <Version>1.5.0</Version>
    </PackageReference>
    <PackageReference Include="Newtonsoft.Json">
      <Version>13.0.3</Version>
    </PackageReference>
  </ItemGroup>
```

- [ ] **Step 2: Restore and build**

Run:
```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Restore -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse && "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Build -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse
```

Expected: Build succeeds with 0 errors.

- [ ] **Step 3: Commit**

```bash
git add GeoMagGUI/GeoMagGUI.csproj
git commit -m "chore: add explicit Newtonsoft.Json PackageReference (#26)"
```

---

### Task 2: Add class-level fields to FrmMain

Store `CalculationOptions` and model name so the save handler can access them.

**Files:**
- Modify: `GeoMagGUI/frmMain.cs:886` (add fields near `_isSaving`)
- Modify: `GeoMagGUI/frmMain.cs:259` (store values in calculate handler)

- [ ] **Step 1: Add the two new fields**

In `GeoMagGUI/frmMain.cs`, immediately after line 886 (`private bool _isSaving;`), add:

```csharp
        private CalculationOptions _lastCalculationOptions;
        private string _lastModelName;
```

- [ ] **Step 2: Store values in buttonCalculate_Click**

In `GeoMagGUI/frmMain.cs`, after line 257 (`_MagCalculator.LoadModel(selectedModel);`) and before line 259 (`if (toolStripMenuItemUseRangeOfDates.Checked)`), insert:

```csharp
                    _lastCalculationOptions = calcOptions;
                    _lastModelName = selectedModel.Name;
```

- [ ] **Step 3: Build**

Run:
```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Build -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse
```

Expected: Build succeeds with 0 errors.

- [ ] **Step 4: Commit**

```bash
git add GeoMagGUI/frmMain.cs
git commit -m "refactor: store calculation options and model name as class fields (#26)"
```

---

### Task 3: Create ResultsExporter with CSV export

**Files:**
- Create: `GeoMagGUI/ResultsExporter.cs`

- [ ] **Step 1: Create ResultsExporter.cs**

Create `GeoMagGUI/ResultsExporter.cs` with the following content:

```csharp
using GeoMagSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoMagGUI
{
    public static class ResultsExporter
    {
        public static async Task ExportCsvAsync(
            string fileName,
            IEnumerable<MagneticCalculations> results,
            CalculationOptions options,
            string modelName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultsList = results.ToList();
            var last = resultsList.Last();
            var ci = CultureInfo.InvariantCulture;
            var sb = new StringBuilder();

            // Metadata comment lines
            sb.AppendLine(string.Format(ci, "# Model: {0}", modelName));
            sb.AppendLine(string.Format(ci, "# Latitude: {0:F7}", options.Latitude));
            sb.AppendLine(string.Format(ci, "# Longitude: {0:F7}", options.Longitude));
            sb.AppendLine(string.Format(ci, "# Elevation: {0:F4} km", options.AltitudeInKm));

            // Column header
            sb.AppendLine("Date,Declination (deg),Inclination (deg),Horizontal Intensity (nT),North Comp (nT),East Comp (nT),Vertical Comp (nT),Total Field (nT)");

            // Data rows
            foreach (var mag in resultsList)
            {
                sb.AppendLine(string.Format(ci,
                    "{0},{1},{2},{3},{4},{5},{6},{7}",
                    mag.Date.ToString("yyyy-MM-dd"),
                    mag.Declination.Value.ToString("F4", ci),
                    mag.Inclination.Value.ToString("F4", ci),
                    mag.HorizontalIntensity.Value.ToString("F2", ci),
                    mag.NorthComp.Value.ToString("F2", ci),
                    mag.EastComp.Value.ToString("F2", ci),
                    mag.VerticalComp.Value.ToString("F2", ci),
                    mag.TotalField.Value.ToString("F2", ci)));
            }

            // Secular variation row (from last result)
            sb.AppendLine(string.Format(ci,
                "Change Per Year,{0},{1},{2},{3},{4},{5},{6}",
                last.Declination.ChangePerYear.ToString("F4", ci),
                last.Inclination.ChangePerYear.ToString("F4", ci),
                last.HorizontalIntensity.ChangePerYear.ToString("F2", ci),
                last.NorthComp.ChangePerYear.ToString("F2", ci),
                last.EastComp.ChangePerYear.ToString("F2", ci),
                last.VerticalComp.ChangePerYear.ToString("F2", ci),
                last.TotalField.ChangePerYear.ToString("F2", ci)));

            // Uncertainty row (only when available)
            if (last.Uncertainty != null)
            {
                var u = last.Uncertainty;
                sb.AppendLine(string.Format(ci,
                    "Uncertainty (1\u03C3),{0},{1},,,,,{2}",
                    u.Declination.ToString("F4", ci),
                    u.DipAngle.ToString("F4", ci),
                    u.TotalField.ToString("F2", ci)));
            }

            var content = sb.ToString();
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() => File.WriteAllText(fileName, content));
        }

        public static async Task ExportJsonAsync(
            string fileName,
            IEnumerable<MagneticCalculations> results,
            CalculationOptions options,
            string modelName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultsList = results.ToList();
            var last = resultsList.Last();
            var ci = CultureInfo.InvariantCulture;
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            var root = new JObject
            {
                ["model"] = modelName,
                ["version"] = version,
                ["latitude"] = options.Latitude,
                ["longitude"] = options.Longitude,
                ["elevation"] = new JObject
                {
                    ["value"] = options.AltitudeInKm,
                    ["units"] = "km"
                }
            };

            // Results array
            var resultsArray = new JArray();
            foreach (var mag in resultsList)
            {
                resultsArray.Add(new JObject
                {
                    ["date"] = mag.Date.ToString("yyyy-MM-dd"),
                    ["declination"] = Math.Round(mag.Declination.Value, 4),
                    ["inclination"] = Math.Round(mag.Inclination.Value, 4),
                    ["horizontalIntensity"] = Math.Round(mag.HorizontalIntensity.Value, 2),
                    ["northComp"] = Math.Round(mag.NorthComp.Value, 2),
                    ["eastComp"] = Math.Round(mag.EastComp.Value, 2),
                    ["verticalComp"] = Math.Round(mag.VerticalComp.Value, 2),
                    ["totalField"] = Math.Round(mag.TotalField.Value, 2)
                });
            }
            root["results"] = resultsArray;

            // Secular variation (from last result)
            root["secularVariation"] = new JObject
            {
                ["declination"] = Math.Round(last.Declination.ChangePerYear, 4),
                ["inclination"] = Math.Round(last.Inclination.ChangePerYear, 4),
                ["horizontalIntensity"] = Math.Round(last.HorizontalIntensity.ChangePerYear, 2),
                ["northComp"] = Math.Round(last.NorthComp.ChangePerYear, 2),
                ["eastComp"] = Math.Round(last.EastComp.ChangePerYear, 2),
                ["verticalComp"] = Math.Round(last.VerticalComp.ChangePerYear, 2),
                ["totalField"] = Math.Round(last.TotalField.ChangePerYear, 2)
            };

            // Uncertainty (only when available)
            if (last.Uncertainty != null)
            {
                var u = last.Uncertainty;
                root["uncertainty"] = new JObject
                {
                    ["source"] = "ISCWSA",
                    ["sigma"] = 1,
                    ["declination"] = Math.Round(u.Declination, 4),
                    ["inclination"] = Math.Round(u.DipAngle, 4),
                    ["totalField"] = Math.Round(u.TotalField, 2)
                };
            }

            // Units metadata
            root["units"] = new JObject
            {
                ["declination"] = "degrees",
                ["inclination"] = "degrees",
                ["horizontalIntensity"] = "nT",
                ["northComp"] = "nT",
                ["eastComp"] = "nT",
                ["verticalComp"] = "nT",
                ["totalField"] = "nT"
            };

            var content = root.ToString(Formatting.Indented);
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() => File.WriteAllText(fileName, content));
        }
    }
}
```

**Key details for the implementer:**
- `CultureInfo.InvariantCulture` ensures dot decimal separators on all locales
- `default(CancellationToken)` is used instead of `default` for .NET Framework 4.8 compatibility
- `\u03C3` = σ (sigma) in the CSV uncertainty label
- `GeomagneticUncertainty.DipAngle` maps to `inclination` in the output
- `AltitudeInKm` normalizes elevation to km regardless of user's input unit
- CSV uncertainty row has consecutive commas for the 4 empty columns (HorizontalIntensity, NorthComp, EastComp, VerticalComp) — note the format string has `,,,,,` (5 commas) between `{1}` (DipAngle) and `{2}` (TotalField) to produce 4 empty fields
- JSON uses `Math.Round` to control decimal places; JObject serializes doubles with `CultureInfo.InvariantCulture` by default

- [ ] **Step 2: Add ResultsExporter.cs to the csproj**

This project uses a classic-style csproj (not SDK-style), so all `.cs` files must be explicitly listed. In `GeoMagGUI/GeoMagGUI.csproj`, add a `<Compile>` entry after line 95 (`<Compile Include="Program.cs" />`):

```xml
    <Compile Include="ResultsExporter.cs" />
```

- [ ] **Step 3: Build**

Run:
```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Build -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse
```

Expected: Build succeeds with 0 errors.

- [ ] **Step 4: Commit**

```bash
git add GeoMagGUI/ResultsExporter.cs GeoMagGUI/GeoMagGUI.csproj
git commit -m "feat: add ResultsExporter with CSV and JSON export (#26)"
```

---

### Task 4: Add resource strings and update save handler

**Files:**
- Modify: `GeoMagGUI/Properties/Resources.resx:120-122` (replace old filter, add new ones)
- Modify: `GeoMagGUI/frmMain.cs:888-927` (rewrite save handler)

- [ ] **Step 1: Update Resources.resx**

In `GeoMagGUI/Properties/Resources.resx`, replace the existing `File_Type_Text_Tab` entry (lines 120-122):

```xml
  <data name="File_Type_Text_Tab" xml:space="preserve">
    <value>Text File  (Tab delimited) (*.txt)|*.txt</value>
  </data>
```

With two new entries:

```xml
  <data name="File_Type_CSV" xml:space="preserve">
    <value>CSV Files (*.csv)|*.csv</value>
  </data>
  <data name="File_Type_JSON" xml:space="preserve">
    <value>JSON Files (*.json)|*.json</value>
  </data>
```

- [ ] **Step 1b: Update Resources.Designer.cs**

This project uses a classic-style csproj with `ResXFileCodeGenerator`, which only runs at design time in Visual Studio — NOT during command-line MSBuild builds. You must manually update `GeoMagGUI/Properties/Resources.Designer.cs`.

Replace the `File_Type_Text_Tab` property (lines 90-97):

```csharp
        /// <summary>
        ///   Looks up a localized string similar to Text File  (Tab delimited) (*.txt)|*.txt.
        /// </summary>
        internal static string File_Type_Text_Tab {
            get {
                return ResourceManager.GetString("File_Type_Text_Tab", resourceCulture);
            }
        }
```

With two new properties:

```csharp
        /// <summary>
        ///   Looks up a localized string similar to CSV Files (*.csv)|*.csv.
        /// </summary>
        internal static string File_Type_CSV {
            get {
                return ResourceManager.GetString("File_Type_CSV", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to JSON Files (*.json)|*.json.
        /// </summary>
        internal static string File_Type_JSON {
            get {
                return ResourceManager.GetString("File_Type_JSON", resourceCulture);
            }
        }
```

- [ ] **Step 2: Rewrite the save handler**

In `GeoMagGUI/frmMain.cs`, replace the entire `saveToolStripMenuItem_Click` method (lines 888-927) with:

```csharp
        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isSaving || _MagCalculator == null) return;

            var fldlg = new SaveFileDialog
            {
                Filter = string.Format("{0}|{1}", Resources.File_Type_CSV, Resources.File_Type_JSON),
                Title = "Save Results",
                FileName = "Results"
            };

            if (fldlg.ShowDialog() == DialogResult.OK)
            {
                _isSaving = true;
                try
                {
                    buttonCalculate.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    UseWaitCursor = true;
                    toolStripStatusLabel1.Text = "Saving results...";

                    var ext = Path.GetExtension(fldlg.FileName).ToLowerInvariant();
                    if (ext == ".json")
                    {
                        await ResultsExporter.ExportJsonAsync(
                            fldlg.FileName,
                            _MagCalculator.ResultsOfCalculation,
                            _lastCalculationOptions,
                            _lastModelName);
                    }
                    else
                    {
                        await ResultsExporter.ExportCsvAsync(
                            fldlg.FileName,
                            _MagCalculator.ResultsOfCalculation,
                            _lastCalculationOptions,
                            _lastModelName);
                    }

                    SetStatusTemporary("Results saved");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: Saving Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "Error saving";
                }
                finally
                {
                    buttonCalculate.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    UseWaitCursor = false;
                    _isSaving = false;
                }
            }
        }
```

**Key details for the implementer:**
- The filter string is built by joining `File_Type_CSV` and `File_Type_JSON` with `|` — this produces `CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json`
- CSV is the first filter (default)
- File extension determines the export method — `.json` uses JSON, everything else defaults to CSV
- The `using System.IO;` directive is already present in `frmMain.cs` (line 5)
- The error handling pattern (try/catch/finally) is preserved from the original

- [ ] **Step 3: Build**

Run:
```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Build -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse
```

Expected: Build succeeds with 0 errors. If the build shows errors about `File_Type_Text_Tab` not existing, search for other usages in the codebase and remove them.

- [ ] **Step 4: Commit**

```bash
git add GeoMagGUI/Properties/Resources.resx GeoMagGUI/Properties/Resources.Designer.cs GeoMagGUI/frmMain.cs
git commit -m "feat: replace tab-delimited export with CSV and JSON (#26)"
```

---

### Task 5: Manual verification

- [ ] **Step 1: Run the application**

Run: `GeoMagGUI\bin\Debug\GeoMagGUI.exe`

- [ ] **Step 2: Test CSV export**

1. Select **WMM2025** model
2. Enter coordinates: Latitude `41.30981`, Longitude `-81.33229`
3. Click **Calculate**
4. Click **File → Save** (or the save toolbar button)
5. Verify the Save dialog shows **CSV Files (*.csv)** as the default filter
6. Save as `test-results.csv`
7. Open the file and verify:
   - `#` comment lines with Model, Latitude, Longitude, Elevation
   - Column header row
   - One data row with date and 7 numeric values
   - "Change Per Year" row
   - "Uncertainty (1σ)" row with Declination, Inclination, and TotalField values (em dashes columns should be empty commas)

- [ ] **Step 3: Test JSON export**

1. With the same results, click **File → Save** again
2. Switch the filter dropdown to **JSON Files (*.json)**
3. Save as `test-results.json`
4. Open the file and verify:
   - Pretty-printed JSON with model, version, latitude, longitude, elevation
   - `results` array with one entry
   - `secularVariation` object
   - `uncertainty` object with source, sigma, declination, inclination, totalField
   - `units` object

- [ ] **Step 4: Test with date range**

1. Enable **Use Range of Dates**
2. Set a start date and end date spanning multiple dates
3. Click **Calculate**
4. Export as CSV — verify multiple data rows, one "Change Per Year" row, one optional uncertainty row
5. Export as JSON — verify `results` array has multiple entries, `secularVariation` is a single object

- [ ] **Step 5: Test with model that has no uncertainty**

1. Select a model that does NOT produce uncertainty (e.g., an older IGRF model if available)
2. Calculate and export as CSV — verify no "Uncertainty (1σ)" row
3. Export as JSON — verify no `uncertainty` object

- [ ] **Step 6: Clean up test files**

Delete `test-results.csv` and `test-results.json` from disk.
