/****************************************************************************
 * File:            GeoMagBGGM.cs
 * Description:     routines to handle bggm coefficients file and calculate
 *                  field values
 * Akowlegements:   Ported from the C++ model code created by the British Geological Survey
 * Website:         http://www.geomag.bgs.ac.uk/data_service/directionaldrilling/bggm.html
 * Warnings:        This code can be used with the IGRF, WMM, or BGGM coeficent file.  The BGGM 
 *                  coeficient file is Commerically avalable from the British Geological Survey 
 *                  and is not distributed with this project.  Please contcact the BGS for more information.
 *                  
 * Current version: 2.21
 *  ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public static class Calculator
    {

        /// <summary>
        /// calculate main field and field change at a 'spot' in time and space
        /// </summary>
        /// <param name="CalculationOptions">details of the time and space to for which to calculate</param>
        /// <param name="dateOfCalc">The DateTime object for the calculation date</param>
        /// <param name="magModels">the models loaded by the Model Reader</param>
        /// <param name="internalSH">internal coeffiecients for a particular date</param>
        /// <param name="externalSH">external coeffiecients for a particular date</param>
        /// <param name="earthRadius">Optional: Radius of the earth used by the model</param>
        /// <returns>The result of the magnetic calculation</returns>
        public static MagneticCalculations SpotCalculation(CalculationOptions CalculationOptions, DateTime dateOfCalc, MagneticModelSet magModels, 
            Coefficients internalSH, Coefficients externalSH, double earthRadius = Constants.EarthsRadiusInKm)
        {
            /* call procedure GETFIELD - values returned in geomagfield*/
            var fieldCalculations = GetField(CalculationOptions, internalSH, externalSH, earthRadius);

            GeoMagVector svCalculations = null;

            if (CalculationOptions.SecularVariation)
            {
                double date1 = -1;
                double date2 = -1;

                CalculateDatesForVariation(CalculationOptions.StartDate.ToDecimal(), magModels.MinDate, magModels.MaxDate, out date1, out date2);

                /* get coefficients and field for date1 */
                var SVintSH = new Coefficients();
                var SVextSH = new Coefficients();

                magModels.GetIntExt(date1, out SVintSH, out SVextSH);

                var geomagfield1 = GetField(CalculationOptions, SVintSH, SVextSH, earthRadius);

                /* get coefficients and field for date2 */

                magModels.GetIntExt(date2, out SVintSH, out SVextSH);

                var geomagfield2 = GetField(CalculationOptions, SVintSH, SVextSH, earthRadius);

                /* calculate changes - difference between 
                fields 1 & 2 returned in fielddiffs */

                svCalculations = geomagfield2.Subtract(geomagfield1);

            } /* end of secular variation calc. for SPOT */

            return new MagneticCalculations(dateOfCalc, fieldCalculations, svCalculations);

        }

        private static GeoMagVector GetField(CalculationOptions CalculationOptions, Coefficients internalSH, Coefficients externalSH, double er_rad)
        {
            /* allocate storage for arrays */
            Int32 kmx = (internalSH.MaxDegree + 1) * (internalSH.MaxDegree + 2) / 2;
            Int32 isize = internalSH.MaxDegree * (internalSH.MaxDegree + 2);
            Int32 esize = externalSH.MaxDegree * (externalSH.MaxDegree + 2);
            Int32 nagh = isize + esize;

            var agh = new double[nagh];
            var zmn = new double[2 * isize];
            var xmn = new double[2 * isize];
            var ymn = new double[2 * isize];

            var cl = new double[isize];
            var sl = new double[isize];

            var p = new double[kmx];
            var q = new double[kmx];

            /* set values of agh */
            
            for (Int32 aghIdx = 0; aghIdx < nagh; aghIdx++)
            {
                if (aghIdx < isize)
                {
                    agh[aghIdx] = internalSH.coeffs[aghIdx];
                }
                else
                {
                    agh[aghIdx] = externalSH.coeffs[aghIdx - isize];
                }
            } /* end of setting agh */

            double altitude = CalculationOptions.AltitudeInKm;

            double gcCoLat = 0;
            double earthRadius = 0;
            double cd = 0;
            double sd = 0;

            GeodeticToGeocentric(CalculationOptions.CoLatitude, altitude, out gcCoLat, out earthRadius, out cd, out sd);
            double one = gcCoLat;

            double slat = Math.Cos(one.ToRadian());
            double clat = Math.Sin(one.ToRadian());

            one = CalculationOptions.Longitude;
            cl[0] = (Math.Cos(one.ToRadian()));
            sl[0] = (Math.Sin(one.ToRadian()));

            var geomagfield = new GeoMagVector();

            /* printf("GETFIELD 2: %lf %lf %lf %lf\n", slat, clat, cl[0],sl[0]); */

            /****Computation of the spherical harmonics using
            Schmidt quasi-normal Legendre polynomials p and q  **/

            Int32 li = 0;
            Int32 le = isize;
            Int32 m = 0;
            Int32 n = -1;

            /* reference radius */
            double ratio = er_rad / earthRadius;
            double rri = ratio * ratio;
            double rre = ratio;
            p[0] = 1.0;
            p[2] = clat;
            q[0] = 0.0;
            q[2] = slat;

            double fn = 0;
            double gn = 0;
            double fm = 0;
            double two = 0;
            double three = 0;

            Int32 i = 0;
            Int32 j = 0;


            for (Int32 k = 1; k < kmx; k++, m++)
            {
                if (n < m)
                {
                    m = -1;
                    n++;
                    rri = rri * ratio;
                    rre = rre / ratio;
                    fn = n + 1;
                    gn = n;
                }
                fm = m + 1;
                if (n != m)
                {
                    /** block 3 **/
                    one = Math.Sqrt(fn * fn - fm * fm);
                    two = Math.Sqrt(gn * gn - fm * fm) / one;
                    three = (fn + gn) / one;
                    i = k - n - 1;
                    j = k - 2 * (n + 1) + 1;
                    p[k] = three * slat * p[i] - two * p[j];
                    q[k] = three * (slat * q[i] - clat * p[i]) - two * q[j];
                }
                else if (k != 2)
                {
                    /** block 2 **/
                    one = Math.Sqrt(1.0 - 0.5 / fm);
                    j = k - n - 2;
                    p[k] = one * clat * p[j];
                    q[k] = one * (clat * q[j] + slat * p[j]);
                    sl[m] = sl[m - 1] * cl[0] + cl[m - 1] * sl[0];
                    cl[m] = cl[m - 1] * cl[0] - sl[m - 1] * sl[0];
                }

                if (m < 0)
                {
                    /** block 4 **/
                    zmn[li] = -(fn + 1.0) * p[k] * rri;
                    zmn[le] = fn * p[k] * rre;
                    xmn[li] = q[k] * rri;
                    ymn[li] = 0.0;
                    xmn[le] = q[k] * rre;
                    ymn[le] = 0.0;
                    li++;
                    le++;
                }
                else
                {     /* m >= 0 */
                    /**block 5**/
                    zmn[li] = -(fn + 1.0) * p[k] * rri * cl[m];
                    xmn[li] = q[k] * rri * cl[m];
                    zmn[le] = fn * p[k] * rre * cl[m];
                    xmn[le] = q[k] * rre * cl[m];

                    if (clat != 0)
                    {
                        ymn[li] = fm * p[k] / clat * rri * sl[m];
                        ymn[le] = fm * p[k] / clat * rre * sl[m];
                    }
                    else
                    {
                        ymn[li] = q[k] * slat * rri * sl[m];
                        ymn[le] = q[k] * slat * rre * sl[m];
                    }

                    /**block 8 **/
                    li++;
                    le++;
                    zmn[li] = -(fn + 1.0) * p[k] * rri * sl[m];
                    xmn[li] = q[k] * rri * sl[m];
                    zmn[le] = fn * p[k] * rre * sl[m];
                    xmn[le] = q[k] * rre * sl[m];

                    if (clat != 0)
                    {
                        ymn[li] = -fm * p[k] / clat * rri * cl[m];
                        ymn[le] = -fm * p[k] / clat * rre * cl[m];
                    }
                    else
                    {
                        ymn[li] = -q[k] * slat * rri * cl[m];
                        ymn[le] = -q[k] * slat * rre * cl[m];
                    }

                    /** block 10 **/
                    li++;
                    le++;

                } /* end of if statement for m<=0 */


            } /* end of for loop */

            for (i = 0; i < 2 * isize; i++)
            {
                if (i >= nagh)
                {
                    xmn[i] = 0.0;
                    ymn[i] = 0.0;
                    zmn[i] = 0.0;
                }
                else
                {
                    geomagfield.x += xmn[i] * agh[i];
                    geomagfield.y += ymn[i] * agh[i];
                    geomagfield.z += zmn[i] * agh[i];
                }
            } /* end of for loop assigning geomagfield */

            /* convert x and z into geodetic coordinate system */
            one = geomagfield.x;
            geomagfield.x = (geomagfield.x) * cd + (geomagfield.z) * sd;
            geomagfield.z = (geomagfield.z) * cd - one * sd;

            /* compute remaining elements */
            geomagfield.d = Math.Atan2(geomagfield.y, geomagfield.x).ToDegree();
            geomagfield.h = Math.Sqrt(geomagfield.x * geomagfield.x + geomagfield.y * geomagfield.y);
            geomagfield.s = Math.Atan2(geomagfield.z, geomagfield.h).ToDegree();
            geomagfield.f = Math.Sqrt(geomagfield.x * geomagfield.x + geomagfield.y * geomagfield.y + geomagfield.z * geomagfield.z);

            return geomagfield;
        }

        /*****************************************************************************
         * GeodeticToGeocentric
         *
         * Description: conversion of position from geodetic, i.e. spheroidal, 
         *              to geocentric, i.e. spherical, coordinates using WGS84
         *              spheroid
         *
         * Input parameters: cltgd - geodetic colatitude (deg)
         *                   alt - altitude above sea-level (km)
         * Output parameters: cltgc - geocentic colatitude (deg)
         *                    r - radius from centre of earth (km)
         *                    cd - cos (difference in angle between geodetic and 
         *                              geocentric latitude)
         *                    sd - sin (difference in angle between geodetic and 
         *                              geocentric latitude)
         * Returns: none
         *
         * Comments:
         *****************************************************************************/
        static void GeodeticToGeocentric(double gdCoLat, double altitude, out double gcCoLat, out double radiusOfEarth, out double cd, out double sd)
        {
            double one, ct, st, a2, b2, two, three, rho;

            one = gdCoLat;

            ct = Math.Cos(one.ToRadian());
            st = Math.Sin(one.ToRadian());
            a2 = 40680631.6;
            b2 = 40408296.0;
            one = a2 * st * st;
            two = b2 * ct * ct;
            three = two + one;
            rho = Math.Sqrt(three);


            radiusOfEarth = Math.Sqrt(altitude * (altitude + 2.0 * rho) + (a2 * one + b2 * two) / three);
            cd = (altitude + rho) / (radiusOfEarth);
            sd = (a2 - b2) / rho * ct * st / (radiusOfEarth);

            one = ct;
            ct = ct * cd - st * sd;
            st = st * cd + one * sd;

            gcCoLat = Math.Atan2(st, ct).ToDegree();

        }

        /*****************************************************************************
         * CalculateDatesForVariation
         *
         * Description: calculate the dates used in secular variation calculation
         *
         * Input parameters: date - user entered date (decimal)
         *                   mindate - minimum allowable date
         *                   maxdate - maximum allowable date
         * Output parameters: date1 first date (decimal)
         *                    date2 second date (decimal)
         * Returns: none
         *
         * Comments:
         *****************************************************************************/
        static void CalculateDatesForVariation(double date, double mindate, double maxdate,
                                                   out double date1, out double date2)
        {
            date1 = date - 0.5;
            date2 = date + 0.5;

            if (date1 < mindate)
            {
                date1 = mindate;
                date2 = mindate + 1;
            }
            if (date2 > maxdate)
            {
                date1 = maxdate - 1;
                date2 = maxdate;
            }

        }


    }
}
