/****************************************************************************
 * File:            DataType.cs
 * Description:     Contains Data Types used for calculations
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public enum CoordinateSystem
    {
        Geodetic = 1,
        Geocentric = 2
    }

    public static class Constants
    {
        public const Int32 RecordLen = 80;

        public const Int32 MaxModules = 30;  /** Max number of models in a file **/

        public const Int32 MaxDeg = 13;
        public static Int32 MaxCoeff 
            {
                get
                {
                    return (MaxDeg * (MaxDeg + 2) + 1); /* index starts with 1!, (from old Fortran?) */
                }
            }

        public const double SN = 0.0001;

        public const double ThreeHundredFeetFromNorthPole = 89.999D;
        public const double ThreeHundredFeetFromSouthPole = -89.999D;

        public const double EarthsRadiusInKm = 6371.2D;

        public const double A2WGS84 = 40680631.59;            /* WGS84 */
        public const double B2WGS84 = 40408299.98;            /* WGS84 */

        public const double FeetToKilometer = 0.0003048;
        public const double FeetToMeter = 0.3048;

        public const double MeterToKilometer = 0.001;
        public const double MeterToFeet = 3.28084;

        public const double KilometerToMeter = 1000;
        public const double KilometerToFeet = 3280.84;

    }

    public class Options
    {
        public Options()
        {
            Latitude = 0;
            Longitude = 0;
            AltitudeInKm = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            StepInterval = 0;
            SecularVariation = true;
        }

        public Options(Options other)
        {
            Latitude = other.Latitude;
            Longitude = other.Longitude;
            AltitudeInKm = other.AltitudeInKm;
            StartDate = other.StartDate;
            EndDate = other.EndDate;
            StepInterval = other.StepInterval;
            SecularVariation = other.SecularVariation;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AltitudeInKm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StepInterval { get; set; }
        public bool SecularVariation { get; set; }

        public double DepthInM
        {
            get
            {
                return -(AltitudeInKm * 1000);
            }
        }

        public double CoLatitude 
        { 
            get
            {
                return 90 - Latitude;
            }
        }

    }

    public class MagneticCalculations
    {
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

        public MagneticCalculations(DateTime inDate, vector fieldCalculations, vector SecVarCalculations = null)
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

        public DateTime Date { get; set; }
        public MagneticValue Declination { get; set; }
        public MagneticValue Inclination { get; set; }
        public MagneticValue HorizontalIntensity { get; set; }
        public MagneticValue NorthComp { get; set; }
        public MagneticValue EastComp { get; set; }
        public MagneticValue VerticalComp { get; set; }
        public MagneticValue TotalField { get; set; }
    }

    public class MagneticValue
    {
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

        public double Value { get; set; }
        public double ChangePerYear { get; set; }
    }

    public class Latitude
    {
        public Latitude ()
        {
            Decimal = 0;
        }

        public Latitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        public Latitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        public Latitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            double coordDec = (inMinutes * 60) + inSeconds;

            coordDec = coordDec / 3600;  //Total number of seconds 

            coordDec = inDegrees + coordDec;

            if (inDirection.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                coordDec = Math.Abs(coordDec);
            }
            else if (inDirection.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                coordDec = -Math.Abs(coordDec);
            }

            Decimal = coordDec;
        }

        public double Degrees
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                return absDecimal.Truncate();
            }
        }

        public double Minutes
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                absDecimal -= absDecimal.Truncate();

                return (absDecimal * 60).Truncate();
            }
        }

        public double Seconds
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                absDecimal -= absDecimal.Truncate();

                absDecimal *= 60;
                
                absDecimal -= absDecimal.Truncate();

                return absDecimal * 60;
            }
        }

        public string Hemisphere 
        { 
            get
            {
                return Decimal >= 0 ? "N" : "S"; 
            } 
        }

        public string ToStringDMS
        {
            get
            {
                return string.Format("{0}° {1}′ {2}″ {3}", Degrees, Minutes, Seconds.ToString("F4"), Hemisphere);
            }
        }

        public double Decimal { get; set; }
    }

    public class Longitude
    {
        public Longitude ()
        {
            Decimal = 0;
        }

        public Longitude(double inDecimal)
        {
            Decimal = inDecimal;
        }

        public Longitude(Latitude other)
        {
            Decimal = other.Decimal;
        }

        public Longitude(double inDegrees, double inMinutes, double inSeconds, string inDirection)
        {
            double coordDec = (inMinutes * 60) + inSeconds;

            coordDec = coordDec / 3600;  //Total number of seconds 

            coordDec = inDegrees + coordDec;

            if (inDirection.Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                coordDec = Math.Abs(coordDec);
            }
            else if (inDirection.Equals("W", StringComparison.OrdinalIgnoreCase))
            {
                coordDec = -Math.Abs(coordDec);
            }

            Decimal = coordDec;
        }

        public double Degrees
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                return absDecimal.Truncate();
            }
        }

        public double Minutes
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                absDecimal -= absDecimal.Truncate();

                return (absDecimal * 60).Truncate();
            }
        }

        public double Seconds
        {
            get
            {
                double absDecimal = Math.Abs(Decimal);

                absDecimal -= absDecimal.Truncate();

                absDecimal *= 60;

                absDecimal -= absDecimal.Truncate();

                return absDecimal * 60;
            }
        }

        public string Hemisphere 
        { 
            get
            {
                return Decimal >= 0 ? "E" : "W"; 
            } 
        }

        public string ToStringDMS
        {
            get
            {
                return string.Format("{0}° {1}′ {2}″ {3}", Degrees, Minutes, Seconds.ToString("F4"), Hemisphere);
            }
        }

        public double Decimal { get; set; }
    }
}
