using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public class ModelSetBGGM
    {
        public ModelSetBGGM()
        {
            FileName = string.Empty;
            MinDate = -1;
            MaxDate = -1;

            Models = new List<ModelBGGM>();
        }

        public ModelSetBGGM(ModelSetBGGM other)
        {
            FileName = other.FileName;
            MinDate = other.MinDate;
            MaxDate = other.MaxDate;

            Models = new List<ModelBGGM>();
            if (other.Models.Any()) Models.AddRange(other.Models);

        }

        public void AddModel(ModelBGGM newModel)
        {
            if (newModel == null) return;

            if (Models == null) Models = new List<ModelBGGM>();

            Models.Add(newModel);
        }

        public void AddCoefficients(Int32 modelIdx, double coeff)
        {
            if (modelIdx.Equals(-1)) return;

            if (Models == null) return;

            Models[modelIdx].SharmCoeff.Add(coeff);

        }

        public bool IsDateInRange(DateTime date)
        {
            return !((date.ToDecimal() < MinDate) || (date.ToDecimal() > MaxDate));
        }

        public void GetIntExt(double date, out coefficientsBGGM internalSH, out coefficientsBGGM externalSH)
        {
            internalSH = new coefficientsBGGM();

            externalSH = new coefficientsBGGM();

            Int32 nModels = NumberOfModels -1;

            //ModelBGGM coeff_set;

            //int Mmodel1, Mmodel2;  /* M type model just before and after date */
            //int Emodel1, Emodel2;  /* E type model just before and after date */
            //int Smodel1=0, Smodel2=0;  /* S type model */
            //double frs, fre;
            ////int i,j; 
            
            //List<double> coeff1, coeff2;
            //int num_coeff;
            //int ncoeff1, ncoeff2;
            //int maxdeg1, maxdeg2;
            //double f1,f2,z;   /* date increments used in linear interpolation */
            //double x,y;

            /* Find M model with epoch = date or just before */
            Int32 Mmodel1 = -1;
            for (Int32 mIdx = nModels; mIdx >= 0 && Mmodel1 == -1; mIdx--)
            {
                //coeff_set = new ModelBGGM(Models[mIdx]);

                if (Models[mIdx].Year <= date && Models[mIdx].Type.Equals("M", StringComparison.OrdinalIgnoreCase)) Mmodel1 = mIdx;
            }

            /* Find E type model with epoch equal to or just before date */
            Int32 Emodel1 = -1;
            for (Int32 eIdx = nModels; eIdx >= 0 && Emodel1 == -1; eIdx--)
            {
                //coeff_set = new ModelBGGM(Models[eIdx]);

                if (Models[eIdx].Year <= date && Models[eIdx].Type.Equals("E", StringComparison.OrdinalIgnoreCase)) Emodel1 = eIdx;
            }

            /* Find M model with epoch = date or just after */
            Int32 Mmodel2 = -1;
            for (Int32 mIdx = Mmodel1; mIdx <= nModels && Mmodel2 == -1; mIdx++)
            {
                //coeff_set = new ModelBGGM(Models[mIdx]);

                if (Models[mIdx].Year > date && Models[mIdx].Type.Equals("M", StringComparison.OrdinalIgnoreCase)) Mmodel2 = mIdx;
            }

            /* Find E type model with epoch just after date */
            Int32 Emodel2 = -1;
            if (Emodel1 != -1)
            {
                for (Int32 eIdx = Emodel1; eIdx <= nModels && Emodel2 == -1; eIdx++)
                {
                    //coeff_set = new ModelBGGM(Models[eIdx]);

                    if (Models[eIdx].Year > date && Models[eIdx].Type.Equals("E", StringComparison.OrdinalIgnoreCase)) Emodel2 = eIdx;
                }
            }

            /*compute internal coefficients for date */

            Int32 Smodel1 = 0;
            Int32 Smodel2 = 0;

            var coeff1 = new List<double>();
            var coeff2 = new List<double>();

            Int32 maxdeg1 = 0;
            Int32 maxdeg2 = 0;

            Int32 ncoeff1 = 0;
            Int32 ncoeff2 = 0;

            Int32 numCoeff = 0;

            /* if date just after is found ....*/
            /* then use linear interpolation   */
            if (Mmodel2 != -1)
            {
                coeff1 = new List<double>(Models[Mmodel1].SharmCoeff);
                coeff2 = new List<double>(Models[Mmodel2].SharmCoeff);

                maxdeg1 = Models[Mmodel1].Max_Degree;
                maxdeg2 = Models[Mmodel2].Max_Degree;

                ncoeff1 = maxdeg1 * (maxdeg1 + 2);
                ncoeff2 = maxdeg2 * (maxdeg2 + 2);

                if (ncoeff1 >= ncoeff2)
                    internalSH.MaxDegree = maxdeg1;
                else
                    internalSH.MaxDegree = maxdeg2;

                numCoeff = internalSH.MaxDegree * (internalSH.MaxDegree + 2);

                //internalSH.coeffs = (double*)malloc(num_coeff * sizeof(double));

                //if (!internalSH->coeffs) return BGGM_NOT_ENOUGH_MEM;

                /* get dates */
                double f1 = date - Models[Mmodel1].Year;
                double f2 = Models[Mmodel2].Year - date;
                double x = 0;
                double y = 0;
                double z = Models[Mmodel2].Year - Models[Mmodel1].Year;

                for (Int32 cIdx = 0; cIdx < numCoeff; cIdx++)
                {
                    if (cIdx < ncoeff1) x = f2 * coeff1[cIdx];
                    else x = 0.0;
                    if (cIdx < ncoeff2) y = f1 * coeff2[cIdx];
                    else y = 0.0;
                    internalSH.coeffs.Add((x + y) / z);
                }

            }
            else
            {
                /* if no second M model then
                   if date after last M type model look for S type model, add and 
                   subtract what is needed at start and end of time span
                   assumes an S model spans 1 year and epoch is for mid-date)
                   (at this stage we assume complete coverage in time provided by coefficients) */

                internalSH.MaxDegree = Models[Mmodel1].Max_Degree;
                numCoeff = internalSH.MaxDegree * (internalSH.MaxDegree + 2);

                //internalSH->coeffs = (double*)malloc(num_coeff * sizeof(double));

                //if (!internalSH->coeffs) return BGGM_NOT_ENOUGH_MEM;

                for (Int32 cIdx = 0; cIdx < numCoeff; cIdx++)
                {
                    internalSH.coeffs.Add(Models[Mmodel1].SharmCoeff[cIdx]);
                }

                for (Int32 sIdx = Mmodel1; sIdx <= nModels; sIdx++)
                {
                    //coeff_set = &(coeff[i]);
                    if (date >= ((Models[sIdx].Year) - 0.5) && Models[sIdx].Type.Equals("S", StringComparison.OrdinalIgnoreCase))
                    {
                        if (Smodel1.Equals(0)) Smodel1 = sIdx;
                        Smodel2 = sIdx;
                        for (Int32 jIdx = 0; jIdx < numCoeff; jIdx++)
                        {
                            if (jIdx < Models[sIdx].Num_Coeff)
                                internalSH.coeffs[jIdx] += Models[sIdx].SharmCoeff[jIdx];
                        }
                    }
                }


                /* subtract what is needed at start and end */
                double frs = Models[Mmodel1].Year - Models[Smodel1].Year + 0.5;
                double fre = Models[Smodel2].Year - date + 0.5;

                for (Int32 sIdx = 0; sIdx < numCoeff; sIdx++)
                {
                    if (sIdx < Models[Smodel1].Num_Coeff)
                        internalSH.coeffs[sIdx] -= (frs * Models[Smodel1].SharmCoeff[sIdx]);

                    if (sIdx < Models[Smodel2].Num_Coeff)
                        internalSH.coeffs[sIdx] -= (fre * Models[Smodel2].SharmCoeff[sIdx]);
                } 

            }/* end of if no second M model */

            /* compute external coefficients for date */
            /* no E type models */
            if (Emodel1.Equals(-1))
            {
                externalSH.MaxDegree = 0;
            }
            /* only a start date E model */
            else if (Emodel2.Equals(-1))
            {
                externalSH.MaxDegree = Models[Emodel1].Max_Degree;

                numCoeff = externalSH.MaxDegree * (externalSH.MaxDegree + 2);

                //externalSH->coeffs = (double*)malloc(num_coeff * sizeof(double));
                //if (!externalSH->coeffs) return BGGM_NOT_ENOUGH_MEM;

                for (Int32 cIdx = 0; cIdx < numCoeff; cIdx++)
                    externalSH.coeffs.Add(Models[Emodel1].SharmCoeff[cIdx]);
                
            }

            /* both E type models */
            else
            {
                coeff1 = new List<double>(Models[Emodel1].SharmCoeff);
                coeff2 = new List<double>(Models[Emodel2].SharmCoeff);

                maxdeg1 = Models[Emodel1].Max_Degree;
                maxdeg2 = Models[Emodel2].Max_Degree;

                ncoeff1 = maxdeg1 * (maxdeg1 + 2);
                ncoeff2 = maxdeg2 * (maxdeg2 + 2);

                if (ncoeff1 >= ncoeff2)
                    externalSH.MaxDegree = maxdeg1;
                else
                    externalSH.MaxDegree = maxdeg2;
                
                numCoeff = externalSH.MaxDegree * (externalSH.MaxDegree + 2);

                //externalSH->coeffs = (double*)malloc(num_coeff * sizeof(double));
                //if (!externalSH->coeffs) return BGGM_NOT_ENOUGH_MEM;
                /* get dates */
                double f1 = date - Models[Emodel1].Year;
                double f2 = Models[Emodel2].Year - date;
                double x = 0;
                double y = 0;
                double z = Models[Emodel2].Year - Models[Emodel1].Year;

                for (Int32 cIdx = 0; cIdx < numCoeff; cIdx++)
                {
                    if (cIdx < ncoeff1) x = f2 * coeff1[cIdx];
                    else x = 0.0;

                    if (cIdx < ncoeff2) y = f1 * coeff2[cIdx];
                    else y = 0.0;

                    externalSH.coeffs.Add((x + y) / z);

                }
            }   
        }

        public string FileName { get; set; }
        public double MinDate { get; set; }
        public double MaxDate { get; set; }
        private List<ModelBGGM> Models { get; set; }

        public List<ModelBGGM> GetModels 
        { 
            get
            {
                return new List<ModelBGGM>(Models);
            }
        }

        public Int32 NumberOfModels
        {
            get
            {
                if (Models == null) return -1;

                return Models.Count; 
            }
        }
    }

    public class ModelBGGM
    {
        public ModelBGGM()
        {
            Type = string.Empty;
            Year = -1;
            EarthRadius = Constants.EarthsRadiusInKm;

            SharmCoeff = new List<double>();
        }

        public ModelBGGM(ModelBGGM other)
        {
            Type = other.Type;
            Year = other.Year;
            EarthRadius = other.EarthRadius;

            SharmCoeff = new List<double>();
            if (other.SharmCoeff.Any()) SharmCoeff.AddRange(SharmCoeff);
        }

        public string Type { get; set; }
        public double Year { get; set; }
        public List<double> SharmCoeff;
        public double EarthRadius;

        public Int32 Num_Coeff
        {
            get
            {
                if (SharmCoeff == null) return -1;

                return SharmCoeff.Count();
            }
        }

        public Int32 Max_Degree
        {
            get
            {
                Int32 j = Num_Coeff + 1;  /* number of coefficients */

                double rmax = Math.Sqrt(j) - 1.0;

                if (Math.IEEERemainder(rmax, 1.0) != 0) throw new GeoMagExceptionBadNumberOfCoefficients(string.Format("Number of Coefficients: {0} | Max Degree: {1}", Num_Coeff, rmax));
                
                return Convert.ToInt32(rmax);
            }
        }
    }

    public class coefficientsBGGM
    {
        public coefficientsBGGM()
        {
            coeffs = new List<double>();
            MaxDegree = 0;
        }

        public coefficientsBGGM(coefficientsBGGM other)
        {
            coeffs = new List<double>();
            if (other.coeffs.Any()) coeffs.AddRange(other.coeffs);

            MaxDegree = other.MaxDegree;
        }

        public List<double> coeffs { get; set; }
        public Int32 MaxDegree { get; set; }
    }

    public class vectorBGGM 
    {
        public vectorBGGM()
        {
            d = 0;
            s = 0;
            h = 0;
            x = 0;
            y = 0;
            z = 0;
            f = 0;
        }

        public vectorBGGM(vectorBGGM other)
        {
            d = other.d;
            s = other.s;
            h = other.h;
            x = other.x;
            y = other.y;
            z = other.z;
            f = other.f;
        }

        public double d { get; set; } /* declination (deg +ve east) */
        public double s { get; set; } /* inclination (deg +ve down) */
        public double h { get; set; } /* horizontal intensity (nT) */
        public double x { get; set; } /* north intensity (NT) */
        public double y { get; set; } /* east intensity (nT) */
        public double z { get; set; } /* vertical intensity (nT) */
        public double f { get; set; } /* total intensity (nT) */
    };

}
