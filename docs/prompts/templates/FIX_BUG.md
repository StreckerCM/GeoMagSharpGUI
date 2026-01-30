# Bug Fix Prompt Template

Copy and customize this template for investigating and fixing bugs.

---

## Template

```
Using the DEBUGGER persona from docs/prompts/PERSONAS.md:

## Bug Description
[Description of the bug, including steps to reproduce if known]

## Expected Behavior
[What should happen]

## Actual Behavior
[What actually happens]

## Instructions
1. Reproduce the bug first
2. Add diagnostic logging/output to trace the issue
3. Form a hypothesis about the root cause
4. Verify hypothesis through testing
5. Implement fix addressing root cause (not just symptoms)
6. Add regression test to prevent recurrence
7. Run full test suite to ensure no new issues

## Success Criteria
- Bug is fixed and no longer reproduces
- Root cause is documented in commit message
- Regression test added and passing
- All existing tests still pass
- Build succeeds with no warnings

## Completion
When all success criteria are met, output:
<promise>BUG FIXED</promise>
```

---

## Example: Calculation Accuracy Bug

```bash
/ralph-loop "Using the DEBUGGER persona from docs/prompts/PERSONAS.md:

## Bug Description
Declination calculation returns incorrect values near the magnetic poles (latitude > 85 degrees).
User reports differ by ~2 degrees from expected values.

## Expected Behavior
Declination should match NOAA online calculator within 0.1 degree tolerance.

## Actual Behavior
Values near poles show 1-2 degree discrepancy.

## Instructions
1. Reproduce with test coordinates: 87.0 lat, 0.0 long, 2024-01-01
2. Compare with NOAA online calculator reference
3. Add logging to Calculator.SpotCalculation for intermediate values
4. Check spherical harmonic expansion near poles
5. Verify coordinate system conversion (geodetic vs geocentric)
6. Implement fix for root cause
7. Add unit test with polar coordinates

## Success Criteria
- Polar calculations match NOAA within 0.1 degree
- Root cause documented
- Regression test for polar coordinates
- All existing tests pass

## Completion
When all success criteria are met, output:
<promise>BUG FIXED</promise>" --completion-promise "BUG FIXED" --max-iterations 15
```

---

## Example: File Loading Bug

```bash
/ralph-loop "Using the DEBUGGER persona from docs/prompts/PERSONAS.md:

## Bug Description
ModelReader.Read() throws exception when loading certain EMM coefficient files.
Error: 'Index was outside the bounds of the array' at line 142.

## Expected Behavior
EMM files should load successfully like IGRF and WMM files.

## Actual Behavior
Array index exception during coefficient parsing.

## Instructions
1. Obtain sample EMM file that causes the error
2. Add try-catch with detailed logging around line 142
3. Compare EMM file format with working IGRF/WMM files
4. Identify format difference causing array bounds issue
5. Fix coefficient array sizing or parsing logic
6. Add unit test with EMM file
7. Verify all model types still load correctly

## Success Criteria
- EMM files load without exception
- All model types (IGRF, WMM, DGRF, EMM) load correctly
- Regression test for EMM loading
- No new warnings

## Completion
When all success criteria are met, output:
<promise>BUG FIXED</promise>" --completion-promise "BUG FIXED" --max-iterations 15
```
