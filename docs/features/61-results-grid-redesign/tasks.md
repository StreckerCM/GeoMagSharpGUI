# Feature: Redesign Results Grid — Master-Detail Layout

Issue: #61
Branch: feature/61-results-grid-redesign
Mockup: [docs/mockups/results-grid-redesign.html](../../mockups/results-grid-redesign.html)

## Overview

Restructure `frmMain`'s results display from a single mega-grid (currently 8 components × N dates plus uncertainty/SV rows) into a **master-detail** layout: a values-only grid on the left, a rich detail pane on the right showing the selected row's full breakdown (per-component σ, change/yr, model metadata, HDGM coverage flag).

Export becomes **always-full** (Pattern A) — the JSON/CSV files capture every available field regardless of what the grid shows.

## Workflow

Subagent-driven development (same pattern as #57): implement in main thread, dispatch parallel reviewer/tester/UI subagents at the end of each phase rather than full Ralph rotation. Phases are independent enough to ship incrementally.

## Phases

### Phase 1 — Layout shift: SplitContainer + side panel (this PR)

**Goal:** No new data displayed. Existing data redistributed: values stay in grid (5 columns instead of 8), uncertainty/SV/model info move to a new side panel populated by row selection.

- [ ] Add `SplitContainer` to `frmMain` between the toolbar/inputs area and the existing status bar
- [ ] Move existing `dataGridViewResults` into `splitContainer.Panel1`
- [ ] Build new detail panel in `splitContainer.Panel2` — `TableLayoutPanel` with sections:
  - Header: date + row index ("3 of 5")
  - Field Values (Decl, Incl, H, F)
  - Components (X north, Y east, Z down)
  - Change/yr (subset — D, I, F)
  - Model (name, type, validity range, σ source)
- [ ] Wire `dataGridViewResults.SelectionChanged` → populate detail panel from the corresponding `MagneticCalculations` instance
- [ ] Auto-select row 0 after `buttonCalculate_Click` populates the grid (so the panel shows for single-date results)
- [ ] Reduce grid columns to 5 (Date, Decl, Incl, H, F) — hide North/East/Vert/Total columns (keep them in the underlying `MagneticCalculations` so export still has them)
- [ ] **Remove** the existing in-grid `Change/yr` and `Uncertainty (1σ)` rows (moved to side panel)
- [ ] Persist last calculation (existing `_lastCalculationOptions` / `_lastModelName` fields) so the side panel survives a re-bind

**Subagent reviews after Phase 1:**
- Reviewer: row-selection edge cases (empty grid, multi-select, after re-calculate)
- Tester: cold launch, calculate single date, calculate range, switch model, repeat calculation
- UI/UX: grid-to-panel proportions, font sizing, scroll behavior on long ranges

### Phase 2 — Per-component σ display in side panel

**Depends on:** StreckerCM/GeoMagSharp#13 (WMM/WMMHR Level 2 location-dependent uncertainty) — without it, only HDGM has per-component σ.

- [ ] Replace ISCWSA-only fields in detail panel with `Uncertainty.SigmaD/SigmaI/SigmaH/SigmaF/SigmaX/SigmaY/SigmaZ` when populated
- [ ] Inline format: `18,712 nT  ±118 nT` per component
- [ ] Coverage badge for HDGM (✓ NSD coverage / ⚠ Satellite fallback)
- [ ] Gray-out fields for which σ is null (e.g. WMM pre-#13)

### Phase 3 — Export refactor (Pattern A)

- [ ] Refactor existing `Save Results` exporters to read from the `MagneticCalculations` model objects, not from grid cells
- [ ] `SaveFileDialog` filter offers JSON, CSV-wide, CSV-long, TXT (existing) options
- [ ] JSON: full nested serialization including `Uncertainty` subobject and a top-level `model` block
- [ ] CSV-wide: 25+ columns; deterministic ordering; empty cells where field unavailable
- [ ] CSV-long: tidy format (Date, Field, Value, Unit, Sigma, ChangePerYr) — better for spreadsheet pivots
- [ ] Existing TXT format unchanged for backwards compatibility

### Phase 4 — Cleanup + polish

- [ ] Add "/yr" suffix to secular variation values (item from old #25)
- [ ] Add right-click context menu for cell copy / row copy / table copy (item from old #25)
- [ ] Update README with screenshots of the new layout
- [ ] Visual smoke test with each model type (WMM, WMMHR, IGRF12/13/14, HDGM)

## Out of scope

- WPF rewrite (#19) — this is a WinForms refactor. Master-detail pattern is portable.
- Multi-model comparison view — possible side-panel evolution later.
- Real-time disturbance corrections (HDGM-RT) — DLL adapter currently discards `outData[24]`.
- Date-range step-size improvements (#54) — separate UX issue.

## Completion Criteria

- [ ] All Phase 1 tasks checked
- [ ] Build succeeds (`msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"`)
- [ ] Manual smoke test with WMM2025 single calc, IGRF14 date range, HDGM (if .dll available)
- [ ] No regression in existing keyboard shortcuts, model loading, or save/export

## Notes

- Phase 1 alone is mergeable — it's a layout reshuffling that adds no new data display capability. Phase 2 adds the actual new data (per-component σ).
- The existing `MagneticCalculations` model already exposes everything needed; no library changes required for Phase 1.
- Designer.cs hand-editing risk: when WinForms designer re-saves the form via Visual Studio, it may regenerate the layout. To mitigate, we'll either edit Designer.cs directly with comments or accept that the designer view becomes the source of truth.
