using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public static class ExtensionMethods
    {
        /****************************************************************************/
        /*                                                                          */
        /*                           Subroutine ToDecimal                            */
        /*                                                                          */
        /****************************************************************************/
        /*                                                                          */
        /*     Computes the decimal day of year from month, day, year.              */
        /*     Supplied by Daniel Bergstrom                                         */
        /*                                                                          */
        /* References:                                                              */
        /*                                                                          */
        /* 1. Nachum Dershowitz and Edward M. Reingold, Calendrical Calculations,   */
        /*    Cambridge University Press, 3rd edition, ISBN 978-0-521-88540-9.      */
        /*                                                                          */
        /* 2. Claus Tøndering, Frequently Asked Questions about Calendars,          */
        /*    Version 2.9, http://www.tondering.dk/claus/calendar.html              */
        /*                                                                          */
        /****************************************************************************/

        public static double ToDecimal(this DateTime date)
        {
            //Int32 year = date.Year;

            //Int32 month = date.Month;

            //Int32 day = date.Day;

            //Int32[] days = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};

            //Boolean isleapYear = (((year % 4) == 0) && (((year % 100) != 0) || ((year % 400) == 0)));

            //Int32 leapYear = Convert.ToInt32(isleapYear);

            //double dayInYear = (days[month - 1] + day + (month > 2 ? leapYear : 0));

            //return ((double)year + (dayInYear / (365.0 + leapYear)));

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

        public static double ToDegree(this double inRadians)
        {
            return inRadians * (180.0 / Math.PI);
        }

        public static double ToRadian(this double inDegree)
        {
            return inDegree * (Math.PI / 180);
        }

        public static double Truncate(this double number)
        {
            var numStr = number.ToString("F15");

            if (numStr.IndexOf('.') == -1)
            {
                return number;
            }

            return Convert.ToDouble(numStr.Substring(0, numStr.IndexOf('.')));
        }

    }
    
}
