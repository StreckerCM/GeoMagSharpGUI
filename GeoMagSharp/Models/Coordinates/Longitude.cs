/****************************************************************************
 * File:            Longitude.cs
 * Description:     Geographic longitude coordinate class
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;

namespace GeoMagSharp
{
    /// <summary>
    /// Represents a geographic longitude coordinate (-180 to +180 degrees)
    /// </summary>
    public class Longitude : Coordinate
    {
        protected override string PositiveHemisphere => "E";
        protected override string NegativeHemisphere => "W";

        #region Constructors

        /// <summary>
        /// Initializes a new instance at the prime meridian (0 degrees).
        /// </summary>
        public Longitude()
        {
            Decimal = 0;
        }

        /// <summary>
        /// Initializes a new instance with the specified decimal degree value.
        /// </summary>
        /// <param name="inDecimal">Longitude in decimal degrees (-180 to +180).</param>
        public Longitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        /// <summary>
        /// Initializes a new instance by copying the decimal value from a coordinate.
        /// </summary>
        /// <param name="other">The source coordinate to copy.</param>
        public Longitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        /// <summary>
        /// Initializes a new instance from degrees, minutes, seconds, and hemisphere direction.
        /// </summary>
        /// <param name="inDegrees">Degrees component.</param>
        /// <param name="inMinutes">Minutes component.</param>
        /// <param name="inSeconds">Seconds component.</param>
        /// <param name="inDirection">Hemisphere direction ("E" or "W").</param>
        public Longitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            bool isPositive = inDirection.Equals("E", StringComparison.OrdinalIgnoreCase);
            Decimal = DMSToDecimal(inDegrees, inMinutes, inSeconds, isPositive);
        }

        #endregion
    }
}
