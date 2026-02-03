/****************************************************************************
 * File:            MagneticModelSet.cs
 * Description:     Collection of magnetic models for a specific magnetic field model
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoMagSharp
{
    /// <summary>
    /// Magnetic Model Set Object - contains multiple models (main, secular variation, external)
    /// for a specific magnetic field model like WMM, IGRF, etc.
    /// </summary>
    public class MagneticModelSet
    {
        #region Constructors

        public MagneticModelSet()
        {
            Models = new List<MagneticModel>();
        }

        public MagneticModelSet(MagneticModelSet other)
        {
            ID = other.ID;
            Type = other.Type;
            MinDate = other.MinDate;
            MaxDate = other.MaxDate;
            EarthRadius = other.EarthRadius;

            FileNames = new List<string>();
            if (other.FileNames.Any()) FileNames.AddRange(other.FileNames);

            Models = new List<MagneticModel>();
            if (other.Models.Any()) Models.AddRange(other.Models);
        }

        #endregion

        #region Public Methods

        public void AddModel(MagneticModel newModel)
        {
            if (newModel == null) return;

            if (Models == null) Models = new List<MagneticModel>();

            Models.Add(newModel);

            // Update data range to include added model
            MinDate = newModel.Year;
            MaxDate = newModel.Year;
        }

        public void AddCoefficients(Int32 modelIdx, double coeff)
        {
            if (modelIdx.Equals(-1)) return;

            if (Models == null) return;

            Models[modelIdx].SharmCoeff.Add(coeff);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Validate that the given date is in range for the loaded models
        /// </summary>
        /// <param name="date">The date of the calculation</param>
        /// <returns>Boolean representing if the date is valid for the loaded model</returns>
        public bool IsDateInRange(DateTime date)
        {
            return !((date.ToDecimal() < MinDate) || (date.ToDecimal() > MaxDate));
        }

        /// <summary>
        /// Given models of different types for different epochs, determine internal and external coefficients for given date.
        /// </summary>
        /// <param name="date">decimal year</param>
        /// <param name="internalSH">Output: internal coefficients</param>
        /// <param name="externalSH">Output: external coefficients</param>
        public void GetIntExt(double date, out Coefficients internalSH, out Coefficients externalSH)
        {
            internalSH = new Coefficients();
            externalSH = new Coefficients();

            Int32 nModels = NumberOfModels - 1;

            /* Find M model with epoch = date or just before */
            Int32 Mmodel1 = -1;
            for (Int32 mIdx = nModels; mIdx >= 0 && Mmodel1 == -1; mIdx--)
            {
                if (Models[mIdx].Year <= date && Models[mIdx].Type.Equals("M", StringComparison.OrdinalIgnoreCase)) Mmodel1 = mIdx;
            }

            /* Find E type model with epoch equal to or just before date */
            Int32 Emodel1 = -1;
            for (Int32 eIdx = nModels; eIdx >= 0 && Emodel1 == -1; eIdx--)
            {
                if (Models[eIdx].Year <= date && Models[eIdx].Type.Equals("E", StringComparison.OrdinalIgnoreCase)) Emodel1 = eIdx;
            }

            /* Find M model with epoch = date or just after */
            Int32 Mmodel2 = -1;
            for (Int32 mIdx = Mmodel1; mIdx <= nModels && Mmodel2 == -1; mIdx++)
            {
                if (Models[mIdx].Year > date && Models[mIdx].Type.Equals("M", StringComparison.OrdinalIgnoreCase)) Mmodel2 = mIdx;
            }

            /* Find E type model with epoch just after date */
            Int32 Emodel2 = -1;
            if (Emodel1 != -1)
            {
                for (Int32 eIdx = Emodel1; eIdx <= nModels && Emodel2 == -1; eIdx++)
                {
                    if (Models[eIdx].Year > date && Models[eIdx].Type.Equals("E", StringComparison.OrdinalIgnoreCase)) Emodel2 = eIdx;
                }
            }

            /* Compute internal coefficients for date */

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

                for (Int32 cIdx = 0; cIdx < numCoeff; cIdx++)
                {
                    internalSH.coeffs.Add(Models[Mmodel1].SharmCoeff[cIdx]);
                }

                for (Int32 sIdx = Mmodel1; sIdx <= nModels; sIdx++)
                {
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
            }

            /* Compute external coefficients for date */
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

        #endregion

        private Guid _ID = Guid.NewGuid();
        private string _Name = string.Empty;
        private List<string> _FileNames = new List<string>();
        private knownModels _Type = knownModels.NONE;
        private double? _MinDate = null;
        private double? _MaxDate = null;
        private double _EarthRadius = Constants.EarthsRadiusInKm;

        [JsonProperty]
        private List<MagneticModel> Models { get; set; }

        #region getters & setters

        public Guid ID
        {
            get
            {
                if (Models == null) return Guid.Empty;

                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public string Name
        {
            get
            {
                if (Models == null) return string.Empty;

                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public List<string> FileNames
        {
            get
            {
                if (Models == null) return null;

                return _FileNames;
            }
            set
            {
                _FileNames = value;
            }
        }

        public knownModels Type
        {
            get
            {
                if (Models == null) return knownModels.NONE;

                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public double MinDate
        {
            get
            {
                if (Models == null) return double.NaN;

                return Convert.ToDouble(_MinDate);
            }
            set
            {
                if (!value.IsValidYear()) return;

                if (_MinDate == null || value <= _MinDate) _MinDate = value;
            }
        }

        public double MaxDate
        {
            get
            {
                if (Models == null) return double.NaN;

                return Convert.ToDouble(_MaxDate);
            }
            set
            {
                if (!value.IsValidYear()) return;

                if (_MaxDate == null || _MaxDate <= value) _MaxDate = value;
            }
        }

        public double EarthRadius
        {
            get
            {
                if (Models == null) return double.NaN;

                return _EarthRadius;
            }
            set
            {
                _EarthRadius = value;
            }
        }

        [JsonIgnore]
        public List<MagneticModel> GetModels
        {
            get
            {
                return new List<MagneticModel>(Models);
            }
        }

        [JsonIgnore]
        public Int32 NumberOfModels
        {
            get
            {
                if (Models == null) return -1;

                return Models.Count;
            }
        }

        #endregion
    }
}
