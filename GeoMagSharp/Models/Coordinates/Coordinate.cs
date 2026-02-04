/****************************************************************************
 * File:            Coordinate.cs
 * Description:     Abstract base class for geographic coordinates
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;

namespace GeoMagSharp
{
    /// <summary>
    /// Abstract base class for geographic coordinates (Latitude/Longitude).
    /// Provides common functionality for decimal degrees and DMS (Degrees/Minutes/Seconds) conversion.
    /// </summary>
    public abstract class Coordinate
    {
        /// <summary>
        /// The hemisphere identifier for positive values (N for Latitude, E for Longitude)
        /// </summary>
        protected abstract string PositiveHemisphere { get; }

        /// <summary>
        /// The hemisphere identifier for negative values (S for Latitude, W for Longitude)
        /// </summary>
        protected abstract string NegativeHemisphere { get; }

        /// <summary>
        /// The coordinate value in decimal degrees
        /// </summary>
        public double Decimal { get; set; }

        /// <summary>
        /// The whole degrees component of the DMS representation
        /// </summary>
        public double Degrees
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);
                return absDecimal.Truncate();
            }
        }

        /// <summary>
        /// The minutes component of the DMS representation
        /// </summary>
        public double Minutes
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);
                absDecimal -= absDecimal.Truncate();
                return (absDecimal * 60).Truncate();
            }
        }

        /// <summary>
        /// The seconds component of the DMS representation
        /// </summary>
        public double Seconds
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);
                absDecimal -= absDecimal.Truncate();
                absDecimal *= 60;
                absDecimal -= absDecimal.Truncate();
                return absDecimal * 60;
            }
        }

        /// <summary>
        /// The hemisphere indicator (N/S for Latitude, E/W for Longitude)
        /// </summary>
        public string Hemisphere
        {
            get
            {
                return Decimal >= 0 ? PositiveHemisphere : NegativeHemisphere;
            }
        }

        /// <summary>
        /// Formats the coordinate as a DMS string (e.g., "45° 30′ 15.0000″ N")
        /// </summary>
        public string ToStringDMS
        {
            get
            {
                return string.Format("{0}° {1}′ {2}″ {3}", Degrees, Minutes, Seconds.ToString("F4"), Hemisphere);
            }
        }

        /// <summary>
        /// Converts DMS components to decimal degrees
        /// </summary>
        /// <param name="degrees">Degrees component</param>
        /// <param name="minutes">Minutes component</param>
        /// <param name="seconds">Seconds component</param>
        /// <param name="isPositiveHemisphere">True if positive hemisphere (N or E)</param>
        /// <returns>Decimal degrees value</returns>
        protected double DMSToDecimal(double degrees, double minutes, double seconds, bool isPositiveHemisphere)
        {
            double coordDec = (minutes * 60) + seconds;
            coordDec = coordDec / 3600;  // Total number of seconds
            coordDec = degrees + coordDec;

            if (isPositiveHemisphere)
            {
                coordDec = Math.Abs(coordDec);
            }
            else
            {
                coordDec = -Math.Abs(coordDec);
            }

            return coordDec;
        }
    }
}
