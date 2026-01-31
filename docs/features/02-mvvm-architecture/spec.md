# MVVM Architecture Refactoring - Specification

## Overview

Refactor the GeoMagSharpGUI application from its current tightly-coupled WinForms architecture to a Model-View-ViewModel (MVVM) pattern, improving testability, maintainability, and separation of concerns.

## User Story

As a developer maintaining this codebase, I want the UI logic separated from business logic so that I can unit test calculations independently and modify the UI without affecting core functionality.

## Current Architecture Problems

### 1. God Object Anti-Pattern
`frmMain.cs` (700+ lines) handles:
- Calculation orchestration
- Model loading and management
- Result formatting and display
- GPS integration
- Coordinate conversions
- Validation logic
- Preference management

### 2. Tightly Coupled Event Handlers
```csharp
// Current: Business logic embedded in UI events
private void textBoxLatitudeDecimal_Validated(object sender, EventArgs e)
{
    var latitude = new Latitude(Convert.ToDouble(textBoxLatitudeDecimal.Text));
    TextBoxLatDeg.Text = latitude.Degrees.ToString("F0");
    // ... more UI manipulation
}
```

### 3. No Testability
- Cannot unit test calculation workflow without instantiating forms
- Cannot verify coordinate conversions without UI
- Cannot test result formatting in isolation

### 4. Manual Re-entrancy Protection
```csharp
public bool _processingEvents;  // Manual flag to prevent circular updates
```

## Requirements

### Functional Requirements

1. **Preserve all existing functionality** - No changes to user-facing behavior
2. **Bidirectional coordinate binding** - Decimal <-> DMS conversion via data binding
3. **Observable results** - Results grid updates automatically from ViewModel
4. **Command-based actions** - Calculate, Load Model, Save Results as commands
5. **Validation through ViewModel** - Input validation in ViewModel, not code-behind

### Non-Functional Requirements

1. **Testability** - All ViewModels must be unit testable without UI
2. **Maintainability** - Clear separation between UI and logic
3. **Minimal Dependencies** - No external MVVM frameworks required
4. **Backward Compatibility** - Existing model classes (GeoMagSharp) unchanged

## Proposed Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                          VIEW                                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   frmMain    │  │frmPreferences│  │  frmAddModel │       │
│  │  (UI Only)   │  │  (UI Only)   │  │  (UI Only)   │       │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘       │
│         │                 │                 │                │
│         │    Data Binding / Commands        │                │
│         ▼                 ▼                 ▼                │
├─────────────────────────────────────────────────────────────┤
│                       VIEWMODEL                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   MainVM     │  │PreferencesVM │  │  AddModelVM  │       │
│  │              │  │              │  │              │       │
│  │ - Coordinates│  │ - Settings   │  │ - FilePath   │       │
│  │ - Options    │  │ - Units      │  │ - ModelInfo  │       │
│  │ - Results    │  │              │  │              │       │
│  │ - Commands   │  │              │  │              │       │
│  └──────┬───────┘  └──────────────┘  └──────────────┘       │
│         │                                                    │
│         │  Uses                                              │
│         ▼                                                    │
├─────────────────────────────────────────────────────────────┤
│                        MODEL                                 │
│  ┌──────────────────────────────────────────────────┐       │
│  │                 GeoMagSharp Library               │       │
│  │  GeoMag, Calculator, MagneticModelSet, etc.      │       │
│  └──────────────────────────────────────────────────┘       │
└─────────────────────────────────────────────────────────────┘
```

## Acceptance Criteria

### Phase 1: Infrastructure
- [ ] INotifyPropertyChanged base class created
- [ ] RelayCommand implementation created
- [ ] ObservableCollection wrapper available

### Phase 2: ViewModels
- [ ] MainViewModel created with all properties
- [ ] CoordinateViewModel handles decimal/DMS conversion
- [ ] CalculateCommand executes calculation workflow
- [ ] ResultsViewModel exposes formatted results

### Phase 3: View Refactoring
- [ ] frmMain uses data binding to MainViewModel
- [ ] Event handlers reduced to single-line command invocations
- [ ] Validation errors shown via ViewModel properties

### Phase 4: Testing
- [ ] Unit tests for MainViewModel calculation workflow
- [ ] Unit tests for coordinate conversion
- [ ] Unit tests for validation logic

### Phase 5: Cleanup
- [ ] `_processingEvents` flag removed
- [ ] Code-behind reduced to < 100 lines
- [ ] All business logic in ViewModels

## Dependencies

- **GeoMagSharp library** - No changes required
- **System.ComponentModel** - For INotifyPropertyChanged
- **.NET Framework 4.5.2** - Current target (supports data binding)

## Open Questions

1. Should we consider migrating to .NET Core/5+ during this refactor?
2. Should preferences be a service with dependency injection?
3. Should GPS integration be abstracted behind an interface for testability?

## Out of Scope

- Migrating from WinForms to WPF
- Adding external MVVM frameworks (Prism, MVVM Light)
- Changing the GeoMagSharp library structure
- Adding new features (this is a pure refactoring effort)
