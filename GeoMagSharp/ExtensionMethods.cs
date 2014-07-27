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
            Int32 year = date.Year;

            Int32 month = date.Month;

            Int32 day = date.Day;

            Int32[] days = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};

            Boolean isleapYear = (((year % 4) == 0) && (((year % 100) != 0) || ((year % 400) == 0)));

            Int32 leapYear = Convert.ToInt32(isleapYear);

            double dayInYear = (days[month - 1] + day + (month > 2 ? leapYear : 0));

            return ((double)year + (dayInYear / (365.0 + leapYear)));

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
