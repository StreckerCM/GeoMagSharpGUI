# WMM Format Support Update - Task List

## Phase 1: Header Format Detection

- [ ] Update `CheckStringForModel()` in `ExtensionMethods.cs`
  - [ ] Allow WMM detection anywhere in line (not just position 0)
  - [ ] Add WMMHR detection (treat as WMM variant)
  - [ ] Maintain EMM special handling
  - [ ] Maintain backward compatibility for IGRF detection

- [ ] Add header format detection helper in `ModelReader.cs`
  - [ ] Create `IsNewHeaderFormat()` private method
  - [ ] Detect based on whether line starts with digit vs letter

- [ ] Update `COFreader()` header parsing
  - [ ] Use format detection on first line
  - [ ] Branch to appropriate parsing logic

## Phase 2: Year Extraction

- [ ] Add new format year extraction
  - [ ] Parse year from first whitespace-delimited field
  - [ ] Validate year is in reasonable range (1900-2100)

- [ ] Maintain old format year extraction
  - [ ] Parse year from second field (after model name)
  - [ ] No changes needed to existing logic

- [ ] Update model creation with extracted year
  - [ ] Ensure `MagneticModel.Year` is set correctly for both formats

## Phase 3: End-of-File Marker Handling

- [ ] Add terminator detection in `COFreader()` read loop
  - [ ] Check if line starts with "9999999999"
  - [ ] Break loop when terminator found
  - [ ] Handle case where terminator has trailing content

- [ ] Test terminator handling
  - [ ] Verify WMM2020.COF parses completely
  - [ ] Verify no errors from terminator lines

## Phase 4: WMMHR High-Resolution Support

- [ ] Review `Constants.MaxDeg` in `DataModel.cs`
  - [ ] Current value: 13
  - [ ] Required for WMMHR: 18+
  - [ ] Decision: Increase to 20

- [ ] Update `Constants.MaxDeg` if needed
  - [ ] Change value from 13 to 20
  - [ ] Verify `MaxCoeff` calculation still correct

- [ ] Verify coefficient storage handles larger models
  - [ ] Check `MagneticModel.SharmCoeff` list capacity
  - [ ] Check `Coefficients.coeffs` list capacity

- [ ] Verify `Calculator.GetField()` handles larger degrees
  - [ ] Check array allocations use dynamic sizing
  - [ ] Verify no hardcoded degree limits

## Phase 5: Unit Tests

- [ ] Create `ModelReaderUnitTest.cs`
  - [ ] Add to test project
  - [ ] Update `.csproj` to include new file

- [ ] Add backward compatibility tests
  - [ ] Test WMM2015.COF parsing
  - [ ] Test IGRF12.COF parsing
  - [ ] Verify model type detected correctly
  - [ ] Verify year extracted correctly
  - [ ] Verify coefficient count correct

- [ ] Add WMM2020 format tests
  - [ ] Test WMM2020.COF parsing
  - [ ] Verify model type is WMM
  - [ ] Verify year is 2020.0
  - [ ] Verify terminator handled

- [ ] Add WMM2025 format tests
  - [ ] Test WMM2025.COF parsing
  - [ ] Verify model type is WMM
  - [ ] Verify year is 2025.0

- [ ] Add WMMHR format tests
  - [ ] Test WMMHR.COF parsing
  - [ ] Verify model type is WMM
  - [ ] Verify coefficient count for degree 18

- [ ] Add error handling tests
  - [ ] Test empty file
  - [ ] Test malformed header
  - [ ] Test missing coefficients

## Phase 6: Integration Testing

- [ ] Add calculation tests with WMM2020
  - [ ] Load WMM2020.COF
  - [ ] Calculate at NOAA test points
  - [ ] Compare with reference values (100nT tolerance)

- [ ] Add calculation tests with WMM2025
  - [ ] Load WMM2025.COF
  - [ ] Calculate at NOAA test points
  - [ ] Compare with reference values

- [ ] Add calculation tests with WMMHR2025
  - [ ] Load WMMHR.COF
  - [ ] Calculate at NOAA test points
  - [ ] Compare with reference values
  - [ ] Note any precision improvements

## Verification

- [ ] Build succeeds with no warnings
- [ ] All existing tests pass
- [ ] All new tests pass
- [ ] Manual testing with GUI application
  - [ ] Load WMM2020.COF in GUI
  - [ ] Load WMM2025.COF in GUI
  - [ ] Perform sample calculations

## Documentation

- [ ] Update AGENTS.md if coding patterns change
- [ ] Add comments to new/modified methods
- [ ] Document any limitations or known issues

## Completion

- [ ] All tasks checked off
- [ ] Code reviewed
- [ ] Changes committed with descriptive message
- [ ] GitHub Issue #1 closed

---

## Test Data Locations

**Existing (in repository):**
- `GeoMagGUI/coefficient/WMM2015.COF`
- `GeoMagGUI/coefficient/IGRF12.COF`

**External (need to copy or reference):**
- `C:\GitHub\Magnetic Models\WMM2020COF\WMM2020.COF`
- `C:\GitHub\Magnetic Models\WMM2025COF\WMM2025.COF`
- `C:\GitHub\Magnetic Models\WMMHR2025COF\WMMHR.COF`

**Reference Values:**
- `C:\GitHub\Magnetic Models\WMM2020COF\WMM2020_TEST_VALUES.txt`
- `C:\GitHub\Magnetic Models\WMM2025COF\WMM2025_TestValues.txt`
- `C:\GitHub\Magnetic Models\WMMHR2025COF\WMMHR2025_TEST_VALUES.txt`
