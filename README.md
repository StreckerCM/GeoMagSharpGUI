# GeoMagSharpGUI

A C# WinForms application for calculating geomagnetic field values using spherical harmonic models. This is a port of the NOAA Geomag 7.0 software with a modern graphical interface.

> **Note:** The core calculation library has been split into its own repository and is available as a standalone NuGet package:
> **[GeoMagSharp](https://github.com/StreckerCM/GeoMagSharp)** | [NuGet](https://www.nuget.org/packages/GeoMagSharp)
>
> This repo contains only the GUI application, which consumes GeoMagSharp via NuGet.

## Overview

GeoMagSharpGUI calculates Earth's magnetic field components (declination, inclination, intensity) at any location and date using coefficient files from models like:

- **IGRF** (International Geomagnetic Reference Field)
- **WMM** (World Magnetic Model)
- **DGRF** (Definitive Geomagnetic Reference Field)
- **EMM** (Enhanced Magnetic Model)

## Features

- Calculate magnetic declination, inclination, and field intensity
- Support for multiple coordinate input formats (Decimal Degrees, DMS)
- GPS location integration via Windows Location Services
- Historical calculations with date range support
- Multiple output units (nanoTesla, Gauss)
- Secular variation (change per year) calculations
- Save results to file
- User-configurable preferences

## Solution Structure

```
GeoMagGUI.sln
├── GeoMagGUI/              # WinForms Application (.NET Framework 4.0)
│   ├── coefficient/        # Magnetic model coefficient files
│   │   ├── IGRF12.COF
│   │   ├── WMM2015.COF
│   │   └── MagneticModels.json
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
- .NET Framework 4.0

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

1. **Select a Magnetic Model** - Choose from available models (IGRF, WMM, etc.)
2. **Enter Location** - Input coordinates as decimal degrees or DMS
3. **Set Elevation** - Enter altitude above MSL or depth below MSL
4. **Choose Date(s)** - Select single date or date range
5. **Calculate** - View magnetic field results in the grid
6. **Save Results** - Export to text file if needed

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

1. Obtain coefficient file (.COF or .DAT format)
2. Place in `GeoMagGUI/coefficient/` directory
3. Use Tools > Add Model to register the model
4. Model will be available in the dropdown

## Dependencies

- **[GeoMagSharp](https://www.nuget.org/packages/GeoMagSharp)** v1.4.0 - Geomagnetic field calculation library
- **System.Device** - GPS location services (Windows)

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
