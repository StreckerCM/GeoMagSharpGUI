# Feature Specification: Async Model Reader and Calculations

## Summary

Convert model file reading and magnetic field calculations to asynchronous operations to prevent UI freezing during long-running tasks.

## Current State

### Synchronous Operations

Currently, the following operations run synchronously on the UI thread:

| Operation | Location | Impact |
|-----------|----------|--------|
| `ModelReader.Read()` | `frmAddModel.cs:41` | UI freezes during file parsing |
| `GeoMag.MagneticCalculations()` | `frmMain.cs:213` | UI freezes during calculations |
| `MagneticModelCollection.Save()` | `frmMain.cs:313` | Brief freeze on save |
| `MagneticModelCollection.Load()` | `frmMain.cs:106` | Freeze on startup |
| `GeoMag.SaveResults()` | `frmMain.cs:700` | Freeze during export |

### User Impact

- Application appears "Not Responding" during model loading
- Large coefficient files (e.g., WMMHR with 18th degree harmonics) cause noticeable delays
- Date range calculations with many steps can freeze UI significantly
- Users cannot cancel long-running operations

## Proposed Solution

### Async/Await Pattern

Convert identified methods to async versions using the Task-based Asynchronous Pattern (TAP):

```csharp
// Before
public static MagneticModelSet Read(string filePath)

// After
public static async Task<MagneticModelSet> ReadAsync(string filePath,
    IProgress<int> progress = null,
    CancellationToken cancellationToken = default)
```

### Operations to Convert

#### High Priority (User-Facing Delays)

1. **ModelReader.ReadAsync()**
   - File I/O is inherently async-friendly
   - Add progress reporting for large files
   - Support cancellation

2. **Calculator.SpotCalculationAsync()**
   - Wrap computation in `Task.Run()` for CPU-bound work
   - Support cancellation between calculation steps

3. **GeoMag.MagneticCalculationsAsync()**
   - Orchestrates spot calculations
   - Report progress for date range calculations
   - Support cancellation

#### Medium Priority (Startup/Background)

4. **MagneticModelCollection.LoadAsync()**
   - Async JSON deserialization
   - Called at startup

5. **MagneticModelCollection.SaveAsync()**
   - Async JSON serialization
   - Called after model changes

6. **GeoMag.SaveResultsAsync()**
   - Async file writing
   - Progress for large result sets

### Progress Reporting

```csharp
public interface ICalculationProgress
{
    void Report(CalculationProgressInfo value);
}

public class CalculationProgressInfo
{
    public int CurrentStep { get; set; }
    public int TotalSteps { get; set; }
    public string StatusMessage { get; set; }
    public double PercentComplete => TotalSteps > 0
        ? (CurrentStep * 100.0 / TotalSteps)
        : 0;
}
```

### UI Integration

#### Progress Indicator

Add a progress bar to the status strip:

```
┌─────────────────────────────────────────────────────────────┐
│ [Toolbar]                                                   │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│                    [Main Content]                           │
│                                                             │
├─────────────────────────────────────────────────────────────┤
│ Ready | ████████░░░░░░ 60% Loading WMM2025...    [Cancel]  │
└─────────────────────────────────────────────────────────────┘
```

#### Cancellation Support

- Add Cancel button to status bar (visible during operations)
- Support Escape key to cancel
- Clean up partial results on cancellation

## Implementation Details

### ModelReader Changes

```csharp
// GeoMagSharp/ModelReader.cs

public static async Task<MagneticModelSet> ReadAsync(
    string filePath,
    IProgress<CalculationProgressInfo> progress = null,
    CancellationToken cancellationToken = default)
{
    if (string.IsNullOrWhiteSpace(filePath))
        throw new ArgumentNullException(nameof(filePath));

    if (!File.Exists(filePath))
        throw new FileNotFoundException("Model file not found", filePath);

    // Read file asynchronously
    string[] lines = await File.ReadAllLinesAsync(filePath, cancellationToken);

    progress?.Report(new CalculationProgressInfo
    {
        StatusMessage = "Parsing coefficients...",
        CurrentStep = 1,
        TotalSteps = 3
    });

    // Parse based on extension (existing logic)
    var extension = Path.GetExtension(filePath).ToLower();

    return extension switch
    {
        ".cof" => await Task.Run(() => COFreader(lines, filePath), cancellationToken),
        ".dat" => await Task.Run(() => DATreader(lines, filePath), cancellationToken),
        _ => throw new GeoMagException($"Unsupported file type: {extension}")
    };
}

// Keep synchronous version for backward compatibility
public static MagneticModelSet Read(string filePath)
{
    return ReadAsync(filePath).GetAwaiter().GetResult();
}
```

### Calculator Changes

```csharp
// GeoMagSharp/Calculator.cs

public static async Task<MagneticCalculations> SpotCalculationAsync(
    CalculationOptions options,
    DateTime dateOfCalc,
    MagneticModelSet magModels,
    Coefficients internalSH,
    Coefficients externalSH,
    double earthRadius = Constants.EarthsRadiusInKm,
    CancellationToken cancellationToken = default)
{
    return await Task.Run(() =>
    {
        cancellationToken.ThrowIfCancellationRequested();
        return SpotCalculation(options, dateOfCalc, magModels,
            internalSH, externalSH, earthRadius);
    }, cancellationToken);
}
```

