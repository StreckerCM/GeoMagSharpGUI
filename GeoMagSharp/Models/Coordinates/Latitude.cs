/****************************************************************************
 * File:            Latitude.cs
 * Description:     Geographic latitude coordinate class
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;

namespace GeoMagSharp
{
    /// <summary>
    /// Represents a geographic latitude coordinate (-90 to +90 degrees)
    /// </summary>
    public class Latitude : Coordinate
    {
        protected override string PositiveHemisphere => "N";
        protected override string NegativeHemisphere => "S";

        #region Constructors

        /// <summary>
        /// Initializes a new instance at the equator (0 degrees).
        /// </summary>
        public Latitude()
        {
            Decimal = 0;
        }

        /// <summary>
        /// Initializes a new instance with the specified decimal degree value.
        /// </summary>
        /// <param name="inDecimal">Latitude in decimal degrees (-90 to +90).</param>
        public Latitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        /// <summary>
        /// Initializes a new instance by copying another <see cref="Latitude"/>.
        /// </summary>
        /// <param name="other">The source latitude to copy.</param>
        public Latitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        /// <summary>
        /// Initializes a new instance from degrees, minutes, seconds, and hemisphere direction.
        /// </summary>
        /// <param name="inDegrees">Degrees component.</param>
        /// <param name="inMinutes">Minutes component.</param>
        /// <param name="inSeconds">Seconds component.</param>
        /// <param name="inDirection">Hemisphere direction ("N" or "S").</param>
        public Latitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            bool isPositive = inDirection.Equals("N", StringComparison.OrdinalIgnoreCase);
            Decimal = DMSToDecimal(inDegrees, inMinutes, inSeconds, isPositive);
        }

        #endregion
    }
}
