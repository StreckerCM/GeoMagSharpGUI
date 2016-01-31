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
    public static class Distance
    {
        public enum Unit
        {
            unknown = 0,
            meter = 1,
            kilometer = 2,
            foot = 3,
            mile = 4
        }

        public static string ToString(Unit inUnit)
        {
            switch (inUnit)
            {
                case Distance.Unit.meter:
                    return @"m";

                case Distance.Unit.kilometer:
                    return @"mi";

                case Distance.Unit.foot:
                    return @"ft";

                case Distance.Unit.mile:
                    return @"mi";

            }

            return string.Empty;
        }

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

        public static class Angle
        {
            public enum Unit
            {
                unknown = 0,
                Degree = 1,
                Radian = 2
            }

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
