# Changelog

All notable changes to GeoMagSharpGUI are recorded here. Versions follow [SemVer](https://semver.org/) and the project's `development → preview → master` promotion flow; pre-release builds are tagged `vX.Y.Z-preview.N`.

## Unreleased

Currently in `development` ahead of the next stable cut. Will land together with the GeoMagSharp 1.7.1 NuGet release.

### Features
- **Switch model loading to `ModelDiscovery` API** (#57): GUI now uses `GeoMagSharp.ModelDiscovery.DiscoverModels()` for folder enumeration instead of the hand-rolled `Directory.GetFiles` + `ModelReader.Read` loop. Adds HDGM `.dll` discovery, `.models.json` cache for fast subsequent startups, and centralized error handling. Folder is the source of truth — `MagneticModels.json` no longer required.
- **Expanded file picker filter**: Add Model / Load Model dialogs now offer five filter options — All Supported (`.cof;.dat;.dll`), Coefficient (`.cof`), Data (`.dat`), HDGM Library (`.dll`), All Files. Resolves #55.
- **CSV and JSON export** (#26 / PR #53): results grid now exports to CSV and JSON in addition to TXT.
- **ISCWSA uncertainty display** (#48 / PR #52): uncertainty summary row appears in the results grid (1-sigma) when the loaded model exposes uncertainty data.

### Bug Fixes
- **Reject unclassifiable files in Add/Load** (#60): empty `.cof` or garbled-content files no longer copy into `coefficient/` when picked via the dialogs. Belt-and-suspenders alongside [GeoMagSharp #27](https://github.com/StreckerCM/GeoMagSharp/issues/27).

### Infrastructure
- **Remove bundled coefficient files** (#49 / PR #51): removed `IGRF12.COF`, `WMM2015.COF`, and `MagneticModels.json` from source. Coefficient files now come from the GeoMagSharp NuGet package via `PackageCopyToOutput`, with an `AfterBuild` MSBuild target moving them into `coefficient/` at build time.
- **Bump GeoMagSharp dependency** to 1.7.1 (multi-epoch IGRF/DGRF DisplayName fix, cache invalidation, NONE-filter).

## v1.3.0-preview.3 (2026-03-12)

### Features
- **Bump GeoMagSharp to 1.5.0**: enables ISCWSA uncertainty estimation in the underlying library (UI display arrives in the next release).
- **Replace embedded GeoMagSharp project with NuGet package** (#38 / PR #44): the GUI repo no longer carries an in-tree copy of the library; it consumes `GeoMagSharp` from nuget.org.

### Documentation
- Update branching strategy to include the `development` branch.
- Add model hints to Ralph Loop personas.

## v1.2.0-preview.2 (2026-02-13)

### Features
- **Async operations for model reading and calculations** (#24 / PR #33): `ModelReader.ReadAsync`, `MagneticCalculationsAsync`, `SaveResultsAsync` with progress reporting and cancellation support. Cancel button visible during long operations; Escape cancels.
- **MVC refactor of `DataModel.cs`** (#22): split into smaller files and centralized constants for maintainability.
- **XML documentation** added across public APIs (#28).

### Bug Fixes
- Fix issues #7 (Latitude/Longitude duplication), #8 (DialogResult on cancel), #13 (keyboard shortcuts), #18 (WGS84 constants and pole handling) (PR #21).
- Reduce `ModelReader.cs` code duplication; add unit tests (#9, #12 / PR #27).

### Infrastructure
- Add CI/CD pipeline with MSI installer support and 4-branch promotion flow (#37, #39).
- Add Claude Code permissions and slim CLAUDE.md.
- Strengthen CLAUDE.md with mandatory Ralph Loop and branch protection rules (#31).

## v1.1.0 (2026-02-01)

### Features
- **WMM2020+ coefficient file format support** (#1 / PR #17): handle WMM coefficient files using the post-2020 format alongside legacy formats.

### Notes
- First "modernized" release after the long gap since v1.0.0 (2014). Subsequent development moved to a CI-driven preview/release flow.

## v1.0.0 (2014-07-30)

Initial public release of GeoMag # — WinForms application for IGRF / WMM magnetic field calculations.
