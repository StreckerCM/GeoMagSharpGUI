# MVVM Architecture Refactoring - Implementation Plan

## Overview

This plan outlines a phased approach to refactoring the GeoMagSharpGUI application to MVVM architecture. Each phase is designed to be independently mergeable while maintaining application functionality.

## Phase 1: MVVM Infrastructure

### Goals
- Create reusable MVVM base classes
- Establish patterns for the rest of the refactoring

### Tasks
1. Create `ViewModelBase` class implementing `INotifyPropertyChanged`
2. Create `RelayCommand` class implementing `ICommand`
3. Create `ObservableObject` helper for nested property change notifications
4. Add unit tests for infrastructure classes

### Files to Create
- `GeoMagGUI/MVVM/ViewModelBase.cs`
- `GeoMagGUI/MVVM/RelayCommand.cs`
- `GeoMagGUI/MVVM/ObservableObject.cs`
- `GeoMagSharp-UnitTests/MVVMInfrastructureTests.cs`

### Code Examples

```csharp
// ViewModelBase.cs
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

// RelayCommand.cs
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object parameter) => _execute(parameter);
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
```

---

## Phase 2: Coordinate ViewModel

### Goals
- Extract coordinate handling from frmMain
- Implement bidirectional decimal/DMS conversion via binding
- Remove `_processingEvents` flag

### Tasks
1. Create `CoordinateViewModel` with decimal and DMS properties
2. Implement automatic conversion when either changes
3. Create unit tests for coordinate conversions
4. Wire up data binding in frmMain for latitude controls
5. Wire up data binding in frmMain for longitude controls

### Files to Create/Modify
- `GeoMagGUI/ViewModels/CoordinateViewModel.cs`
- `GeoMagSharp-UnitTests/CoordinateViewModelTests.cs`
- `GeoMagGUI/frmMain.cs` (modify)
- `GeoMagGUI/frmMain.Designer.cs` (modify bindings)

### Code Example

```csharp
public class CoordinateViewModel : ViewModelBase
{
    private double _decimal;
    private int _degrees;
    private int _minutes;
    private double _seconds;
    private string _hemisphere;
    private bool _isUpdating;
    private readonly double _minValue;
    private readonly double _maxValue;
    private readonly string[] _hemispheres;

    public CoordinateViewModel(double min, double max, string[] hemispheres)
    {
        _minValue = min;
        _maxValue = max;
        _hemispheres = hemispheres;
    }

    public double Decimal
    {
        get => _decimal;
        set
        {
            if (_isUpdating) return;
            if (SetProperty(ref _decimal, value))
            {
                _isUpdating = true;
                UpdateDMSFromDecimal();
                _isUpdating = false;
            }
        }
    }

    public int Degrees { get => _degrees; set => SetProperty(ref _degrees, value); }
    public int Minutes { get => _minutes; set => SetProperty(ref _minutes, value); }
    public double Seconds { get => _seconds; set => SetProperty(ref _seconds, value); }
    public string Hemisphere { get => _hemisphere; set => SetProperty(ref _hemisphere, value); }

    private void UpdateDMSFromDecimal()
    {
        // Use existing Latitude/Longitude classes for conversion
    }

    private void UpdateDecimalFromDMS()
    {
        // Use existing Latitude/Longitude classes for conversion
    }
}
```

---

## Phase 3: Main ViewModel - Properties

### Goals
- Create MainViewModel with all form properties
- Move validation logic to ViewModel
- Expose validation errors via properties

### Tasks
1. Create `MainViewModel` extending `ViewModelBase`
2. Add coordinate properties (using `CoordinateViewModel`)
3. Add altitude, date range, step size properties
4. Add validation error properties
5. Implement `IDataErrorInfo` for WinForms validation
6. Create unit tests for validation

### Files to Create/Modify
- `GeoMagGUI/ViewModels/MainViewModel.cs`
- `GeoMagSharp-UnitTests/MainViewModelTests.cs`
- `GeoMagGUI/frmMain.cs` (modify)

### Properties to Migrate

| Form Control | ViewModel Property | Type |
|-------------|-------------------|------|
| textBoxLatitudeDecimal | Latitude.Decimal | double |
| TextBoxLatDeg/Min/Sec | Latitude.Degrees/Minutes/Seconds | int/int/double |
| textBoxLongitudeDecimal | Longitude.Decimal | double |
| TextBoxLongDeg/Min/Sec | Longitude.Degrees/Minutes/Seconds | int/int/double |
| textBoxAltitude | Altitude | double |
| comboBoxAltitudeUnits | AltitudeUnit | ElevationUnits |
| dateTimePicker1 | StartDate | DateTime |
| dateTimePicker2 | EndDate | DateTime |
| numericUpDownStepSize | StepSize | int |
| comboBoxModels | SelectedModel | MagneticModelInfo |

