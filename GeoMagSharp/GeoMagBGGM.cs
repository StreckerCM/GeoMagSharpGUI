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

            return SpotCalculation(CalculationOptions);

        }

        private static MagneticCalculations SpotCalculation(Options CalculationOptions)
        {
            var outCalc = new MagneticCalculations();

            /* call procedure GETFIELD - values returned in geomagfield*/
            //status = run_getfield(internalSH, externalSH, coeff->er_rad, options, geomagfield);
            //if (status != BGGM_SUCCESS) return status;

            return outCalc;

        }

        private static void GetField(Options CalculationOptions, coefficientsBGGM internalSH, coefficientsBGGM externalSH)
        {
            /* allocate storage for arrays */
            Int32 kmx = (internalSH.MaxDegree + 1) * (internalSH.MaxDegree + 2) / 2;
            Int32 isize = internalSH.MaxDegree * (internalSH.MaxDegree + 2);
            Int32 esize = externalSH.MaxDegree * (externalSH.MaxDegree + 2);
            Int32 nagh = isize + esize; 


            /* set values of agh */
            var agh = new List<double>();
            for (Int32 aghIdx = 0; aghIdx < nagh; aghIdx++)
            {
                if (aghIdx < isize)
                    agh.Add(internalSH.coeffs[aghIdx]);
                else
                {
                    //j = aghIdx - isize;
                    agh[aghIdx] = externalSH.coeffs[aghIdx - isize];
                    /*    printf("COEFFS: i: %d j:%d coeff: %lf %lf\n",
                    i,j,externalSH->coeffs[j], agh[i]); */
                }
            } /* end of setting agh */

        }


    }
}