### GeoMag Changes

```csharp
// GeoMagSharp/GeoMag.cs

public async Task<List<MagneticCalculations>> MagneticCalculationsAsync(
    IProgress<CalculationProgressInfo> progress = null,
    CancellationToken cancellationToken = default)
{
    ResultsOfCalculation.Clear();

    var dates = GetCalculationDates(); // Helper to generate date list
    int totalSteps = dates.Count;
    int currentStep = 0;

    foreach (var date in dates)
    {
        cancellationToken.ThrowIfCancellationRequested();

        progress?.Report(new CalculationProgressInfo
        {
            CurrentStep = ++currentStep,
            TotalSteps = totalSteps,
            StatusMessage = $"Calculating for {date:yyyy-MM-dd}..."
        });

        var result = await Calculator.SpotCalculationAsync(
            _CalculationOptions, date, _MagModel,
            internalSH, externalSH, _MagModel.EarthRadius,
            cancellationToken);

        ResultsOfCalculation.Add(result);
    }

    return ResultsOfCalculation;
}
```

### UI Integration (frmMain.cs)

```csharp
private CancellationTokenSource _calculationCts;

private async void btnCalculate_Click(object sender, EventArgs e)
{
    _calculationCts = new CancellationTokenSource();

    try
    {
        SetUIBusy(true);

        var progress = new Progress<CalculationProgressInfo>(info =>
        {
            toolStripProgressBar.Value = (int)info.PercentComplete;
            toolStripStatusLabel.Text = info.StatusMessage;
        });

        var results = await _MagCalculator.MagneticCalculationsAsync(
            progress,
            _calculationCts.Token);

        PopulateResultsGrid(results);
    }
    catch (OperationCanceledException)
    {
        toolStripStatusLabel.Text = "Calculation cancelled";
    }
    finally
    {
        SetUIBusy(false);
        _calculationCts?.Dispose();
        _calculationCts = null;
    }
}

private void btnCancel_Click(object sender, EventArgs e)
{
    _calculationCts?.Cancel();
}

private void SetUIBusy(bool busy)
{
    btnCalculate.Enabled = !busy;
    btnCancel.Visible = busy;
    toolStripProgressBar.Visible = busy;
    UseWaitCursor = busy;
}
```

## Files to Modify

| File | Changes |
|------|---------|
| `GeoMagSharp/ModelReader.cs` | Add `ReadAsync()` method |
| `GeoMagSharp/Calculator.cs` | Add `SpotCalculationAsync()` method |
| `GeoMagSharp/GeoMag.cs` | Add `MagneticCalculationsAsync()`, `SaveResultsAsync()` |
| `GeoMagSharp/Models/Magnetic/MagneticModelCollection.cs` | Add `LoadAsync()`, `SaveAsync()` |
| `GeoMagGUI/frmMain.cs` | Update event handlers to use async methods |
| `GeoMagGUI/frmMain.Designer.cs` | Add progress bar and cancel button to status strip |
| `GeoMagGUI/frmAddModel.cs` | Update to use `ReadAsync()` |

## New Classes

```csharp
// GeoMagSharp/Models/Progress/CalculationProgressInfo.cs
namespace GeoMagSharp
{
    public class CalculationProgressInfo
    {
        public int CurrentStep { get; set; }
        public int TotalSteps { get; set; }
        public string StatusMessage { get; set; }
        public double PercentComplete => TotalSteps > 0
            ? (CurrentStep * 100.0 / TotalSteps)
            : 0;
    }
}
```

## Acceptance Criteria

- [ ] `ModelReader.ReadAsync()` reads files without blocking UI
- [ ] `Calculator.SpotCalculationAsync()` performs calculations off UI thread
- [ ] `GeoMag.MagneticCalculationsAsync()` supports progress and cancellation
- [ ] Progress bar visible during long operations
- [ ] Cancel button stops operations gracefully
- [ ] Escape key cancels active operation
- [ ] Synchronous methods remain for backward compatibility
- [ ] All existing unit tests pass
- [ ] New async unit tests added

## Performance Considerations

- Use `ConfigureAwait(false)` in library code where UI context not needed
- Batch progress updates to avoid UI flooding
- Consider pooling for repeated calculations

## Testing Strategy

```csharp
[TestMethod]
public async Task ReadAsync_ValidFile_ReturnsModel()
{
    var result = await ModelReader.ReadAsync(TestDataPath + "WMM2025.COF");
    Assert.IsNotNull(result);
    Assert.AreEqual(knownModels.WMM, result.Type);
}

[TestMethod]
public async Task ReadAsync_Cancellation_ThrowsOperationCanceled()
{
    var cts = new CancellationTokenSource();
    cts.Cancel();

    await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
        ModelReader.ReadAsync(TestDataPath + "WMM2025.COF", null, cts.Token));
}
```

## Related Issues

- Issue #19: WPF Migration (async patterns align well with MVVM)
- Issue #9: Code duplication in ModelReader (refactor before adding async)
