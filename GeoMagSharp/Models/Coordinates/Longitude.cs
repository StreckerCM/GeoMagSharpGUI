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

        public Longitude()
        {
            Decimal = 0;
        }

        public Longitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        public Longitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        public Longitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            bool isPositive = inDirection.Equals("E", StringComparison.OrdinalIgnoreCase);
            Decimal = DMSToDecimal(inDegrees, inMinutes, inSeconds, isPositive);
        }

        #endregion
    }
}
