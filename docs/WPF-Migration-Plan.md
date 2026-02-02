# WPF Migration Plan for GeoMagGUI

## Executive Summary

This document outlines a detailed migration plan for converting the GeoMagGUI WinForms application to a WPF (Windows Presentation Foundation) application using the MVVM (Model-View-ViewModel) architectural pattern.

**Estimated Effort:** 4-6 weeks
**Target Framework:** .NET 6+ (or .NET Framework 4.8 for compatibility)
**Architecture:** MVVM with dependency injection

---

## 1. Current Application Analysis

### 1.1 Form Inventory

| Form | Purpose | Complexity |
|------|---------|------------|
| **FrmMain** | Main calculation interface | HIGH (~700 lines) |
| **frmAddModel** | Add magnetic model dialog | MEDIUM (~100 lines) |
| **frmPreferences** | User preferences dialog | LOW (~83 lines) |
| **AboutBoxGeoMag** | About dialog | LOW (~104 lines) |

### 1.2 Key Dependencies

- **GeoMagSharp Library**: Core calculation engine (unchanged)
- **System.Device.Location**: GeoCoordinateWatcher for GPS
- **Newtonsoft.Json**: Preferences and model serialization

### 1.3 Current Data Flow

```
User Input (UI Controls)
    ↓
FrmMain (Event Handlers)
    ↓
CalculationOptions (GeoMagSharp)
    ↓
GeoMag.MagneticCalculations()
    ↓
Calculator.SpotCalculation()
    ↓
MagneticCalculations Results
    ↓
DataGridView Display
```

---

## 2. Proposed WPF Project Structure

```
GeoMagGUI.Wpf/
├── App.xaml
├── App.xaml.cs
├── Models/
│   ├── CalculationResultModel.cs
│   └── CoordinateModel.cs
├── ViewModels/
│   ├── Base/
│   │   ├── ViewModelBase.cs
│   │   └── RelayCommand.cs
│   ├── MainViewModel.cs
│   ├── AddModelViewModel.cs
│   ├── PreferencesViewModel.cs
│   └── AboutViewModel.cs
├── Views/
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   └── Dialogs/
│       ├── AddModelDialog.xaml
│       ├── PreferencesDialog.xaml
│       └── AboutDialog.xaml
├── Services/
│   ├── Interfaces/
│   │   ├── IDialogService.cs
│   │   ├── IFileService.cs
│   │   ├── ILocationService.cs
│   │   ├── IMagneticCalculationService.cs
│   │   └── IPreferencesService.cs
│   └── Implementations/
├── Converters/
├── Validation/
├── Resources/
│   ├── Styles/
│   └── Icons/
└── Helpers/
```

---

## 3. MVVM Architecture Design

### 3.1 MainViewModel Properties

| Property | Type | Purpose |
|----------|------|---------|
| `SelectedModel` | `MagneticModelSet` | Model selection |
| `Models` | `ObservableCollection<MagneticModelSet>` | Available models |
| `LatitudeDecimal` | `double` | Decimal latitude input |
| `LatitudeDegrees` | `int` | DMS degrees |
| `LatitudeMinutes` | `int` | DMS minutes |
| `LatitudeSeconds` | `double` | DMS seconds |
| `LatitudeHemisphere` | `string` | N/S selector |
| `LongitudeDecimal` | `double` | Decimal longitude input |
| `LongitudeDegrees` | `int` | DMS degrees |
| `LongitudeMinutes` | `int` | DMS minutes |
| `LongitudeSeconds` | `double` | DMS seconds |
| `LongitudeHemisphere` | `string` | E/W selector |
| `Altitude` | `double` | Altitude/depth value |
| `AltitudeUnit` | `string` | km/m/ft selector |
| `StartDate` | `DateTime` | Start date |
| `EndDate` | `DateTime` | End date (range mode) |
| `StepSize` | `int` | Step size for range |
| `UseRangeOfDates` | `bool` | Date range toggle |
| `UseDecimalDegrees` | `bool` | Coordinate format |
| `CalculationResults` | `ObservableCollection<CalculationResultModel>` | Results grid |
| `IsBusy` | `bool` | Loading indicator |

### 3.2 MainViewModel Commands

| Command | Purpose |
|---------|---------|
| `CalculateCommand` | Perform calculation |
| `AddModelCommand` | Open add model dialog |
| `LoadModelCommand` | Load model from file |
| `SaveResultsCommand` | Save calculation results |
| `GetMyLocationCommand` | Get GPS coordinates |
| `OpenPreferencesCommand` | Open preferences |
| `OpenAboutCommand` | Open about dialog |
| `ExitCommand` | Close application |

### 3.3 Service Interfaces

```csharp
public interface IDialogService
{
    bool? ShowDialog<TViewModel>(TViewModel viewModel);
    void ShowMessage(string message, string title, MessageType type);
    string ShowOpenFileDialog(string filter, string title);
    string ShowSaveFileDialog(string filter, string title, string defaultFileName);
}

public interface ILocationService
{
    event EventHandler<LocationChangedEventArgs> LocationChanged;
    void StartWatching();
    void StopWatching();
    bool IsAvailable { get; }
}

public interface IMagneticCalculationService
{
    Task<List<MagneticCalculations>> CalculateAsync(
        MagneticModelSet model,
        CalculationOptions options);
}

public interface IPreferencesService
{
    Preferences Load();
    void Save(Preferences preferences);
}
```

---

## 4. WinForms to WPF Control Mapping

