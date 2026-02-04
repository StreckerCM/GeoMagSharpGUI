/****************************************************************************
 * File:            Coefficients.cs
 * Description:     Spherical Harmonic Coefficient storage class
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoMagSharp
{
    /// <summary>
    /// Spherical Harmonic Coefficient Object
    /// </summary>
    public class Coefficients
    {
        #region Constructors

        public Coefficients()
        {
            coeffs = new List<double>();
            MaxDegree = 0;
        }

        public Coefficients(Coefficients other)
        {
            coeffs = new List<double>();
            if (other.coeffs.Any()) coeffs.AddRange(other.coeffs);

            MaxDegree = other.MaxDegree;
        }

        #endregion

        public List<double> coeffs { get; set; }
        public Int32 MaxDegree { get; set; }
    }
}
