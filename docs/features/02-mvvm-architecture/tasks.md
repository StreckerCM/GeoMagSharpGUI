# MVVM Architecture Refactoring - Task List

## Phase 1: MVVM Infrastructure
- [ ] Create `GeoMagGUI/MVVM/` folder
- [ ] Create `ViewModelBase.cs` with INotifyPropertyChanged
- [ ] Create `RelayCommand.cs` with ICommand implementation
- [ ] Create `ObservableObject.cs` helper class
- [ ] Add unit tests for `ViewModelBase.SetProperty()`
- [ ] Add unit tests for `RelayCommand.Execute()` and `CanExecute()`

## Phase 2: Coordinate ViewModel
- [ ] Create `GeoMagGUI/ViewModels/` folder
- [ ] Create `CoordinateViewModel.cs`
- [ ] Implement decimal property with DMS auto-update
- [ ] Implement DMS properties with decimal auto-update
- [ ] Add validation for coordinate ranges
- [ ] Add unit tests for decimal-to-DMS conversion
- [ ] Add unit tests for DMS-to-decimal conversion
- [ ] Add unit tests for validation
- [ ] Replace latitude textbox event handlers with binding
- [ ] Replace longitude textbox event handlers with binding
- [ ] Remove `_processingEvents` flag usage for coordinates

## Phase 3: Main ViewModel - Properties
- [ ] Create `MainViewModel.cs`
- [ ] Add `Latitude` property (CoordinateViewModel)
- [ ] Add `Longitude` property (CoordinateViewModel)
- [ ] Add `Altitude` property with validation
- [ ] Add `AltitudeUnit` property (enum)
- [ ] Add `StartDate` property
- [ ] Add `EndDate` property
- [ ] Add `StepSize` property
- [ ] Add `SelectedModel` property
- [ ] Add `AvailableModels` collection
- [ ] Implement `IDataErrorInfo` for validation
- [ ] Add `HasErrors` property
- [ ] Add unit tests for property validation
- [ ] Add unit tests for date range validation

## Phase 4: Main ViewModel - Commands
- [ ] Add `CalculateCommand` property
- [ ] Implement calculation logic in command
- [ ] Add `CanExecute` based on validation state
- [ ] Add `LoadModelCommand` property
- [ ] Add `SaveResultsCommand` property
- [ ] Add `ClearResultsCommand` property
- [ ] Add `GetGPSLocationCommand` property
- [ ] Bind Calculate button to command
- [ ] Bind menu items to commands
- [ ] Add unit tests for command execution
- [ ] Add unit tests for CanExecute logic

## Phase 5: Results ViewModel
- [ ] Create `CalculationResultViewModel.cs`
- [ ] Add formatted properties (Declination, Inclination, etc.)
- [ ] Add `Results` ObservableCollection to MainViewModel
- [ ] Add `ChangePerYearResult` property for summary row
- [ ] Bind DataGridView to Results collection
- [ ] Remove manual grid population code
- [ ] Add unit tests for result formatting

## Phase 6: Preferences ViewModel
- [ ] Create `PreferencesViewModel.cs`
- [ ] Add all preference properties
- [ ] Add `SaveCommand` and `CancelCommand`
- [ ] Modify `frmPreferences` to use ViewModel
- [ ] Add data binding in preferences form
- [ ] Add unit tests for preferences

## Phase 7: Cleanup and Testing
- [ ] Remove `_processingEvents` field entirely
- [ ] Remove unused event handlers from frmMain
- [ ] Reduce frmMain.cs code-behind to < 100 lines
- [ ] Add integration test for full calculation workflow
- [ ] Update AGENTS.md with architecture section
- [ ] Document MVVM patterns used
- [ ] Final manual testing of all features

## Verification
- [ ] Build succeeds with no warnings
- [ ] All unit tests pass
- [ ] All existing functionality preserved
- [ ] Code coverage > 80% for ViewModels
- [ ] Manual testing complete per PR checklist

## Documentation
- [ ] Update README with architecture overview
- [ ] Add developer guide for extending ViewModels
- [ ] Document data binding patterns used
- [ ] Update feature status to Complete
