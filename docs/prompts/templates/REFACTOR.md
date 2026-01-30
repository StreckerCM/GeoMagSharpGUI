# Refactoring Prompt Template

Copy and customize this template for code refactoring tasks.

---

## Template

```
Using the REFACTORER persona from docs/prompts/PERSONAS.md:

## Refactoring Goal
[What improvement you want to achieve]

## Scope
[Files/classes affected]

## Constraints
- Preserve existing behavior exactly
- Maintain backward compatibility
- No functional changes

## Instructions
1. Understand current implementation thoroughly
2. Make small, incremental changes
3. Run tests after each change
4. Commit frequently with descriptive messages
5. Document any API changes for downstream users

## Success Criteria
- All existing tests still pass
- Build succeeds with no warnings
- Code is cleaner/better organized
- No functional changes introduced

## Completion
When all success criteria are met, output:
<promise>REFACTOR COMPLETE</promise>
```

---

## Example: Extract Data Models

```bash
/ralph-loop "Using the REFACTORER persona from docs/prompts/PERSONAS.md:

## Refactoring Goal
Extract classes from the large DataModel.cs file into separate files for better organization.

## Scope
- GeoMagSharp/DataModel.cs (1000+ lines)
- Create new files in GeoMagSharp/Models/ folder

## Constraints
- Preserve all existing behavior
- Keep backward compatibility (classes remain in GeoMagSharp namespace)
- No functional changes

## Plan
1. Create Models subfolder
2. Extract Preferences class to Models/Preferences.cs
3. Extract CalculationOptions class to Models/CalculationOptions.cs
4. Extract MagneticCalculations class to Models/MagneticCalculations.cs
5. Extract coordinate classes (Latitude, Longitude) to Models/Coordinates.cs
6. Extract MagneticModelSet to Models/MagneticModelSet.cs
7. Keep enums and constants in DataModel.cs (or move to separate files)
8. Update using statements as needed

## Instructions
1. Run tests to establish baseline
2. Extract one class at a time
3. Build after each extraction
4. Run tests after each extraction
5. Commit each successful extraction

## Success Criteria
- All tests pass
- Build succeeds
- Each major class in its own file
- DataModel.cs reduced in size
- No functional changes

## Completion
When all success criteria are met, output:
<promise>REFACTOR COMPLETE</promise>" --completion-promise "REFACTOR COMPLETE" --max-iterations 25
```

---

## Example: Improve Calculator Structure

```bash
/ralph-loop "Using the REFACTORER persona from docs/prompts/PERSONAS.md:

## Refactoring Goal
Improve Calculator class structure by extracting calculation methods into logical groups.

## Scope
- GeoMagSharp/Calculator.cs

## Constraints
- Preserve all calculation accuracy
- Keep public API unchanged
- No functional changes

## Plan
1. Identify logical method groups
2. Consider extracting:
   - SphericalHarmonics helper methods
   - Coordinate conversion methods
   - Field component calculations
3. Add XML documentation where missing
4. Improve variable naming for clarity

## Instructions
1. Run tests to establish baseline
2. Extract helper methods into regions or partial classes
3. Build and test after each change
4. Improve naming without changing behavior
5. Add documentation comments

## Success Criteria
- All tests pass (especially calculation accuracy tests)
- Build succeeds with no warnings
- Code is better organized and documented
- No changes to calculated values

## Completion
When all success criteria are met, output:
<promise>REFACTOR COMPLETE</promise>" --completion-promise "REFACTOR COMPLETE" --max-iterations 20
```

---

## Example: Rename for Consistency

```bash
/ralph-loop "Using the REFACTORER persona from docs/prompts/PERSONAS.md:

## Refactoring Goal
Rename methods and properties to follow consistent naming conventions.

## Scope
- Multiple files in GeoMagSharp

## Items to Rename
- Inconsistent capitalization
- Abbreviations that should be spelled out
- Method names that don't describe their action

## Constraints
- Update all call sites
- Preserve behavior exactly
- Update XML documentation if present

## Instructions
1. Identify naming inconsistencies
2. Rename one item at a time
3. Use IDE refactoring tools to update all references
4. Build and test after each rename
5. Commit each rename separately

## Success Criteria
- All tests pass
- Build succeeds
- Naming is consistent throughout
- No functional changes

## Completion
When all success criteria are met, output:
<promise>REFACTOR COMPLETE</promise>" --completion-promise "REFACTOR COMPLETE" --max-iterations 15
```
