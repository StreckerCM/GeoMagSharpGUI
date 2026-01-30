# Rotating Persona Feature Implementation

This template uses rotating personas that cycle through different perspectives on each iteration, ensuring comprehensive review from multiple angles.

## How It Works

Each iteration, the persona changes based on `iteration % N`:
- Iteration 0, 6, 12... → Persona 0
- Iteration 1, 7, 13... → Persona 1
- And so on...

This ensures every perspective reviews the work multiple times.

---

## Standard 6-Persona Rotation

Best for most feature implementations:

```bash
/ralph-loop "
Feature: [FEATURE_NUMBER]-[feature-name]
Branch: feature/[FEATURE_NUMBER]-[feature-name]

PHASE 1 - TASK COMPLETION:
- Read docs/features/[FEATURE_FOLDER]/tasks.md
- If any tasks unchecked (- [ ]), complete them first
- Mark completed tasks (- [x]) as you finish them
- Run: msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform=\"x86\"

PHASE 2 - ROTATING PERSONA REVIEW (cycle each iteration):
Current Persona = ITERATION MOD 6:

[0] #5 IMPLEMENTER:
- Check tasks.md for incomplete items
- Implement next unchecked task
- Follow existing code patterns
- Run build after changes

[1] #9 CODE REVIEWER:
- Review recent changes for bugs, edge cases
- Check error handling and null safety
- Verify code follows project patterns
- Fix any issues found

[2] #7 TESTER:
- Run: vstest.console.exe GeoMagSharp-UnitTests\\bin\\Debug\\GeoMagSharp-UnitTests.dll
- Check test coverage for new code
- Write missing unit tests
- Verify edge cases are covered

[3] #3 UI_UX_DESIGNER (if UI changes):
- Review WinForms layout and controls
- Check tab order and keyboard navigation
- Verify consistent styling
- Check visual states (enabled, disabled, error)

[4] #10 SECURITY_AUDITOR:
- Check for hardcoded values that should be config
- Verify input validation for coordinates
- Look for potential security issues
- Review any new file I/O code

[5] #2 PROJECT_MANAGER:
- Verify all tasks in tasks.md are checked
- Check spec.md requirements are met
- Document any gaps or issues found
- Update tasks.md if new work discovered

EACH ITERATION:
1. Identify current persona (Iteration % 6)
2. Perform that persona's review/work
3. Make improvements or fixes as needed
4. Commit changes with message: '[persona] description'
5. If all tasks complete AND no issues found by ANY persona for 2 full cycles (12 iterations), output completion

OUTPUT <promise>FEATURE COMPLETE</promise> when:
- All tasks in tasks.md are checked [x]
- Build succeeds with no errors
- All personas report no issues for 2 consecutive cycles
" --completion-promise "FEATURE COMPLETE" --max-iterations 30
```

---

## Example: Add New Magnetic Model Support

```bash
/ralph-loop "
Feature: emm-model-support
Branch: feature/emm-model-support

PHASE 1 - TASK COMPLETION:
- Read docs/features/emm-model-support/tasks.md
- If any tasks unchecked (- [ ]), complete them first
- Mark completed tasks (- [x]) as you finish them
- Run: msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform=\"x86\"

PHASE 2 - ROTATING PERSONA REVIEW (cycle each iteration):
Current Persona = ITERATION MOD 6:

[0] #5 IMPLEMENTER:
- Check tasks.md for incomplete items
- Implement next unchecked task
- Follow existing ModelReader patterns
- Run build after changes

[1] #9 CODE REVIEWER:
- Review recent changes for bugs, edge cases
- Check coefficient parsing is correct
- Verify error handling for malformed files
- Fix any issues found

[2] #7 TESTER:
- Run unit tests
- Add tests for EMM file parsing
- Verify calculations match reference values
- Test edge cases (min/max dates, boundaries)

[3] #3 UI_UX_DESIGNER:
- Verify EMM appears correctly in model dropdown
- Check model info display is consistent
- Verify date range validation UI

[4] #10 SECURITY_AUDITOR:
- Check EMM file parsing is safe
- Verify no path traversal vulnerabilities
- Review coefficient array bounds

[5] #2 PROJECT_MANAGER:
- Verify all tasks in tasks.md are checked
- Ensure backward compatibility maintained
- Document any remaining work

EACH ITERATION:
1. Identify current persona (Iteration % 6)
2. Perform that persona's review/work
3. Make improvements or fixes as needed
4. Commit changes with message: '[persona] description'
5. If all tasks complete AND no issues found for 2 full cycles, output completion

OUTPUT <promise>FEATURE COMPLETE</promise> when:
- All tasks in tasks.md are checked [x]
- Build succeeds with no errors
- All personas report no issues for 2 consecutive cycles
" --completion-promise "FEATURE COMPLETE" --max-iterations 30
```

---

## Compact 4-Persona Rotation (Faster)

For simpler features or when speed is preferred:

```bash
/ralph-loop "
Feature: [FEATURE_NUMBER]-[feature-name]

ROTATING PERSONA (ITERATION MOD 4):

[0] #5 IMPLEMENTER: Complete next task from tasks.md, run build
[1] #9 REVIEWER: Review code for bugs/issues, fix problems
[2] #7 TESTER: Verify functionality, add tests if needed
[3] #2 PROJECT_MANAGER: Check all requirements met, update tasks.md

EACH ITERATION:
1. Run current persona's checks
2. Make one fix/improvement
3. Commit: '[persona] description'

OUTPUT <promise>DONE</promise> when all tasks complete and 2 clean cycles.
" --completion-promise "DONE" --max-iterations 20
```

---

## Commit Message Format

Each commit should indicate which persona made the change:

```
[IMPLEMENTER] Add EMM coefficient file reader
[REVIEWER] Fix array bounds check in ModelReader
[TESTER] Add unit tests for EMM date validation
[UI_UX_DESIGNER] Update model dropdown styling
[SECURITY_AUDITOR] Add input validation for model paths
[PROJECT_MANAGER] Mark EMM support tasks complete
```
