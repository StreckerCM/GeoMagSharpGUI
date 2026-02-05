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

        /// <summary>
        /// Initializes a new instance with an empty coefficient list and zero max degree.
        /// </summary>
        public Coefficients()
        {
            coeffs = new List<double>();
            MaxDegree = 0;
        }

        /// <summary>
        /// Initializes a new instance by copying coefficients from another <see cref="Coefficients"/>.
        /// </summary>
        /// <param name="other">The source coefficients to copy.</param>
        public Coefficients(Coefficients other)
        {
            coeffs = new List<double>();
            if (other.coeffs.Any()) coeffs.AddRange(other.coeffs);

            MaxDegree = other.MaxDegree;
        }

        #endregion

        /// <summary>The list of spherical harmonic coefficient values.</summary>
        public List<double> coeffs { get; set; }

        /// <summary>The maximum spherical harmonic degree represented by these coefficients.</summary>
        public Int32 MaxDegree { get; set; }
    }
}
