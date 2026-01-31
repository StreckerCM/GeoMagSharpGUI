# WMM Format Support Update - Implementation Plan

## Overview

This plan outlines the phased approach to update the coefficient file reader for WMM2020+ format support while maintaining backward compatibility.

## Phase 1: Header Format Detection

### Goals
- Detect whether a COF file uses the old or new header format
- Support model name detection anywhere in the header line

### Tasks
1. Update `CheckStringForModel()` to find model names at any position
2. Add a helper method to detect header format type
3. Update `COFreader()` to use format-aware header parsing

### Files Affected
- `GeoMagSharp/ExtensionMethods.cs`
- `GeoMagSharp/ModelReader.cs`

### Implementation Details

**Current `CheckStringForModel()` Logic:**
```csharp
// Only returns model if found at index 0 (except EMM)
if (idx.Equals(0) && !(model.Equals(knownModels.EMM) || model.Equals(knownModels.NONE)))
{
    return model;
}
```

**Proposed Change:**
```csharp
// Return model if found anywhere in line for WMM detection
// Add logic to detect new vs old format based on whether line starts with digit
```

**Header Format Detection:**
```csharp
private static bool IsNewHeaderFormat(string headerLine)
{
    // New format starts with year (digit), old format starts with model name (letter)
    return !string.IsNullOrEmpty(headerLine) && char.IsDigit(headerLine.TrimStart()[0]);
}
```

---

## Phase 2: Year Extraction

### Goals
- Extract model epoch year correctly from both header formats
- Handle different field positions in old vs new format

### Tasks
1. Add year extraction for new format (year is first field)
2. Maintain existing year extraction for old format (year is second field)
3. Validate extracted year is reasonable (1900-2100)

### Files Affected
- `GeoMagSharp/ModelReader.cs`

### Implementation Details

**Old Format Parsing (WMM2015/IGRF):**
```
WMM-2015  2015.00 12 12  0 2014.75 2020.00    -1.0 600.0
^model    ^year
fields[0] fields[1]
```

**New Format Parsing (WMM2020+):**
```
    2020.0            WMM-2020        12/10/2019
    ^year             ^model          ^release date
fields[0]         fields[1]       fields[2]
```

---

## Phase 3: End-of-File Marker Handling

### Goals
- Detect and skip the `999...999` terminator lines
- Prevent parsing errors from terminator lines

### Tasks
1. Add terminator line detection in `COFreader()`
2. Break parsing loop when terminator is encountered
3. Ensure partial terminator lines don't cause issues

### Files Affected
- `GeoMagSharp/ModelReader.cs`

### Implementation Details

```csharp
// Check for terminator line (48 nines)
if (inbuff.StartsWith("9999999999"))
{
    break; // End of coefficient data
}
```

---

## Phase 4: WMMHR High-Resolution Support

### Goals
- Support spherical harmonic coefficients up to degree 18+
- Handle dynamic degree allocation based on file content

### Tasks
1. Review `Constants.MaxDeg` usage and impact
2. Update coefficient storage to handle higher degrees dynamically
3. Verify Calculator handles variable degree models

### Files Affected
- `GeoMagSharp/DataModel.cs`
- `GeoMagSharp/ModelReader.cs`
- `GeoMagSharp/Calculator.cs` (verification only)

### Implementation Details

Current `MaxDeg = 13` may be insufficient for WMMHR (degree 18).
Options:
1. **Increase globally**: Change to `MaxDeg = 20` (simple but uses more memory)
2. **Dynamic allocation**: Determine from file content (more complex but efficient)

**Recommendation**: Increase `MaxDeg` to 20 for simplicity. The memory impact is minimal.

---

## Phase 5: Unit Tests

### Goals
- Verify all supported file formats parse correctly
- Ensure backward compatibility is maintained
- Test edge cases and error handling

### Tasks
1. Create `ModelReaderUnitTest.cs` test file
2. Add tests for WMM2015 format (backward compatibility)
3. Add tests for WMM2020 format
4. Add tests for WMM2025 format
5. Add tests for WMMHR format
6. Add tests for IGRF format
7. Add tests for error cases (malformed files)

### Files Affected
- `GeoMagSharp-UnitTests/ModelReaderUnitTest.cs` (new)
- `GeoMagSharp-UnitTests/GeoMagSharp-UnitTests.csproj`

### Test Data
Copy test coefficient files to test project:
- `GeoMagGUI/coefficient/WMM2015.COF`
- External: `WMM2020.COF`, `WMM2025.COF`, `WMMHR.COF`

---

## Phase 6: Integration Testing

### Goals
- Verify end-to-end calculations work with new models
- Compare calculation results with NOAA reference values

### Tasks
1. Load each model and perform test calculations
2. Compare against NOAA test values (from PDF files)
3. Document any precision differences

### Files Affected
- `GeoMagSharp-UnitTests/CalculatorUnitTest.cs`

### Reference Data
- `WMM2020_TEST_VALUES.txt`
- `WMM2025_TestValues.txt`
- `WMMHR2025_TEST_VALUES.txt`

---

## Testing Strategy

### Unit Tests
- Test header detection for both formats
- Test year extraction for both formats
- Test terminator line handling
- Test coefficient parsing for each model type

### Integration Tests
- Load each supported model file
- Perform calculations at known test points
- Verify results within tolerance of NOAA values

### Regression Tests
- Ensure existing WMM2015 and IGRF tests still pass
- Verify no changes to calculation results for existing models

---

## Rollback Plan

If issues arise:
1. Revert changes to `ExtensionMethods.cs` and `ModelReader.cs`
2. The changes are additive, so removing them restores original behavior
3. Unit tests will catch any regressions before merge

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Breaking backward compatibility | Low | High | Extensive unit tests |
| Incorrect year extraction | Medium | High | Validate against known files |
| WMMHR memory issues | Low | Low | Monitor memory usage in tests |
| Calculation precision changes | Low | Medium | Compare with NOAA test values |

---

## Timeline Estimate

| Phase | Effort |
|-------|--------|
| Phase 1: Header Detection | 1-2 hours |
| Phase 2: Year Extraction | 1 hour |
| Phase 3: EOF Marker | 30 minutes |
| Phase 4: WMMHR Support | 1 hour |
| Phase 5: Unit Tests | 2-3 hours |
| Phase 6: Integration Tests | 1-2 hours |
| **Total** | **6-10 hours** |
