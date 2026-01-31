/****************************************************************************
 * File:            ExtensionMethods.cs
 * Description:     Routines extend basic functionality to built in classes
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;

namespace GeoMagSharp
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// converts DateTime object into decimal year, returned in decyear
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>decyear - the equivalent decimal year</returns>
        public static double ToDecimal(this DateTime date)
        {
            double decYear = -1;

            if (date.Equals(DateTime.MinValue) || !date.IsValidYear()) return decYear;

            Int32 im = -1;

            switch (date.Month)
            {
                case 1: im = 0; break;
                case 2: im = 31; break;
                case 3: im = 59; break;
                case 4: im = 90; break;
                case 5: im = 120; break;
                case 6: im = 151; break;
                case 7: im = 181; break;
                case 8: im = 212; break;
                case 9: im = 243; break;
                case 10: im = 273; break;
                case 11: im = 304; break;
                case 12: im = 334; break;
                default: return decYear;
            }

            double day = im + date.Day - 0.5;

            double yearDbl = Convert.ToDouble(date.Year);

            if (!DateTime.IsLeapYear(date.Year))
            {
                decYear = yearDbl + (day / 365D);
            }
            else
            {
                if (date.Month.Equals(1) || date.Month.Equals(2))
                {
                    decYear = yearDbl + (day / 366D);
                }
                else
                {
                    day++;
                    decYear = yearDbl + (day / 366D);
                }
            }

            return decYear;

        }

        /// <summary>
        /// converts decimal year into DateTime object
        /// </summary>
        /// <param name="decDate">The decimal year</param>
        /// <returns>The equivalent DateTime object year</returns>
        public static DateTime ToDateTime(this double decDate)
        {
            double daysDbl = decDate - Math.Truncate(decDate);

            Int32 yearInt = Convert.ToInt32(decDate - daysDbl);

            if (!decDate.IsValidYear()) return DateTime.MaxValue;

            double day = 0;
            
            if (!DateTime.IsLeapYear(yearInt))
            {
                day = daysDbl * 365D;
            }
            else
            {
                if (daysDbl < 0.165D)
                {
                    day = daysDbl * 366D;
                }
                else
                {
                    day = daysDbl * 366D;
                    day--;
                }
            }

            Int32 dayInt = 0;
            Int32 monthInt = -1;

            if((day + 0.5) < 31)
            {
                monthInt = 1;
                dayInt = Convert.ToInt32(day + 1);
            }
            else if((day - 0.5) < 59)
            {
                monthInt = 2;
                dayInt = Convert.ToInt32(31 - day + 1);
            }
            else if ((day - 0.5) < 90)
            {
                monthInt = 3;
                dayInt = Convert.ToInt32(59 - day + 1);
            }
            else if ((day - 0.5) < 120)
            {
                monthInt = 4;
                dayInt = Convert.ToInt32(90 - day + 1);
            }
            else if ((day - 0.5) < 151)
            {
                monthInt = 5;
                dayInt = Convert.ToInt32(120 - day + 1);
            }
            else if ((day - 0.5) < 181)
            {
                monthInt = 6;
                dayInt = Convert.ToInt32(151 - day + 1);
            }
            else if ((day - 0.5) < 212)
            {
                monthInt = 7;
                dayInt = Convert.ToInt32(181 - day + 1);
            }
            else if ((day - 0.5) < 243)
            {
                monthInt = 8;
                dayInt = Convert.ToInt32(212 - day + 1);
            }
            else if ((day - 0.5) < 273)
            {
                monthInt = 9;
                dayInt = Convert.ToInt32(243 - day + 1);
            }
            else if ((day - 0.5) < 304)
            {
                monthInt = 10;
                dayInt = Convert.ToInt32(273 - day + 1);
            }
            else if ((day - 0.5) < 334)
            {
                monthInt = 11;
                dayInt = Convert.ToInt32(304 - day + 1);
            }
            else
            {
                monthInt = 12;
                dayInt = Convert.ToInt32(334 - day + 1);
            }

            return new DateTime(yearInt, monthInt, dayInt);
        }

        public static bool IsValidYear(this DateTime date)
        {
            return (1900 <= date.Year && date.Year <= DateTime.MaxValue.Year);
        }

        public static bool IsValidYear(this double decDate)
        {
            return (1900D <= decDate && decDate <= DateTime.MaxValue.ToDecimal());
        }

        /// <summary>
        /// converts radians to degrees
        /// </summary>
        /// <param name="inRadians">the radian value as double</param>
        /// <returns>the equivalent degree value as double</returns>
        public static double ToDegree(this double inRadians)
        {
            return inRadians * (180.0 / Math.PI);
        }

        /// <summary>
        /// converts degrees to radians
        /// </summary>
        /// <param name="inDegree">the degree value as double</param>
        /// <returns>the equivalent radians value as double</returns>
        public static double ToRadian(this double inDegree)
        {
            return inDegree * (Math.PI / 180);
        }

        /// <summary>
        /// Truncates a double value
        /// </summary>
        /// <param name="number">The number to truncate as double</param>
        /// <returns>The truncated value as double</returns>
        public static double Truncate(this double number)
        {
            var numStr = number.ToString("F15");

            //Check to see if number has a decimal value
            if (numStr.IndexOf('.').Equals(-1)) return number;

            return Convert.ToDouble(numStr.Substring(0, numStr.IndexOf('.')));
        }

        /// <summary>
        /// Checks a string (typically a COF file header line) for known magnetic model identifiers.
        /// Supports both old format (model name at start) and new format (year at start, model name later).
        /// </summary>
        /// <param name="value">The string to check for model identifiers</param>
        /// <returns>The detected model type, or knownModels.NONE if not found</returns>
        public static knownModels CheckStringForModel(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return knownModels.NONE;

            // Check for each known model type
            foreach (knownModels model in Enum.GetValues(typeof(knownModels)))
            {
                if (model.Equals(knownModels.NONE))
                    continue;

                string modelName = model.ToString();
                Int32 idx = value.IndexOf(modelName, StringComparison.OrdinalIgnoreCase);

                if (idx == -1)
                    continue;

                // EMM can be found anywhere in the line
                if (model.Equals(knownModels.EMM))
                {
                    return model;
                }

                // For WMM, IGRF, DGRF: accept if found at position 0 (old format)
                // OR if found later in line after a digit (new format where year comes first)
                if (idx == 0)
                {
                    return model;
                }

                // New format detection: line starts with year (digit), model name appears later
                // Example: "    2020.0            WMM-2020        12/10/2019"
                string trimmed = value.TrimStart();
                if (trimmed.Length > 0 && char.IsDigit(trimmed[0]))
                {
                    // This is the new format - model name found somewhere in the line
                    return model;
                }
            }

            return knownModels.NONE;
        }
    
    }

}