---

## Phase 4: Main ViewModel - Commands

### Goals
- Convert button click handlers to Commands
- Remove business logic from code-behind
- Enable command CanExecute based on validation

### Tasks
1. Create `CalculateCommand` in MainViewModel
2. Create `LoadModelCommand` in MainViewModel
3. Create `SaveResultsCommand` in MainViewModel
4. Create `ClearResultsCommand` in MainViewModel
5. Bind buttons to commands
6. Implement CanExecute logic based on validation state

### Commands to Create

```csharp
public ICommand CalculateCommand { get; }
public ICommand LoadModelCommand { get; }
public ICommand SaveResultsCommand { get; }
public ICommand ClearResultsCommand { get; }
public ICommand GetGPSLocationCommand { get; }
```

---

## Phase 5: Results ViewModel

### Goals
- Create observable results collection
- Move result formatting to ViewModel
- Enable automatic grid updates

### Tasks
1. Create `CalculationResultViewModel` for individual results
2. Add `ObservableCollection<CalculationResultViewModel>` to MainViewModel
3. Bind DataGridView to results collection
4. Move formatting logic from grid population to ViewModel properties

### Files to Create/Modify
- `GeoMagGUI/ViewModels/CalculationResultViewModel.cs`
- `GeoMagGUI/ViewModels/MainViewModel.cs` (add Results property)

### Code Example

```csharp
public class CalculationResultViewModel : ViewModelBase
{
    private readonly MagneticCalculations _result;

    public CalculationResultViewModel(MagneticCalculations result)
    {
        _result = result;
    }

    public string Date => _result.Date.ToShortDateString();
    public string Declination => $"{_result.Declination.Value:F4}°";
    public string Inclination => $"{_result.Inclination.Value:F4}°";
    public string HorizontalIntensity => $"{_result.HorizontalIntensity.Value:F2} nT";
    // ... other formatted properties
}
```

---

## Phase 6: Preferences ViewModel

### Goals
- Extract preferences logic from frmPreferences
- Create reusable preferences ViewModel

### Tasks
1. Create `PreferencesViewModel` with all settings
2. Modify frmPreferences to use ViewModel
3. Add save/cancel command logic

### Files to Create/Modify
- `GeoMagGUI/ViewModels/PreferencesViewModel.cs`
- `GeoMagGUI/frmPreferences.cs` (modify)

---

## Phase 7: Cleanup and Testing

### Goals
- Remove legacy code-behind logic
- Ensure comprehensive test coverage
- Document new architecture

### Tasks
1. Remove `_processingEvents` flag
2. Reduce frmMain code-behind to < 100 lines
3. Add integration tests
4. Update AGENTS.md with architecture documentation
5. Create developer guide for MVVM patterns

---

## Testing Strategy

### Unit Tests (Required)
- ViewModelBase property change notifications
- RelayCommand execution and CanExecute
- CoordinateViewModel decimal/DMS conversions
- MainViewModel validation logic
- MainViewModel command execution
- CalculationResultViewModel formatting

### Integration Tests (Recommended)
- Full calculation workflow through ViewModel
- Model loading through ViewModel
- Results export through ViewModel

### Manual Testing
- Verify all existing functionality preserved
- Test coordinate entry both ways
- Test validation error display
- Test GPS integration

---

## Rollback Plan

Each phase is designed to be independently reversible:
1. Keep original code commented until phase is stable
2. Feature branch per phase
3. Full test coverage before merge
4. Phase-by-phase PRs for review

---

## Estimated Effort

| Phase | Complexity | Files Changed |
|-------|------------|---------------|
| 1. Infrastructure | Low | 3-4 new files |
| 2. Coordinates | Medium | 2 new, 2 modified |
| 3. Properties | Medium | 1 new, 1 modified |
| 4. Commands | Medium | 1 modified |
| 5. Results | Medium | 2 new, 1 modified |
| 6. Preferences | Low | 1 new, 1 modified |
| 7. Cleanup | Low | 2-3 modified |

Total new files: ~10
Total modified files: ~6
