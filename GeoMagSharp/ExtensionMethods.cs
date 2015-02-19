/****************************************************************************
 * File:            ExtensionMethods.cs
 * Description:     Routines extend basic functionality to built in classes
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public static class ExtensionMethods
    {
        /*****************************************************************************
         * ToDecimal
         *
         * Description: converts dd mm year into decimal year, returned in decyear
         *
         * Input parameters: dd - day of the month (1 to 31)
         *                   mm - month of the year (1 to 12)
         *                   year - integer year
         * Output parameters: 
         * Returns: decyear - the equivalent decimal year
         *
         * Comments:
         *****************************************************************************/
        public static double ToDecimal(this DateTime date)
        {
            double decYear = -1;

            if (date.Equals(DateTime.MinValue)) return decYear;

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

        /*****************************************************************************
         * ToDateTime
         *
         * Description: converts decimal year into dd mm year
         *
         * Input parameters: dd - day of the month (1 to 31)
         *                   mm - month of the year (1 to 12)
         *                   year - integer year
         * Output parameters:
         * Returns:  DateTime value of the decimal year
         *
         * Comments:
         *****************************************************************************/
        public static DateTime ToDateTime(this double decDate)
        {
            double daysDbl = decDate - Math.Truncate(decDate);

            Int32 yearInt = Convert.ToInt32(decDate - daysDbl);

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

        /*****************************************************************************
         * ToDegree
         *
         * Description: converts radians to degrees
         *
         * Input parameters: inRadians - double for the radian value
         * 
         * Output parameters:
         * Returns:  decimal value in degrees
         *
         * Comments:
         *****************************************************************************/
        public static double ToDegree(this double inRadians)
        {
            return inRadians * (180.0 / Math.PI);
        }

        /*****************************************************************************
         * ToRadian
         *
         * Description: converts degrees to radians
         *
         * Input parameters: inDegree - double for the degree value
         * 
         * Output parameters:
         * Returns:  decimal value in radian
         *
         * Comments:
         *****************************************************************************/
        public static double ToRadian(this double inDegree)
        {
            return inDegree * (Math.PI / 180);
        }

        /*****************************************************************************
         * Truncate
         *
         * Description: truncates the double value
         *
         * Input parameters: number - double for the number to truncate
         * 
         * Output parameters:
         * Returns:  number the truncated value
         *
         * Comments:
         *****************************************************************************/
        public static double Truncate(this double number)
        {
            var numStr = number.ToString("F15");

            //Check to see if number has a decimal value
            if (numStr.IndexOf('.').Equals(-1)) return number;

            return Convert.ToDouble(numStr.Substring(0, numStr.IndexOf('.')));
        }

    }
    
}
