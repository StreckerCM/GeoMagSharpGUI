/****************************************************************************
 * File:            MagneticCalculations.cs
 * Description:     Complete magnetic field calculation results
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;

namespace GeoMagSharp
{
    /// <summary>
    /// Contains the complete results of a magnetic field calculation
    /// </summary>
    public class MagneticCalculations
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance with the current date and zero-valued components.
        /// </summary>
        public MagneticCalculations()
        {
            Date = DateTime.Now;
            Declination = new MagneticValue();
            Inclination = new MagneticValue();
            HorizontalIntensity = new MagneticValue();
            NorthComp = new MagneticValue();
            EastComp = new MagneticValue();
            VerticalComp = new MagneticValue();
            TotalField = new MagneticValue();
        }

        /// <summary>
        /// Initializes a new instance by copying values from another <see cref="MagneticCalculations"/>.
        /// </summary>
        /// <param name="other">The source calculation results to copy.</param>
        public MagneticCalculations(MagneticCalculations other)
        {
            Date = other.Date;
            Declination = new MagneticValue(other.Declination);
            Inclination = new MagneticValue(other.Inclination);
            HorizontalIntensity = new MagneticValue(other.HorizontalIntensity);
            NorthComp = new MagneticValue(other.NorthComp);
            EastComp = new MagneticValue(other.EastComp);
            VerticalComp = new MagneticValue(other.VerticalComp);
            TotalField = new MagneticValue(other.TotalField);
        }

        /// <summary>
        /// Initializes a new instance from field and secular variation vectors.
        /// </summary>
        /// <param name="inDate">The date of the calculation.</param>
        /// <param name="fieldCalculations">The main magnetic field vector.</param>
        /// <param name="SecVarCalculations">The secular variation vector, or null if not computed.</param>
        public MagneticCalculations(DateTime inDate, GeoMagVector fieldCalculations, GeoMagVector SecVarCalculations = null)
        {
            Date = inDate;

            Declination = new MagneticValue
            {
                Value = fieldCalculations.d,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.d
            };

            Inclination = new MagneticValue
            {
                Value = fieldCalculations.s,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.s
            };

            HorizontalIntensity = new MagneticValue
            {
                Value = fieldCalculations.h,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.h
            };

            NorthComp = new MagneticValue
            {
                Value = fieldCalculations.x,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.x
            };

            EastComp = new MagneticValue
            {
                Value = fieldCalculations.y,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.y
            };

            VerticalComp = new MagneticValue
            {
                Value = fieldCalculations.z,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.z
            };

            TotalField = new MagneticValue
            {
                Value = fieldCalculations.f,
                ChangePerYear = SecVarCalculations == null ? 0 : SecVarCalculations.f
            };
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// Date of the calculation
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Magnetic declination (angle between magnetic north and true north)
        /// </summary>
        public MagneticValue Declination { get; set; }

        /// <summary>
        /// Magnetic inclination (dip angle)
        /// </summary>
        public MagneticValue Inclination { get; set; }

        /// <summary>
        /// Horizontal component of the magnetic field
        /// </summary>
        public MagneticValue HorizontalIntensity { get; set; }

        /// <summary>
        /// North component of the magnetic field (X)
        /// </summary>
        public MagneticValue NorthComp { get; set; }

        /// <summary>
        /// East component of the magnetic field (Y)
        /// </summary>
        public MagneticValue EastComp { get; set; }

        /// <summary>
        /// Vertical component of the magnetic field (Z)
        /// </summary>
        public MagneticValue VerticalComp { get; set; }

        /// <summary>
        /// Total field intensity
        /// </summary>
        public MagneticValue TotalField { get; set; }

        #endregion
    }
}
