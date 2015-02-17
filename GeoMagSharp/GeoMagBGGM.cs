using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public static class GeoMagBGGM
    {
        public static MagneticCalculations MagneticCalculations(ModelSetBGGM magModels, Options CalculationOptions) 
        {

            if (!magModels.IsDateInRange(CalculationOptions.StartDate)) 
                throw new GeoMagExceptionOutOfRange(string.Format("The Date {0} is not in the file range of {1} to {2}",
                    CalculationOptions.StartDate.ToDecimal(), magModels.MinDate, magModels.MaxDate));

            if (!magModels.IsDateInRange(CalculationOptions.EndDate))
                throw new GeoMagExceptionOutOfRange(string.Format("The Date {0} is not in the file range of {1} to {2}",
                    CalculationOptions.EndDate.ToDecimal(), magModels.MinDate, magModels.MaxDate));

            var internalSH = new coefficientsBGGM();

            var externalSH = new coefficientsBGGM();

            bool wasLoadSuccessful = false;
            
            try
            {
                magModels.GetIntExt(CalculationOptions.StartDate.ToDecimal(), out internalSH, out externalSH);

                wasLoadSuccessful = true;
            }
            catch(Exception ex)
            {
                wasLoadSuccessful = false;
            }

            if(!wasLoadSuccessful) return null;

            return SpotCalculation(CalculationOptions, magModels, internalSH, externalSH);

        }

        private static MagneticCalculations SpotCalculation(Options CalculationOptions, ModelSetBGGM magModels, coefficientsBGGM internalSH, coefficientsBGGM externalSH)
        {
            /* call procedure GETFIELD - values returned in geomagfield*/
            var fieldCalculations = GetField(CalculationOptions, internalSH, externalSH, Constants.EarthsRadiusInKm);

            //if (status != BGGM_SUCCESS) return status;

            if (CalculationOptions.SecularVariation)
                {
                    //calculate_dates_for_variation(options->date, mindate, maxdate,
                    //                               &date1, &date2);
                    ///* get coefficients and field for date1 */
                    //status = getintext(date1, coeff, num_models, &SVintSH, &SVextSH);
                    //if (status != BGGM_SUCCESS) return status;
                    //status = run_getfield(&SVintSH, &SVextSH, coeff->er_rad, options, &geomagfield1);
                    //freeintext(&SVintSH, &SVextSH);
                    //if (status != BGGM_SUCCESS) return status;

                    ///* get coefficients and field for date2 */
                    //status = getintext(date2, coeff, num_models, &SVintSH, &SVextSH);
                    //if (status != BGGM_SUCCESS) return status;
                    //status = run_getfield(&SVintSH, &SVextSH, coeff->er_rad, options, &geomagfield2);
                    //freeintext(&SVintSH, &SVextSH);
                    //if (status != BGGM_SUCCESS) return status;

                    ///* calculate changes - difference between 
                    //fields 1 & 2 returned in fielddiffs */
                    //calculate_changes(&geomagfield1, &geomagfield2, fielddiffs);

                } /* end of secular variation calc. for SPOT */

            return new MagneticCalculations(CalculationOptions.EndDate, fieldCalculations);

        }

        private static vectorBGGM GetField(Options CalculationOptions, coefficientsBGGM internalSH, coefficientsBGGM externalSH, double er_rad)
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
                    //j = aghIdx - isize;
                    agh[aghIdx] = externalSH.coeffs[aghIdx - isize];
                    /*    printf("COEFFS: i: %d j:%d coeff: %lf %lf\n",
                    i,j,externalSH->coeffs[j], agh[i]); */
                }
            } /* end of setting agh */


            /* convert depth in m to altitude in km and from geodetic to geocentric */
            double altitude = -(CalculationOptions.Depth * 0.001);

            double gcCoLat = 0;
            double lon = 0;
            double earthRadius = 0;
            double cd = 0;
            double sd = 0;

            GeodeticToGeocentric(CalculationOptions.CoLatitude, altitude, out gcCoLat, out earthRadius, out cd, out sd);
            /* printf("GDTOGC: %lf %lf %lf %lf\n", clt,r,cd,sd); */
            double one = gcCoLat;

            double slat = Math.Cos(one * Math.PI / 180);

            gcCoLat = Math.Sin(one * Math.PI / 180);
            
            one = lon;
            cl[0] = (Math.Cos(one * Math.PI / 180));
            sl[0] = (Math.Sin(one * Math.PI / 180));

            var geomagfield = new vectorBGGM();
            //geomagfield->x = 0.0;
            //geomagfield->y = 0.0;
            //geomagfield->z = 0.0;
    
            
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
            p[2] = gcCoLat;
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
                    q[k] = three * (slat * q[i] - gcCoLat * p[i]) - two * q[j];
                }
                else if (k != 2)
                {
                    /** block 2 **/
                    one = Math.Sqrt(1.0 - 0.5 / fm);
                    j = k - n - 2;
                    p[k] = one * gcCoLat * p[j];
                    q[k] = one * (gcCoLat * q[j] + slat * p[j]);
                    sl[m] = sl[m - 1] * cl[0] + cl[m - 1] * sl[0];
                    cl[m] = cl[m - 1] * cl[0] - sl[m - 1] * sl[0];
                }

                /*    printf("k=%d\n",k);  */
                /*    print_array(p,105); */
                /*    print_array(q,105); */
                /*     print_array(sl,30);  */
                /*     print_array(cl,30);  */

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

                    if (gcCoLat != 0)
                    {
                        ymn[li] = fm * p[k] / gcCoLat * rri * sl[m];
                        ymn[le] = fm * p[k] / gcCoLat * rre * sl[m];
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

                    if (gcCoLat != 0)
                    {
                        ymn[li] = -fm * p[k] / gcCoLat * rri * cl[m];
                        ymn[le] = -fm * p[k] / gcCoLat * rre * cl[m];
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

                /*    print_array(xmn,2*isize); */
                /*    print_array(ymn,2*isize); */
                /*    print_array(zmn,2*isize);  */


                /*    print_array(q,105); */
                /*     print_array(sl,30);  */
                /*     print_array(cl,30);  */

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
            geomagfield.d = Math.Atan2(geomagfield.y, geomagfield.x) * 180 / Math.PI;
            geomagfield.h = Math.Sqrt(geomagfield.x * geomagfield.x + geomagfield.y * geomagfield.y);
            geomagfield.s = Math.Atan2(geomagfield.z, geomagfield.h) * 180 / Math.PI;
            geomagfield.f = Math.Sqrt(geomagfield.x * geomagfield.x + geomagfield.y * geomagfield.y + geomagfield.z * geomagfield.z);

            return geomagfield;
        }

        /*****************************************************************************
         * gdtogc
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

            ct = Math.Cos(one * Math.PI / 180);
            st = Math.Sin(one * Math.PI / 180);
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

            gcCoLat = Math.Atan2(st, ct) * 180 / Math.PI;

        }


    }
}
