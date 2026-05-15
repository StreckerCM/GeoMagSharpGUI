# GeoMagSharpGUI

A C# WinForms application for calculating geomagnetic field values using spherical harmonic models. This is a port of the NOAA Geomag 7.0 software with a modern graphical interface.

> **Note:** The core calculation library has been split into its own repository and is available as a standalone NuGet package:
> **[GeoMagSharp](https://github.com/StreckerCM/GeoMagSharp)** | [NuGet](https://www.nuget.org/packages/GeoMagSharp)
>
> This repo contains only the GUI application, which consumes GeoMagSharp via NuGet.

## Overview

GeoMagSharpGUI calculates Earth's magnetic field components (declination, inclination, intensity) at any location and date using coefficient files from models like:

- **WMM** / **WMMHR** (World Magnetic Model and high-resolution variant)
- **IGRF** / **DGRF** (International / Definitive Geomagnetic Reference Field)
- **EMM** (Enhanced Magnetic Model)
- **BGGM** (BGS Global Geomagnetic Model — commercial, user-supplied)
- **HDGM** (High Definition Geomagnetic Model — Windows-only, NOAA DLL)

## Features

- Calculate magnetic declination, inclination, and field intensity
- Auto-discovery of model files in the `coefficient/` folder (`.COF`, `.DAT`, HDGM `.DLL`)
- HDGM (High Definition Geomagnetic Model) support via NOAA-supplied DLL (Windows-only)
- 1-sigma uncertainty shown in the results grid + side detail panel,
  populated from the loaded model's native error model where one exists
  (WMM/WMMHR Tech Report formula, HDGM per-point sigmas) with ISCWSA
  Level 1 fallback for IGRF/DGRF/EMM/BGGM
- Master-detail layout: values-only grid on the left, full breakdown
  (declination, inclination, all components, σ rows, model metadata
  chips) on the right
- Model metadata surfaced from `ModelDescriptor` 1.7.2 fields: degree
  chip, altitude validity row, epoch count, σ source label
- Support for multiple coordinate input formats (Decimal Degrees, DMS)
- GPS location integration via Windows Location Services
- Historical calculations with date range support
- Multiple output units (nanoTesla, Gauss)
- Secular variation (change per year) calculations
- Async calculation with cancellation and progress reporting
- User-configurable preferences

## Solution Structure

```
GeoMagGUI.sln
├── GeoMagGUI/              # WinForms Application (.NET Framework 4.8)
│   ├── coefficient/        # Coefficient files populated from GeoMagSharp
│   │                       # NuGet at build time (WMM2025, WMMHR, IGRF14, etc.).
│   │                       # Drop additional .COF / .DAT / HDGM .DLL files
│   │                       # here — they are auto-discovered on launch.
│   ├── assets/             # Icons and images
│   ├── documentation/      # License and docs
│   ├── frmMain.cs          # Main application window
│   ├── frmAddModel.cs      # Add model dialog
│   ├── frmPreferences.cs   # User preferences dialog
│   └── Program.cs          # Application entry point
│
├── Installer/              # WiX MSI installer
│
└── docs/                   # Documentation
    ├── prompts/            # Claude personas and prompt templates
    └── features/           # Feature specifications and plans
```

The core calculation library ([GeoMagSharp](https://github.com/StreckerCM/GeoMagSharp)) and its unit tests are maintained in a separate repository and consumed via NuGet.

## Build Requirements

- Visual Studio 2019 or later
- .NET Framework 4.8

## Build Commands

Use Developer Command Prompt for Visual Studio:

```bash
# Debug build (x86)
msbuild GeoMagGUI.sln /p:Configuration=Debug /p:Platform="x86"

# Release build (x86)
msbuild GeoMagGUI.sln /p:Configuration=Release /p:Platform="x86"
```

NuGet restore automatically downloads the [GeoMagSharp](https://www.nuget.org/packages/GeoMagSharp) package.

## Usage

1. **Select a Magnetic Model** - Choose from available models (WMM, WMMHR, IGRF, EMM, BGGM, HDGM)
2. **Enter Location** - Input coordinates as decimal degrees or DMS
3. **Set Elevation** - Enter altitude above MSL or depth below MSL
4. **Choose Date(s)** - Select single date or date range
5. **Calculate** - View magnetic field results in the grid
6. **Inspect a Row** - Selecting a grid row populates the side detail panel with the full breakdown (all seven components, 1-σ uncertainty per component, model metadata chips, validity range, epoch count)

## Magnetic Field Components

| Component | Description |
|-----------|-------------|
| Declination | Angle between true north and magnetic north |
| Inclination | Angle between horizontal and magnetic field vector |
| Horizontal Intensity | Horizontal component of magnetic field |
| North Component | Northward component of horizontal intensity |
| East Component | Eastward component of horizontal intensity |
| Vertical Component | Vertical component of magnetic field |
| Total Field | Total magnetic field intensity |

## Adding New Models

The GUI uses GeoMagSharp's `ModelDiscovery` API to scan the `coefficient/` folder on launch. Any supported file dropped into that folder is auto-discovered — no manual registration required.

1. Obtain a coefficient file (`.COF`, `.DAT`, or HDGM `.DLL`)
2. Drop it into `GeoMagGUI/bin/Debug/coefficient/` (or `bin/Release/coefficient/`)
3. Restart the app — the model appears in the dropdown automatically

You can also use **File > Add Model** to pick a file from anywhere on disk; the GUI copies it into the coefficient folder and refreshes the dropdown. The Add Model dialog filters for `.cof` / `.dat` / `.dll` separately or all at once.

For multi-epoch IGRF/DGRF files, the dropdown shows the latest epoch label (e.g. `IGRF2025` for IGRF14.COF) covering the file's full validity range.

## Dependencies

- **[GeoMagSharp](https://www.nuget.org/packages/GeoMagSharp)** - Geomagnetic field calculation library (latest version pinned in `GeoMagGUI.csproj`)
- **System.Device** - GPS location services (Windows)
- **Newtonsoft.Json** - JSON serialization for preferences and (legacy) model registry

## Architecture

```
┌─────────────────────────────────────┐
│   Presentation (WinForms)           │
│   frmMain, dialogs, UI controls     │
├─────────────────────────────────────┤
│   GeoMagSharp (NuGet package)       │
│   GeoMag, Calculator, Models        │
│   ModelReader, JSON serialization   │
├─────────────────────────────────────┤
│   External Data                     │
│   COF/DAT coefficient files         │
└─────────────────────────────────────┘
```

## Contributing

See [AGENTS.md](./AGENTS.md) for coding standards and development guidelines.

For Claude Code assistance, see [docs/prompts/](./docs/prompts/) for personas and templates.

## License

See [GeoMagGUI/documentation/LICENSE](./GeoMagGUI/documentation/LICENSE)

## Credits

- NOAA World Magnetic Model (WMM) and Geomag 7.0 software
- Port to C# with GUI by StreckerCM
