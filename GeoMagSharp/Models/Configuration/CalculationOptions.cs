/****************************************************************************
 * File:            CalculationOptions.cs
 * Description:     Options for magnetic field calculations
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;
using System.Collections.Generic;

namespace GeoMagSharp
{
    /// <summary>
    /// Configuration options for magnetic field calculations
    /// </summary>
    public class CalculationOptions
    {
        #region Constructors

        public CalculationOptions()
        {
            Latitude = 0;
            Longitude = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            StepInterval = 0;
            SecularVariation = true;
            CalculationMethod = Algorithm.BGS;

            ElevationValue = 0;
            ElevationUnit = Distance.Unit.meter;
            ElevationIsAltitude = true;
        }

        public CalculationOptions(CalculationOptions other)
        {
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            StartDate = other.StartDate;
            EndDate = other.EndDate;
            StepInterval = other.StepInterval;
            SecularVariation = other.SecularVariation;
            CalculationMethod = other.CalculationMethod;

            ElevationValue = other.ElevationValue;
            ElevationUnit = other.ElevationUnit;
            ElevationIsAltitude = other.ElevationIsAltitude;
        }

        #endregion

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StepInterval { get; set; }
        public bool SecularVariation { get; set; }
        public Algorithm CalculationMethod { get; set; }

        #region Getters & Setters

        public void SetElevation(double value, Distance.Unit unit, bool isAltitude = true)
        {
            ElevationValue = value;
            ElevationUnit = unit;
            ElevationIsAltitude = isAltitude;
        }

        public double DepthInM
        {
            get
            {
                double depth = ElevationValue;

                switch (ElevationUnit)
                {
                    case Distance.Unit.kilometer:
                        depth = ElevationValue * 1000;
                        break;

                    case Distance.Unit.foot:
                        depth = ElevationValue * 0.3048;
                        break;

                    case Distance.Unit.mile:
                        depth = ElevationValue * 1609.34;
                        break;
                }

                return ElevationIsAltitude
                                ? -depth
                                : depth;
            }
        }

        public double AltitudeInKm
        {
            get
            {
                double altitude = ElevationValue;

                switch (ElevationUnit)
                {
                    case Distance.Unit.meter:
                        altitude = ElevationValue * 0.001;
                        break;

                    case Distance.Unit.foot:
                        altitude = ElevationValue * 0.0003048;
                        break;

                    case Distance.Unit.mile:
                        altitude = ElevationValue * 1.60934;
                        break;
                }

                return ElevationIsAltitude
                                ? altitude
                                : -altitude;
            }
        }

        public List<object> GetElevation
        {
            get
            {
                return new List<object>
                {
                    ElevationIsAltitude ? @"Altitude" : @"Depth",
                    ElevationValue,
                    Distance.ToString(ElevationUnit)
                };
            }
        }

        public double CoLatitude
        {
            get
            {
                return 90 - Latitude;
            }
        }

        private double ElevationValue { get; set; }
        private Distance.Unit ElevationUnit { get; set; }
        private bool ElevationIsAltitude { get; set; }

        #endregion
    }
}
