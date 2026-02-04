/****************************************************************************
 * File:            GeoConstants.cs
 * Description:     Centralized geomagnetic and geodetic constants
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 *
 * This file consolidates all magic numbers and constants used throughout
 * the GeoMagSharp library for maintainability and accuracy verification.
 ****************************************************************************/

using System;

namespace GeoMagSharp
{
    /// <summary>
    /// Centralized constants for geomagnetic calculations.
    /// All physical constants and conversion factors are defined here.
    /// </summary>
    public static class Constants
    {
        #region File Format Constants

        /// <summary>
        /// Standard record length for coefficient files
        /// </summary>
        public const Int32 RecordLen = 80;

        /// <summary>
        /// Maximum number of models allowed in a single file
        /// </summary>
        public const Int32 MaxModules = 30;

        /// <summary>
        /// Maximum spherical harmonic degree supported.
        /// Standard WMM/IGRF use degree 12-13, WMMHR uses degree 18.
        /// Set to 20 to provide headroom for future high-resolution models.
        /// </summary>
        public const Int32 MaxDeg = 20;

        /// <summary>
        /// Maximum number of spherical harmonic coefficients
        /// </summary>
        public static Int32 MaxCoeff
        {
            get
            {
                return (MaxDeg * (MaxDeg + 2) + 1); /* index starts with 1!, (from old Fortran?) */
            }
        }

        #endregion

        #region Numerical Constants

        /// <summary>
        /// Small number for numerical comparisons
        /// </summary>
        public const double SN = 0.0001;

        #endregion

        #region Geographic Limits

        /// <summary>
        /// Latitude limit near North Pole (approximately 300 feet from pole)
        /// </summary>
        public const double ThreeHundredFeetFromNorthPole = 89.999D;

        /// <summary>
        /// Latitude limit near South Pole (approximately 300 feet from pole)
        /// </summary>
        public const double ThreeHundredFeetFromSouthPole = -89.999D;

        /// <summary>
        /// Minimum valid latitude in degrees
        /// </summary>
        public const double MinLatitude = -90.0;

        /// <summary>
        /// Maximum valid latitude in degrees
        /// </summary>
        public const double MaxLatitude = 90.0;

        /// <summary>
        /// Minimum valid longitude in degrees
        /// </summary>
        public const double MinLongitude = -180.0;

        /// <summary>
        /// Maximum valid longitude in degrees
        /// </summary>
        public const double MaxLongitude = 180.0;

        #endregion

        #region Earth Parameters

        /// <summary>
        /// Mean radius of the Earth in kilometers.
        /// Used as the reference radius for spherical harmonic expansion.
        /// </summary>
        public const double EarthsRadiusInKm = 6371.2D;

        #endregion

        #region WGS84 Ellipsoid Parameters

        /// <summary>
        /// WGS84 semi-major axis (equatorial radius) in kilometers
        /// </summary>
        public const double WGS84_SemiMajorAxisKm = 6378.137;

        /// <summary>
        /// WGS84 semi-minor axis (polar radius) in kilometers
        /// </summary>
        public const double WGS84_SemiMinorAxisKm = 6356.7523142;

        /// <summary>
        /// WGS84 semi-major axis squared in km^2.
        /// Value from NOAA GeomagnetismHeader.h: A2WGS84
        /// </summary>
        public const double A2WGS84 = 40680631.59;

        /// <summary>
        /// WGS84 semi-minor axis squared in km^2.
        /// Value from NOAA GeomagnetismHeader.h: B2WGS84
        /// </summary>
        public const double B2WGS84 = 40408299.98;

        /// <summary>
        /// WGS84 flattening factor: f = (a - b) / a
        /// </summary>
        public const double WGS84_Flattening = 1.0 / 298.257223563;

        /// <summary>
        /// WGS84 first eccentricity squared: e^2 = (a^2 - b^2) / a^2
        /// </summary>
        public const double WGS84_EccentricitySquared = 0.00669437999014;

        #endregion

        #region Unit Conversions - Feet

        /// <summary>
        /// Conversion factor: feet to kilometers
        /// </summary>
        public const double FeetToKilometer = 0.0003048;

        /// <summary>
        /// Conversion factor: feet to meters
        /// </summary>
        public const double FeetToMeter = 0.3048;

        #endregion

        #region Unit Conversions - Meters

        /// <summary>
        /// Conversion factor: meters to kilometers
        /// </summary>
        public const double MeterToKilometer = 0.001;

        /// <summary>
        /// Conversion factor: meters to feet
        /// </summary>
        public const double MeterToFeet = 3.28084;

        #endregion

        #region Unit Conversions - Kilometers

        /// <summary>
        /// Conversion factor: kilometers to meters
        /// </summary>
        public const double KilometerToMeter = 1000;

        /// <summary>
        /// Conversion factor: kilometers to feet
        /// </summary>
        public const double KilometerToFeet = 3280.84;

        #endregion

        #region Unit Conversions - Miles

        /// <summary>
        /// Conversion factor: miles to meters
        /// </summary>
        public const double MileToMeter = 1609.34;

        /// <summary>
        /// Conversion factor: miles to kilometers
        /// </summary>
        public const double MileToKilometer = 1.60934;

        #endregion

        #region Magnetic Field Unit Conversions

        /// <summary>
        /// Conversion factor: nanoTesla to Gauss
        /// 1 Gauss = 100,000 nT
        /// </summary>
        public const double NanoTeslaToGauss = 0.00001;

        /// <summary>
        /// Conversion factor: Gauss to nanoTesla
        /// </summary>
        public const double GaussToNanoTesla = 100000;

        #endregion
    }
}
