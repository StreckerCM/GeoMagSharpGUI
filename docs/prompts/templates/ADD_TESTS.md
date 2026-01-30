# Add Tests Prompt Template

Copy and customize this template for adding unit tests.

---

## Template

```
Using the TESTER persona from docs/prompts/PERSONAS.md:

## Component to Test
[Class or method to add tests for]

## Testing Focus
- [Specific functionality to test]
- [Edge cases to cover]
- [Error conditions to verify]

## Instructions
1. Review the component's public API and behavior
2. Identify test cases covering normal operation
3. Identify edge cases and boundary conditions
4. Add tests following Arrange-Act-Assert pattern
5. Name tests descriptively: MethodName_Scenario_ExpectedResult
6. Run tests after each addition to verify they pass
7. Ensure tests are independent (no shared state)

## Success Criteria
- Tests cover the specified functionality
- All tests pass
- Edge cases are covered
- Tests would catch real bugs if code regresses
- No test warnings

## Completion
When all success criteria are met, output:
<promise>TESTS COMPLETE</promise>
```

---

## Example: Calculator Tests

```bash
/ralph-loop "Using the TESTER persona from docs/prompts/PERSONAS.md:

## Component to Test
Calculator.SpotCalculation() in GeoMagSharp

## Testing Focus
- Normal operation with various coordinates
- Boundary conditions (poles, antimeridian, date limits)
- Different algorithm selections (BGS, NOAA, MAGVAR)
- Invalid input handling

## Reference Values
Use NOAA online calculator for expected values:
https://www.ngdc.noaa.gov/geomag/calculators/magcalc.shtml

## Test Cases to Add
1. SpotCalculation_EquatorPrimeMeridian_MatchesReference
2. SpotCalculation_NorthPole_MatchesReference
3. SpotCalculation_SouthPole_MatchesReference
4. SpotCalculation_Antimeridian_MatchesReference
5. SpotCalculation_HistoricalDate_MatchesReference
6. SpotCalculation_MinModelDate_MatchesReference
7. SpotCalculation_MaxModelDate_MatchesReference

## Instructions
1. Add test file CalculatorUnitTest.cs to GeoMagSharp-UnitTests
2. Load IGRF model for consistent testing
3. Add each test case with known reference values
4. Use tolerance of 0.1 degree for angles, 10 nT for intensities
5. Run vstest.console.exe after adding tests

## Success Criteria
- All specified test cases implemented
- All tests pass
- Tests verify actual calculations, not just no-exception

## Completion
When all success criteria are met, output:
<promise>TESTS COMPLETE</promise>" --completion-promise "TESTS COMPLETE" --max-iterations 20
```

---

## Example: ModelReader Tests

```bash
/ralph-loop "Using the TESTER persona from docs/prompts/PERSONAS.md:

## Component to Test
ModelReader class in GeoMagSharp

## Testing Focus
- COF file parsing
- DAT file parsing
- Invalid file handling
- Missing file handling
- Malformed data handling

## Test Cases to Add
1. Read_ValidCOFFile_ReturnsCorrectModel
2. Read_ValidDATFile_ReturnsCorrectModel
3. Read_NonexistentFile_ThrowsFileNotFound
4. Read_EmptyFile_ThrowsBadCharacter
5. Read_TruncatedFile_ThrowsAppropriateException
6. Read_InvalidCoefficients_ThrowsBadNumberOfCoefficients
7. COFreader_ParsesHeaderCorrectly
8. COFreader_ParsesCoefficientsCorrectly

## Instructions
1. Add test file ModelReaderUnitTest.cs
2. Use test coefficient files in test resources
3. Verify correct exception types are thrown
4. Check parsed model properties match expected values
5. Run tests after each addition

## Success Criteria
- All specified test cases implemented
- All tests pass
- Error conditions throw correct exception types
- Tests are independent and repeatable

## Completion
When all success criteria are met, output:
<promise>TESTS COMPLETE</promise>" --completion-promise "TESTS COMPLETE" --max-iterations 20
```
