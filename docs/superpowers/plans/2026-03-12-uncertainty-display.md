# Display Uncertainty Values Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a summary row to the results DataGridView showing ISCWSA 1-sigma geomagnetic uncertainty values.

**Architecture:** Read `_MagCalculator.ResultsOfCalculation.Last().Uncertainty` after the existing "Change per year" row and display a new `LightGoldenrodYellow` summary row with ± values for Declination, DipAngle, and TotalField. No new files, no new dependencies.

**Tech Stack:** C# / .NET Framework 4.8 / WinForms DataGridView

**Note:** This project has no unit tests (tests live in the GeoMagSharp repo). Verification is build + manual run.

**Spec:** `docs/superpowers/specs/2026-03-12-uncertainty-display-design.md`

---

## Chunk 1: Add Uncertainty Summary Row

### Task 1: Add uncertainty summary row to results grid

**Files:**
- Modify: `GeoMagGUI/frmMain.cs:323-325` (insert between end of "Change per year" row and `saveToolStripMenuItem.Enabled = true`)

- [ ] **Step 1: Add the uncertainty row code**

Insert the following block in `GeoMagGUI/frmMain.cs` immediately after line 323 (the last `LightBlue` BackColor assignment for ColumnTotalField) and before line 325 (`saveToolStripMenuItem.Enabled = true`):

```csharp
                    // Uncertainty summary row (ISCWSA 1-sigma) — only shown when uncertainty data exists
                    var lastUncertainty = _MagCalculator.ResultsOfCalculation.Last().Uncertainty;
                    if (lastUncertainty != null)
                    {
                        dataGridViewResults.Rows.Add();
                        var uncertaintyRow = dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1];

                        uncertaintyRow.Cells["ColumnDate"].Value = "Uncertainty (1\u03C3)";
                        uncertaintyRow.Cells["ColumnDate"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnDeclination"].Value = string.Format("\u00B1{0}\u00B0", lastUncertainty.Declination.ToString("F4"));
                        uncertaintyRow.Cells["ColumnDeclination"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnInclination"].Value = string.Format("\u00B1{0}\u00B0", lastUncertainty.DipAngle.ToString("F4"));
                        uncertaintyRow.Cells["ColumnInclination"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnHorizontalIntensity"].Value = "\u2014";
                        uncertaintyRow.Cells["ColumnHorizontalIntensity"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnNorthComp"].Value = "\u2014";
                        uncertaintyRow.Cells["ColumnNorthComp"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnEastComp"].Value = "\u2014";
                        uncertaintyRow.Cells["ColumnEastComp"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnVerticalComp"].Value = "\u2014";
                        uncertaintyRow.Cells["ColumnVerticalComp"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;

                        uncertaintyRow.Cells["ColumnTotalField"].Value = string.Format("\u00B1{0} nT", lastUncertainty.TotalField.ToString("F2"));
                        uncertaintyRow.Cells["ColumnTotalField"].Style.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    }

```

**Key details for the implementer:**
- `\u03C3` = σ (sigma), `\u00B1` = ± (plus-minus), `\u00B0` = ° (degree), `\u2014` = — (em dash)
- Unicode escapes are used instead of literal characters to avoid encoding issues
- The local variable `uncertaintyRow` avoids repeating `dataGridViewResults.Rows[dataGridViewResults.Rows.Count - 1]` on every line — this is a style improvement over the existing "Change per year" block but functionally equivalent
- `lastUncertainty.Declination` maps to the grid's Declination column
- `lastUncertainty.DipAngle` maps to the grid's Inclination column (same physical quantity, different naming convention — see spec)
- `lastUncertainty.TotalField` maps to the grid's TotalField column
- The other 4 columns (HorizontalIntensity, NorthComp, EastComp, VerticalComp) have no ISCWSA values, so they show an em dash

- [ ] **Step 2: Build and verify**

Run:
```bash
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" GeoMagGUI.sln -t:Build -p:Configuration=Debug -p:Platform=x86 -v:minimal -noAutoResponse
```

Expected: Build succeeds with 0 errors.

- [ ] **Step 3: Manual verification**

Run: `GeoMagGUI\bin\Debug\GeoMagGUI.exe`

Test:
1. Select a supported model (WMM2025 or IGRF13)
2. Enter valid coordinates and date
3. Click Calculate
4. Verify the results grid shows:
   - Data rows (white background)
   - "Change per year" row (LightBlue background)
   - "Uncertainty (1σ)" row (LightGoldenrodYellow background) with ± values for Declination, Inclination, and TotalField, and em dashes for the other 4 columns
5. Select an unsupported/legacy model if available — verify no uncertainty row appears

- [ ] **Step 4: Commit**

```bash
git add GeoMagGUI/frmMain.cs
git commit -m "feat: display ISCWSA uncertainty summary row in results grid (#48)"
```
