/****************************************************************************
 * File:            MagneticModel.cs
 * Description:     Single magnetic model with spherical harmonic coefficients
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoMagSharp
{
    /// <summary>
    /// Represents a single magnetic model with spherical harmonic coefficients for a specific epoch
    /// </summary>
    public class MagneticModel
    {
        #region Constructors

        public MagneticModel()
        {
            Type = string.Empty;
            Year = -1;

            SharmCoeff = new List<double>();
        }

        public MagneticModel(MagneticModel other)
        {
            Type = other.Type;
            Year = other.Year;

            SharmCoeff = new List<double>();
            if (other.SharmCoeff.Any()) SharmCoeff.AddRange(SharmCoeff);
        }

        #endregion

        /// <summary>
        /// Model type identifier (M=Main, S=Secular variation, E=External)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Epoch year for the model coefficients
        /// </summary>
        public double Year { get; set; }

        /// <summary>
        /// Spherical harmonic coefficients
        /// </summary>
        public List<double> SharmCoeff;

        /// <summary>
        /// Number of coefficients in the model
        /// </summary>
        public Int32 Num_Coeff
        {
            get
            {
                if (SharmCoeff == null) return -1;

                return SharmCoeff.Count();
            }
        }

        /// <summary>
        /// Maximum spherical harmonic degree
        /// </summary>
        public Int32 Max_Degree
        {
            get
            {
                Int32 j = Num_Coeff + 1;  /* number of coefficients */

                double rmax = Math.Sqrt(j) - 1.0;

                if (Math.IEEERemainder(rmax, 1.0) != 0) return -1;

                return Convert.ToInt32(rmax);
            }
        }
    }
}
