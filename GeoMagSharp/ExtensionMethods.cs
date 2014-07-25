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
        /*                           Subroutine ToJulian                            */
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

        public static double ToJulian(this DateTime date)
        {
            //int days[12] = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};

            //int leap_year = (((year % 4) == 0) &&
            //                 (((year % 100) != 0) || ((year % 400) == 0)));

            //double day_in_year = (days[month - 1] + day + (month > 2 ? leap_year : 0));

            //return ((double)year + (day_in_year / (365.0 + leap_year)));
            return 0;
        }

    }
    
}
