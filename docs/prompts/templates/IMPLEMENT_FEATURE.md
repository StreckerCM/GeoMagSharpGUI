# Feature Implementation Prompt Template

Copy and customize this template for implementing features with a single persona.

---

## Template

```
Using the IMPLEMENTER persona from docs/prompts/PERSONAS.md:

## Task
Implement Feature [FEATURE_NAME] for GeoMagSharpGUI.

## Reference Documents
- Specification: docs/features/[FOLDER]/spec.md
- Implementation Plan: docs/features/[FOLDER]/plan.md
- Task Checklist: docs/features/[FOLDER]/tasks.md

## Instructions
1. Read all reference documents first
2. Follow the implementation plan phases in order
3. Check off tasks in tasks.md as you complete them using `[x]`
4. Run `msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"` after significant changes
5. Commit logical chunks with descriptive messages
6. If blocked, document the issue in tasks.md and continue with other tasks

## Success Criteria
- All tasks in tasks.md are checked `[x]`
- `msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"` succeeds
- No new compiler warnings
- Code follows existing project patterns (see AGENTS.md)

## Completion
When all success criteria are met, output:
<promise>FEATURE COMPLETE</promise>
```

---

## Example: Calculator Enhancement

```bash
/ralph-loop "Using the IMPLEMENTER persona from docs/prompts/PERSONAS.md:

## Task
Implement Calculator performance improvements for GeoMagSharpGUI.

## Reference Documents
- Specification: docs/features/calculator-perf/spec.md
- Implementation Plan: docs/features/calculator-perf/plan.md
- Task Checklist: docs/features/calculator-perf/tasks.md

## Instructions
1. Read all reference documents first
2. Profile existing Calculator.SpotCalculation method
3. Implement optimizations while preserving accuracy
4. Check off tasks in tasks.md as you complete them using [x]
5. Run msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform=\"x86\" after changes
6. Run unit tests to verify calculations still correct
7. Commit logical chunks with descriptive messages

## Success Criteria
- All tasks in tasks.md are checked [x]
- msbuild succeeds with no warnings
- All unit tests pass
- Performance improved by at least 20%

## Completion
When all success criteria are met, output:
<promise>FEATURE COMPLETE</promise>" --completion-promise "FEATURE COMPLETE" --max-iterations 25
```

---

## Example: New Model Format Support

```bash
/ralph-loop "Using the IMPLEMENTER persona from docs/prompts/PERSONAS.md:

## Task
Add support for SHC (Spherical Harmonic Coefficient) file format to ModelReader.

## Instructions
1. Study existing COF and DAT readers in ModelReader.cs
2. Implement SHCreader() following same patterns
3. Add SHC to knownModels enum in DataModel.cs
4. Update CheckStringForModel() extension method
5. Test with sample SHC file
6. Run msbuild after changes
7. Add unit test for SHC parsing

## Success Criteria
- SHC files load correctly via ModelReader.Read()
- Coefficients match reference values
- Build succeeds with no warnings
- Unit test for SHC parsing passes

## Completion
When all success criteria are met, output:
<promise>FEATURE COMPLETE</promise>" --completion-promise "FEATURE COMPLETE" --max-iterations 20
```
