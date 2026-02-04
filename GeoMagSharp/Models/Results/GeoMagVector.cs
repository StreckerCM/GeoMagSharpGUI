/****************************************************************************
 * File:            GeoMagVector.cs
 * Description:     Complete magnetic field vector with all components
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

namespace GeoMagSharp
{
    /// <summary>
    /// Magnetic Vector Object - contains all magnetic field components
    /// </summary>
    public class GeoMagVector
    {
        #region Constructors

        public GeoMagVector()
        {
            d = 0;
            s = 0;
            h = 0;
            x = 0;
            y = 0;
            z = 0;
            f = 0;
        }

        public GeoMagVector(GeoMagVector other)
        {
            d = other.d;
            s = other.s;
            h = other.h;
            x = other.x;
            y = other.y;
            z = other.z;
            f = other.f;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Subtracts one vector from another
        /// </summary>
        /// <param name="vector2">The vector which to subtract</param>
        /// <returns>A vector containing the result of the subtraction</returns>
        public GeoMagVector Subtract(GeoMagVector vector2)
        {
            return new GeoMagVector
            {
                d = d - vector2.d,
                s = s - vector2.s,
                h = h - vector2.h,
                x = x - vector2.x,
                y = y - vector2.y,
                z = z - vector2.z,
                f = f - vector2.f
            };
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// Declination (deg +ve east)
        /// </summary>
        public double d { get; set; }

        /// <summary>
        /// Inclination (deg +ve down)
        /// </summary>
        public double s { get; set; }

        /// <summary>
        /// Horizontal Intensity (nT)
        /// </summary>
        public double h { get; set; }

        /// <summary>
        /// North Intensity (nT)
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// East Intensity (nT)
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Vertical Intensity (nT)
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// Total Intensity (nT)
        /// </summary>
        public double f { get; set; }

        #endregion
    }
}
