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

        public Latitude()
        {
            Decimal = 0;
        }

        public Latitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        public Latitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        public Latitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            bool isPositive = inDirection.Equals("N", StringComparison.OrdinalIgnoreCase);
            Decimal = DMSToDecimal(inDegrees, inMinutes, inSeconds, isPositive);
        }

        #endregion
    }
}
