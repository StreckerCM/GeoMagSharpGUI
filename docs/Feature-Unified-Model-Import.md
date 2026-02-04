# Feature Specification: Unified Model Import with Auto-Detection

## Summary

Replace the current "Add Model" and "Load Model" menu options with a single "Import Model" feature that automatically detects whether a coefficient file can be used standalone or requires a companion file.

## Current State

The application currently has two separate workflows for loading magnetic models:

| Operation | Description | Implementation |
|-----------|-------------|----------------|
| **Add Model** | Opens `frmAddModel` dialog, parses file via `ModelReader`, stores metadata in JSON | Persists model configuration |
| **Load Model** | Copies `.COF/.DAT` file to model folder without parsing | No metadata storage |

This creates confusion for users who don't understand the distinction between these operations.

## Proposed Solution

### Single "Import Model" Button

Replace both menu items with a unified "Import Model" option that:

1. Opens file selection dialog (supporting `.COF` and `.DAT` files)
2. Parses the file to detect coefficient types present
3. Auto-detects if the model is complete or requires companion file
4. Prompts for companion file if needed
5. Validates the combined model can perform calculations
6. Stores the model configuration persistently

### Coefficient Type Detection

Magnetic models contain three types of coefficients:

| Type | Name | Purpose | Required for |
|------|------|---------|--------------|
| **M** | Main | Base magnetic field values | Basic calculations |
| **S** | Secular Variation | Annual change rates | Secular variation display |
| **E** | External | External field contributions | Extended accuracy |

### Auto-Detection Logic

```
1. Parse selected file with ModelReader
2. Analyze coefficient types present:
   - Has M coefficients? (required)
   - Has S coefficients? (optional but recommended)
   - Has E coefficients? (optional)
3. If M only (no S):
   a. Check if companion file exists (same base name)
   b. If not, prompt user: "This model lacks secular variation coefficients.
      Would you like to select a companion file with SV data?"
   c. Allow proceeding without SV (with warning about reduced accuracy)
4. Validate minimum requirements met (at least M coefficients)
5. Store combined model
```

### Calculation Behavior Without S Coefficients

Based on current `MagneticModelSet.GetIntExt()` implementation:
- **YES**, calculations can proceed without S coefficients
- The system uses the most recent M model as baseline
- Accuracy degrades for dates far from the model's base epoch
- Secular variation values will be unavailable/zero

### User Interface Changes

**Before:**
```
File Menu:
  - Add Model...
  - Load Model...
```

**After:**
```
File Menu:
  - Import Model...
```

### Import Dialog Flow

```
┌─────────────────────────────────────────────────────────────┐
│                      Import Model                           │
├─────────────────────────────────────────────────────────────┤
│ Selected File: [________________________] [Browse...]       │
│                                                             │
│ ┌─────────────────────────────────────────────────────────┐│
│ │ Model Information                                       ││
│ │ ─────────────────────────────────────────────────────── ││
│ │ Name:           WMM2025                                 ││
│ │ Type:           WMM                                     ││
│ │ Date Range:     2025.0 - 2030.0                        ││
│ │ Coefficients:   ✓ Main (M)  ✓ Secular (S)  ○ External ││
│ │                                                         ││
│ │ Status:         ✓ Ready to import                      ││
│ └─────────────────────────────────────────────────────────┘│
│                                                             │
│                              [Cancel]  [Import]             │
└─────────────────────────────────────────────────────────────┘
```

### Warning States

**Missing Secular Variation:**
```
┌─────────────────────────────────────────────────────────────┐
│ ⚠ Warning: No Secular Variation Coefficients               │
│                                                             │
│ This model file does not contain secular variation (S)     │
│ coefficients. The model can still be used, but:            │
│                                                             │
│ • Secular variation values will not be available           │
│ • Accuracy decreases for dates far from model epoch        │
│                                                             │
│ [Select Companion File]  [Import Anyway]  [Cancel]         │
└─────────────────────────────────────────────────────────────┘
```

**Invalid File (No M coefficients):**
```
┌─────────────────────────────────────────────────────────────┐
│ ✖ Error: Invalid Model File                                │
│                                                             │
│ The selected file does not contain valid main (M)          │
│ coefficients required for magnetic field calculations.      │
│                                                             │
│ Please select a valid WMM, IGRF, or BGGM coefficient file. │
│                                                             │
│                                         [OK]               │
└─────────────────────────────────────────────────────────────┘
```

## Implementation Details

### Files to Modify

| File | Changes |
|------|---------|
| `frmMain.cs` | Replace menu handlers, remove separate Add/Load logic |
| `frmAddModel.cs` | Rename to `frmImportModel.cs`, add auto-detection UI |
| `frmAddModel.Designer.cs` | Update UI elements for new workflow |
| `ModelReader.cs` | Add method to report coefficient types present |
| `MagneticModelSet.cs` | Add `HasSecularVariation` property |

### New Methods

```csharp
// ModelReader.cs
public static ModelAnalysis AnalyzeFile(string filePath)
{
    return new ModelAnalysis
    {
        HasMainCoefficients = ...,
        HasSecularVariation = ...,
        HasExternalCoefficients = ...,
        ModelType = ...,
        DateRange = ...
    };
}

// MagneticModelSet.cs
public bool HasSecularVariation
{
    get { return Models?.Any(m => m.Type == "S") ?? false; }
}
```

## Acceptance Criteria

- [ ] Single "Import Model" menu option replaces Add/Load
- [ ] Auto-detects coefficient types (M, S, E) present in file
- [ ] Prompts for companion file when S coefficients missing
- [ ] Allows importing M-only models with warning
- [ ] Prevents import of files without M coefficients
- [ ] Displays clear status of model completeness
- [ ] All existing unit tests pass
- [ ] New unit tests for coefficient detection

## Migration Notes

- Existing saved model configurations (JSON) remain compatible
- Users with existing workflows need no action
- Menu keyboard shortcuts may need updating

## Related Issues

- Issue #6: Split DataModel.cs (completed - provides better model structure)
- Issue #19: WPF Migration (this feature aligns with simplified UI goals)
