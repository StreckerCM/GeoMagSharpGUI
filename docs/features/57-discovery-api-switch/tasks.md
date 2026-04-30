# Feature: Switch model loading to GeoMagSharp 1.7.0 ModelDiscovery API

Issue: #57
Branch: feature/57-discovery-api-switch
Depends on: GeoMagSharp 1.7.0 (currently consumed via local NuGet feed)

## Background

`origin/development` already has hand-rolled folder discovery added by PR #51 (`DiscoverAndLoadModels()` in `frmMain.cs`) plus async `ModelReader.ReadAsync` for one-off file inspection. This feature replaces that hand-rolled discovery with the GeoMagSharp 1.7.0 library API, which adds:

- HDGM `.dll` support (existing GUI code only handles `.cof`/`.dat`)
- Built-in `.models.json` cache for fast subsequent startups (no `MagneticModels.json` save/load needed)
- `ScanMode.Quick` vs `ScanMode.Full`, `CancellationToken`, centralized `OnError` callback

## Workflow choice

This feature uses **modified subagent-driven development** in place of the standard 6-persona Ralph Loop. Rationale: this feature has no input parsing, no auth flows, and no schema migrations — Security and Project_Mgr Ralph passes would be empty. The chosen approach implements each phase in the main thread (TDD where applicable) and dispatches **Reviewer + Tester + UI_UX_Designer subagents in parallel** at the end of each phase. Roughly 4 iterations total instead of Ralph's 12+.

If issues are found that warrant a full Ralph rotation, fall back to the standard flow.

## Open design decisions (resolved)

1. **`NumberOfModels` label in frmAddModel** — drop. `ModelDescriptor` doesn't expose it; back-channel `ModelReader.Read` defeats the API switch; extending `ModelDescriptor` upstream costs a new GeoMagSharp release. Field is informational-only.
2. **`coefficient/MagneticModels.json` fate** — delete after migration. Folder + library's `.models.json` cache become the source of truth.
3. **Async + progress UI** — keep the existing async pattern by wrapping `DiscoverModels` in `Task.Run`. `DescribeFile` is light enough to call synchronously inside `frmAddModel`, but `DiscoverModels` on first launch may probe HDGM `.dll` files (slow), so async + progress remains valuable.
4. **"Add Model" semantics** — keep "copy file into `coefficient/`, then rescan" pattern. Aligns with #23.

## Tasks

### Phase 1 — Replace startup discovery with `ModelDiscovery.DiscoverModels`

- [ ] Remove `DiscoverAndLoadModels()` private method from `frmMain.cs`
- [ ] Replace startup `MagneticModelCollection.Load(ModelJson)` + `DiscoverAndLoadModels()` call with a single `ModelDiscovery.DiscoverModels(ModelFolder, new ModelDiscoveryOptions { Mode = ScanMode.Full, UseCache = true, OnError = (path, ex) => Debug.WriteLine(...) })`
- [ ] Change `Models` field from `MagneticModelCollection` to `List<ModelDescriptor>` (or rename to `_descriptors`)
- [ ] Update `LoadModels(string selected)` to bind combobox to `IList<ModelDescriptor>` with `DisplayMember = "DisplayName"`, `ValueMember = "FilePath"`
- [ ] Update selection-tracking (currently uses `Guid` ID) to use `string` (file path) instead
- [ ] Wrap `DiscoverModels` in `Task.Run` so the UI thread isn't blocked during HDGM `.dll` probing on first launch (no cached `.models.json`)

**Subagent review pass after Phase 1:**
- Reviewer (code-reviewer subagent): bug check, exception flow, threading
- Tester: launch app cold (no `.models.json`), launch with `.models.json` present, drop new file mid-session, drop bad file mid-session
- UI_UX_Designer: progress feedback during cold scan, fallback message if folder is empty

### Phase 2 — Rewrite `frmAddModel` to use `ModelDiscovery.DescribeFile`