| WinForms Control | WPF Equivalent | Notes |
|-----------------|----------------|-------|
| `Form` | `Window` | |
| `TextBox` | `TextBox` | Add `UpdateSourceTrigger=PropertyChanged` |
| `ComboBox` | `ComboBox` | Use `SelectedItem` binding |
| `Button` | `Button` | Use `Command` binding |
| `Label` | `TextBlock` | |
| `DateTimePicker` | `DatePicker` | |
| `NumericUpDown` | Custom/Extended WPF Toolkit | |
| `DataGridView` | `DataGrid` | Use `ItemsSource` binding |
| `MenuStrip` | `Menu` | |
| `ErrorProvider` | `Validation.ErrorTemplate` | |
| `TableLayoutPanel` | `Grid` | |
| `PictureBox` | `Image` | |

---

## 5. Migration Phases

### Phase 1: Project Setup (Week 1)

- [ ] Create new WPF project (.NET 6+ or .NET Framework 4.8)
- [ ] Add reference to GeoMagSharp library
- [ ] Implement ViewModelBase and RelayCommand
- [ ] Set up service interfaces
- [ ] Create resource dictionaries for styles

### Phase 2: Core ViewModels and Services (Week 2)

- [ ] Implement MainViewModel with all properties
- [ ] Implement PreferencesService
- [ ] Implement FileService
- [ ] Implement LocationService (GeoCoordinateWatcher wrapper)
- [ ] Implement MagneticCalculationService
- [ ] Unit tests for services

### Phase 3: Main Window View (Week 3)

- [ ] Create MainWindow.xaml layout
- [ ] Implement coordinate input section
- [ ] Implement date selection section
- [ ] Implement altitude/model selection
- [ ] Implement results DataGrid
- [ ] Implement menu structure
- [ ] Wire up all data bindings

### Phase 4: Dialog Windows (Week 4)

- [ ] Implement DialogService
- [ ] Create AddModelDialog with ViewModel
- [ ] Create PreferencesDialog with ViewModel
- [ ] Create AboutDialog with ViewModel
- [ ] Integrate dialogs with MainViewModel

### Phase 5: Validation and Error Handling (Week 5)

- [ ] Implement INotifyDataErrorInfo validation
- [ ] Create validation rules for coordinates
- [ ] Create validation rules for numeric inputs
- [ ] Implement error templates
- [ ] Add error handling for file operations

### Phase 6: Testing and Polish (Week 6)

- [ ] Integration testing
- [ ] Test with various coefficient files
- [ ] Test GPS location functionality
- [ ] UI polish and styling
- [ ] Performance optimization
- [ ] Documentation updates

---

## 6. Bidirectional Coordinate Conversion

The current WinForms implementation uses a `_processingEvents` flag to prevent recursive updates. In MVVM:

```csharp
private bool _isUpdatingCoordinates;

private double _latitudeDecimal;
public double LatitudeDecimal
{
    get => _latitudeDecimal;
    set
    {
        if (SetProperty(ref _latitudeDecimal, value) && !_isUpdatingCoordinates)
        {
            _isUpdatingCoordinates = true;
            UpdateLatitudeDMSFromDecimal(value);
            _isUpdatingCoordinates = false;
        }
    }
}

private void UpdateLatitudeDMSFromDecimal(double decimalValue)
{
    var latitude = new Latitude(decimalValue);
    LatitudeDegrees = (int)latitude.Degrees;
    LatitudeMinutes = (int)latitude.Minutes;
    LatitudeSeconds = latitude.Seconds;
    LatitudeHemisphere = latitude.Hemisphere;
}
```

---

## 7. NuGet Packages

```xml
<!-- MVVM Toolkit -->
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.*" />

<!-- Better dialogs (FolderBrowserDialog replacement) -->
<PackageReference Include="Ookii.Dialogs.Wpf" Version="5.*" />

<!-- NumericUpDown control -->
<PackageReference Include="Extended.Wpf.Toolkit" Version="4.*" />
```

---

## 8. Risks and Mitigation

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| GeoCoordinateWatcher issues | Medium | High | Test early; fallback to manual entry |
| Performance regression | Low | Medium | Profile early; virtualize DataGrid |
| MVVM learning curve | Medium | Medium | Follow patterns; code reviews |
| Third-party control needed | High | Low | Use Extended WPF Toolkit |

---

## 9. Backward Compatibility

These files must remain compatible:
- `ApplicationPreferences.json` format
- `MagneticModels.json` format
- Coefficient files (.COF, .DAT)
- Export results format (tab-delimited)

---

## 10. Testing Checklist

- [ ] Calculate with WMM2015, WMM2020, WMM2025 models
- [ ] Calculate with IGRF12 model
- [ ] Add new model via dialog
- [ ] Load model from file
- [ ] Save calculation results
- [ ] GPS location retrieval
- [ ] Date range calculations
- [ ] Coordinate format switching (decimal ↔ DMS)
- [ ] Altitude/Depth toggle
- [ ] All validation error messages
- [ ] Menu keyboard shortcuts
- [ ] Window resizing behavior
- [ ] High DPI display support

---

## 11. Key Files Reference

| Current File | Purpose | Migration Target |
|--------------|---------|------------------|
| `frmMain.cs` | Main UI logic | `MainViewModel.cs` |
| `frmMain.Designer.cs` | Layout | `MainWindow.xaml` |
| `frmAddModel.cs` | Add model dialog | `AddModelViewModel.cs` |
| `frmPreferences.cs` | Preferences | `PreferencesViewModel.cs` |
| `Helper.cs` | Validation utilities | `ValidationHelper.cs` |
| `DataModel.cs` | Data contracts | Unchanged (in GeoMagSharp) |
| `GeoMag.cs` | Calculation engine | Wrapped in service |

---

## 12. Success Criteria

1. All existing functionality preserved
2. All unit tests passing
3. Manual testing checklist complete
4. Performance equal or better than WinForms version
5. Modern, maintainable MVVM architecture
6. Proper separation of concerns
7. Testable ViewModels with dependency injection
