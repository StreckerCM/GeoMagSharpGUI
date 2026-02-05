/****************************************************************************
 * File:            Units.cs
 * Description:     Contains Data Types used for calculations
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 1.0
 *  ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    /// <summary>
    /// Provides distance and angle unit types with conversion between string and enum representations.
    /// </summary>
    public static class Distance
    {
        /// <summary>
        /// Distance measurement units.
        /// </summary>
        public enum Unit
        {
            /// <summary>Unknown or unspecified unit.</summary>
            unknown = 0,
            /// <summary>Meters.</summary>
            meter = 1,
            /// <summary>Kilometers.</summary>
            kilometer = 2,
            /// <summary>Feet.</summary>
            foot = 3,
            /// <summary>Miles.</summary>
            mile = 4
        }

        /// <summary>
        /// Converts a <see cref="Unit"/> enum value to its abbreviation string.
        /// </summary>
        /// <param name="inUnit">The distance unit to convert.</param>
        /// <returns>The unit abbreviation (e.g., "m", "ft", "mi"), or empty string for unknown.</returns>
        public static string ToString(Unit inUnit)
        {
            switch (inUnit)
            {
                case Distance.Unit.meter:
                    return @"m";

                case Distance.Unit.kilometer:
                    return @"km";

                case Distance.Unit.foot:
                    return @"ft";

                case Distance.Unit.mile:
                    return @"mi";

            }

            return string.Empty;
        }

        /// <summary>
        /// Parses a string to the corresponding <see cref="Unit"/> enum value.
        /// </summary>
        /// <param name="unitString">The unit string (e.g., "m", "km", "ft", "meter").</param>
        /// <returns>The matching <see cref="Unit"/>, or <see cref="Unit.unknown"/> if not recognized.</returns>
        public static Unit FromString(string unitString)
        {
            switch (unitString.ToLower())
            {
                case "meter":
                case "metre":
                case "m":
                    return Distance.Unit.meter;

                case "kilometre":
                case "kilometer":
                case "km":
                    return Distance.Unit.kilometer;

                case "foot":
                case "ft":
                    return Distance.Unit.foot;

                case "mile":
                case "mi":
                    return Distance.Unit.mile;


            }

            return Distance.Unit.unknown;
        }

        /// <summary>
        /// Provides angle unit types with conversion between string and enum representations.
        /// </summary>
        public static class Angle
        {
            /// <summary>
            /// Angle measurement units.
            /// </summary>
            public enum Unit
            {
                /// <summary>Unknown or unspecified unit.</summary>
                unknown = 0,
                /// <summary>Degrees.</summary>
                Degree = 1,
                /// <summary>Radians.</summary>
                Radian = 2
            }

            /// <summary>
            /// Converts an <see cref="Unit"/> enum value to its symbol string.
            /// </summary>
            /// <param name="inUnit">The angle unit to convert.</param>
            /// <returns>The unit symbol (e.g., "°", "g"), or empty string for unknown.</returns>
            public static string ToString(Unit inUnit)
            {
                switch (inUnit)
                {
                    case Angle.Unit.Degree:
                        return @"°";

                    case Angle.Unit.Radian:
                        return @"g";

                }

                return string.Empty;
            }

            /// <summary>
            /// Parses a string to the corresponding <see cref="Unit"/> enum value.
            /// </summary>
            /// <param name="unitString">The unit string (e.g., "degree", "deg", "°", "radian").</param>
            /// <returns>The matching <see cref="Unit"/>, or <see cref="Unit.unknown"/> if not recognized.</returns>
            public static Unit FromString(string unitString)
            {
                switch (unitString.ToLower())
                {
                    case "degree":
                    case "deg":
                    case "°":
                        return Angle.Unit.Degree;

                    case "radian":
                    case "rad":
                    case "g":
                        return Angle.Unit.Radian;

                }

                return Angle.Unit.unknown;

            }
        }

    }

}
