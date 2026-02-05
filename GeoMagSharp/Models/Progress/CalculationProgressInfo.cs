/****************************************************************************
 * File:            CalculationProgressInfo.cs
 * Description:     Progress reporting data for async operations
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

namespace GeoMagSharp
{
    /// <summary>
    /// Provides progress information for long-running async operations
    /// such as model loading and magnetic field calculations.
    /// </summary>
    public class CalculationProgressInfo
    {
        /// <summary>
        /// The current step number in the operation.
        /// </summary>
        public int CurrentStep { get; set; }

        /// <summary>
        /// The total number of steps in the operation.
        /// </summary>
        public int TotalSteps { get; set; }

        /// <summary>
        /// A human-readable status message describing the current operation.
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets the percentage complete (0-100) based on CurrentStep and TotalSteps.
        /// Returns 0 if TotalSteps is 0 or negative.
        /// </summary>
        public double PercentComplete
        {
            get
            {
                return TotalSteps > 0
                    ? (CurrentStep * 100.0 / TotalSteps)
                    : 0;
            }
        }
    }
}
