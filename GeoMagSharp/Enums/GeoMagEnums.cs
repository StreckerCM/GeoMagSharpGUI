/****************************************************************************
 * File:            GeoMagEnums.cs
 * Description:     Enumerations used throughout the GeoMagSharp library
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

namespace GeoMagSharp
{
    /// <summary>
    /// Coordinate system type for calculations
    /// </summary>
    public enum CoordinateSystem
    {
        /// <summary>
        /// Geodetic coordinates (latitude, longitude, altitude above ellipsoid)
        /// </summary>
        Geodetic = 1,

        /// <summary>
        /// Geocentric coordinates (spherical)
        /// </summary>
        Geocentric = 2
    }

    /// <summary>
    /// Algorithm implementation to use for calculations
    /// </summary>
    public enum Algorithm
    {
        /// <summary>
        /// British Geological Survey algorithm
        /// </summary>
        BGS = 1,

        /// <summary>
        /// NOAA algorithm
        /// </summary>
        NOAA = 2,

        /// <summary>
        /// MAGVAR algorithm
        /// </summary>
        MAGVAR = 3
    }

    /// <summary>
    /// Unit for magnetic field intensity values
    /// </summary>
    public enum MagneticFieldUnit
    {
        /// <summary>
        /// NanoTesla (default unit for geomagnetic field)
        /// </summary>
        NanoTesla = 1,

        /// <summary>
        /// Gauss (1 Gauss = 100,000 nT)
        /// </summary>
        Gauss = 2
    }

    /// <summary>
    /// Known magnetic model types
    /// </summary>
    public enum knownModels
    {
        /// <summary>
        /// Unknown or unrecognized model type
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Definitive Geomagnetic Reference Field
        /// </summary>
        DGRF = 1,

        /// <summary>
        /// Enhanced Magnetic Model
        /// </summary>
        EMM = 2,

        /// <summary>
        /// International Geomagnetic Reference Field
        /// </summary>
        IGRF = 3,

        /// <summary>
        /// World Magnetic Model
        /// </summary>
        WMM = 4
    }
}