- [ ] Change private field `_Model` from `MagneticModelSet` to `ModelDescriptor`
- [ ] Replace `ModelReader.Read(modelFile)` with `ModelDiscovery.DescribeFile(modelFile)` (sync — descriptor doesn't need progress)
- [ ] Update label bindings: `DisplayName` (was `Name`), `DetectedType` (was `Type`), `MinDate.HasValue ? <decimal-year-to-date> : "—"` (was `MinDate.ToDateTime().ToShortDateString()`), same for `MaxDate`
- [ ] **Drop the `NumberOfModels` label and its row** (descriptor doesn't expose it)
- [ ] Remove `LoadModelDataAsync` (DescribeFile is fast enough to call synchronously)
- [ ] Remove `textBoxModelName_Validated` handler (descriptor is immutable; user-customized names deferred to #23)

**Subagent review pass after Phase 2:**
- Reviewer: ensure descriptor immutability is respected, file path normalization
- Tester: open dialog with WMM, IGRF, HDGM `.dll`, malformed file, missing file
- UI_UX_Designer: review label changes, decide if dropping `NumberOfModels` needs a tooltip explanation

### Phase 3 — Adapt calculator wiring + Add/Load handlers

- [ ] `_MagCalculator.LoadModel(selectedDescriptor.FilePath)` — uses existing `LoadModel(string)` overload in GeoMagSharp
- [ ] `addModelToolStripMenuItem_Click` — drop `Models.AddOrReplace` and `Models.SaveAsync`. Keep the file-copy step. After dialog returns OK, re-run `DiscoverModels` to refresh the combobox; cache auto-updates inside the library
- [ ] `loadModelToolStripMenuItem_Click` — same pattern: copy file, re-run discovery
- [ ] Decide whether `loadModelToolStripMenuItem` and `addModelToolStripMenuItem` should be unified (defer to #23) or stay separate

**Subagent review pass after Phase 3:**
- Reviewer: dialog cancellation paths, error flow for invalid file selection
- Tester: end-to-end calculation with each model type, including HDGM
- UI_UX_Designer: discovery refresh feedback (spinner? immediate? delayed?)

### Phase 4 — Cleanup

- [ ] Delete `coefficient/MagneticModels.json` (no longer the source of truth — library's `.models.json` cache replaces it)
- [ ] Remove `MagneticModelCollection` type from public API surface use within `frmMain.cs` (`MagneticModelSet` may still be referenced internally by GeoMag.LoadModel)
- [ ] Remove `Resources.File_Name_Magnetic_Model_JSON` if unused
- [ ] Remove unused `using GeoMagSharp;` references for types that no longer apply

**Final review pass:**
- Reviewer: full PR review, dead code, unused imports
- Tester: full regression — startup, model selection, calculation, add, load, cancel
- UI_UX_Designer: end-to-end UX walkthrough

## Completion Criteria

- [ ] All tasks above checked
- [ ] Build succeeds (`msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"`)
- [ ] Manual smoke test: launch with empty folder, with cached `.models.json`, with new file dropped in folder, with HDGM `.dll`
- [ ] No regression in existing `MagneticModelCollection` tests — or, if `MagneticModelCollection` is retired, those tests are removed/migrated
- [ ] Subagent review iterations show no new findings on a clean pass

## Out of scope (defer to other issues)

- #23 Unified Model Import (Phase 2 reduces overlap with #23 but doesn't merge Add+Load)
- #55 Allow HDGM `.dll` selection in file filter (folder-scan auto-discovers HDGM `.dll` already in folder; the file-picker filter is still needed if the user wants to copy a `.dll` *in* via Add/Load)
- Removing the embedded `GeoMagSharp/` solution project (already removed on `origin/development` per PR #51)

## Notes

- `nuget.config` working tree contains a `GeoMagSharp-local` folder feed line for testing 1.7.0 prior to nuget.org publish. **Do not commit that line.** Revert just that line before any commit, or use `git add -p` to selectively stage.
- The local feed will become unnecessary once GeoMagSharp 1.7.0 ships to nuget.org.
