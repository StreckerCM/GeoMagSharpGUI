# WMM Format Support Update - Specification

## Overview

Update the coefficient file reader to support the newer WMM2020+ file format while maintaining backward compatibility with WMM2015, IGRF, and EMM formats. This addresses GitHub Issue #1.

## User Story

As a user of GeoMagSharpGUI, I want to load WMM2020, WMM2025, and WMMHR2025 coefficient files so that I can calculate magnetic field values using the latest geomagnetic models.

## Background

The World Magnetic Model (WMM) coefficient file format changed between WMM2015 and WMM2020. The current `ModelReader.COFreader()` method was written for the older format and fails to parse the newer files correctly.

### Format Comparison

| Aspect | WMM2015/IGRF (Old) | WMM2020+ (New) |
|--------|-------------------|----------------|
| **Header Start** | Model name first | Year first |
| **Header Example** | `WMM-2015  2015.00 12 12  0...` | `2020.0  WMM-2020  12/10/2019` |
| **Model Detection** | At line start (idx=0) | Anywhere in line |
| **End Marker** | None/blank | `999...999` (48 nines) |
| **Max Degree** | 12 | 12 (standard), 18+ (WMMHR) |

### Root Cause

The `CheckStringForModel()` extension method requires the model name to be at position 0 of the header line. WMM2020+ files start with the year, so the model name appears later in the line, causing detection to fail.

## Requirements

### Functional Requirements

1. **FR-1**: Parse WMM2020.COF files correctly
2. **FR-2**: Parse WMM2025.COF files correctly
3. **FR-3**: Parse WMMHR2025.COF high-resolution files correctly
4. **FR-4**: Maintain backward compatibility with WMM2015.COF format
5. **FR-5**: Maintain backward compatibility with IGRF12.COF format
6. **FR-6**: Maintain backward compatibility with EMM format files
7. **FR-7**: Correctly extract model epoch year from both header formats
8. **FR-8**: Skip end-of-file marker lines (48 nines)
9. **FR-9**: Support higher spherical harmonic degrees (up to 18+) for WMMHR

### Non-Functional Requirements

1. **NFR-1**: No breaking changes to the public API
2. **NFR-2**: Parsing performance should not degrade
3. **NFR-3**: Clear error messages for malformed files

## Acceptance Criteria

- [ ] WMM2020.COF loads without errors
- [ ] WMM2025.COF loads without errors
- [ ] WMMHR2025.COF loads without errors
- [ ] WMM2015.COF continues to load correctly
- [ ] IGRF12.COF continues to load correctly
- [ ] Magnetic calculations using new models produce reasonable values
- [ ] Unit tests pass for all supported file formats
- [ ] Existing unit tests continue to pass

## Dependencies

- No external library dependencies
- Requires test coefficient files:
  - `WMM2020.COF` (from NOAA)
  - `WMM2025.COF` (from NOAA)
  - `WMMHR2025.COF` (from NOAA)

## Affected Files

- `GeoMagSharp/ExtensionMethods.cs` - `CheckStringForModel()` method
- `GeoMagSharp/ModelReader.cs` - `COFreader()` method
- `GeoMagSharp/DataModel.cs` - `Constants.MaxDeg` if needed for WMMHR
- `GeoMagSharp-UnitTests/` - New test file for ModelReader

## Open Questions

1. **Q1**: Should WMMHR be treated as a separate `knownModels` enum value, or as a variant of WMM?
   - **Recommendation**: Treat as WMM variant (same calculation logic, different precision)

2. **Q2**: Should we increase `Constants.MaxDeg` globally, or handle WMMHR dynamically?
   - **Recommendation**: Handle dynamically based on file content to avoid memory impact for standard models

## References

- [WMM Format Specification](https://www.ngdc.noaa.gov/geomag/WMM/wmmformat.shtml)
- [NOAA WMM Downloads](https://www.ngdc.noaa.gov/geomag/WMM/DoDWMM.shtml)
- GitHub Issue #1
