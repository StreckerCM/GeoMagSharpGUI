/****************************************************************************
 * File:            MagneticValue.cs
 * Description:     Single magnetic field value with annual change rate
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

namespace GeoMagSharp
{
    /// <summary>
    /// Represents a single magnetic field value with its annual rate of change
    /// </summary>
    public class MagneticValue
    {
        #region Constructors

        public MagneticValue()
        {
            Value = 0.0;
            ChangePerYear = 0.0;
        }

        public MagneticValue(MagneticValue other)
        {
            Value = other.Value;
            ChangePerYear = other.ChangePerYear;
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// The magnetic field value
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// The annual rate of change (secular variation)
        /// </summary>
        public double ChangePerYear { get; set; }

        #endregion
    }
}
