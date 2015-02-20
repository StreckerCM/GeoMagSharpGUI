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

    public enum Algorithm
    {
        BGS = 1,
        NOAA = 2
    }

    //public enum DistanceUnit
    //{
    //    meter = 1,
    //    kilometer = 2,
    //    foot = 3,
    //    mile = 4
    //}

    public static class GeoMagDistance
    {
        public enum Unit
        {
            unknown = 0,
            meter = 1,
            kilometer = 2,
            foot = 3,
            mile = 4
        }


        public static string ToString(Unit inUnit)
        {
            switch (inUnit)
            {
                case GeoMagDistance.Unit.meter:
                    return @"m";

                case GeoMagDistance.Unit.kilometer:
                    return @"mi";

                case GeoMagDistance.Unit.foot:
                    return @"ft";

                case GeoMagDistance.Unit.mile:
                    return @"mi";
                    
            }

            return string.Empty;
        }

        public static Unit FromString(string unitString)
        {
            switch (unitString.ToLower())
            {
                case "meter":
                case "metre":
                case "m":
                    return GeoMagDistance.Unit.meter;

                case "kilometre":
                case "kilometer":
                case "km":
                    return GeoMagDistance.Unit.kilometer;

                case "foot":
                case "ft":
                    return GeoMagDistance.Unit.foot;

                case "mile":
                case "mi":
                    return GeoMagDistance.Unit.mile;


            }

            return GeoMagDistance.Unit.unknown;
        }

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

    public class GeoMagOptions
    {
        public GeoMagOptions()
        {
            Latitude = 0;
            Longitude = 0;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            StepInterval = 0;
            SecularVariation = true;
            CalculationMethod = Algorithm.BGS;

            ElevationValue = 0;
            ElevationUnit = GeoMagDistance.Unit.meter;
            ElevationIsAltitude = true;
        }

        public GeoMagOptions(GeoMagOptions other)
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

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StepInterval { get; set; }
        public bool SecularVariation { get; set; }
        public Algorithm CalculationMethod { get; set; }

        public void SetElevation(double value, GeoMagDistance.Unit unit, bool isAltitude = true)
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

                switch(ElevationUnit)
                {
                    case GeoMagDistance.Unit.kilometer:
                        depth = ElevationValue * 1000;
                        break;

                    case GeoMagDistance.Unit.foot:
                        depth = ElevationValue * 0.3048;
                        break;

                    case GeoMagDistance.Unit.mile:
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
                    case GeoMagDistance.Unit.meter:
                        altitude = ElevationValue * 0.001;
                        break;

                    case GeoMagDistance.Unit.foot:
                        altitude = ElevationValue * 0.0003048;
                        break;

                    case GeoMagDistance.Unit.mile:
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
                        GeoMagDistance.ToString(ElevationUnit)
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
        private GeoMagDistance.Unit ElevationUnit { get; set; }
        private bool ElevationIsAltitude { get; set; }

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

    /*Begin Definition of models*/
    public class GeoMagModelSet
    {
        public GeoMagModelSet()
        {
            FileName = string.Empty;
            MinDate = -1;
            MaxDate = -1;
            EarthRadius = Constants.EarthsRadiusInKm;

            Models = new List<GeoMagModel>();
        }

        public GeoMagModelSet(GeoMagModelSet other)
        {
            FileName = other.FileName;
            MinDate = other.MinDate;
            MaxDate = other.MaxDate;
            EarthRadius = other.EarthRadius;

            Models = new List<GeoMagModel>();
            if (other.Models.Any()) Models.AddRange(other.Models);

        }

        public void AddModel(GeoMagModel newModel)
        {
            if (newModel == null) return;

            if (Models == null) Models = new List<GeoMagModel>();

            Models.Add(newModel);

            //Update data range to include added model
            if (MinDate.Equals(-1) ||
                MinDate > newModel.Year) MinDate = newModel.Year;

            if (MaxDate.Equals(-1) ||
                MaxDate < newModel.Year) MaxDate = newModel.Year;

        }

        public void AddCoefficients(Int32 modelIdx, double coeff)
        {
            if (modelIdx.Equals(-1)) return;

            if (Models == null) return;

            Models[modelIdx].SharmCoeff.Add(coeff);

        }

        public bool IsDateInRange(DateTime date)
        {
            return !((date.ToDecimal() < MinDate) || (date.ToDecimal() > MaxDate));
        }

        /*****************************************************************************
         * GetIntExt
         *
         * Description: given models of different types for different epochs,
         *              determine internal and external coefficients for given date.
         *
         * Input parameters: date - decimal year
         *                   coeff, nmodels - model details from get_coefficients()
         * Output parameters: internalSH, externalSH - internal and external SH coeffs
         *
         * Returns SUCCESS for coefficients calculated OK, 
         *         NOT_ENOUGH_MEM is there was an allocation error 
         *
         * Comments: Once you have used (and no longer need) the calculated 
         *           coefficients, call freeintext() to release the memory
         *           allocated to them
         *
         *****************************************************************************/
        public void GetIntExt(double date, out GeoMagCoefficients internalSH, out GeoMagCoefficients externalSH)
        {
            internalSH = new GeoMagCoefficients();

            externalSH = new GeoMagCoefficients();

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

            /*compute internal coefficients for date */

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
                    //coeff_set = &(coeff[i]);
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

            }/* end of if no second M model */

            /* compute external coefficients for date */
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

        public string FileName { get; set; }
        public double MinDate { get; set; }
        public double MaxDate { get; set; }
        public double EarthRadius { get; set; }
        private List<GeoMagModel> Models { get; set; }

        public List<GeoMagModel> GetModels
        {
            get
            {
                return new List<GeoMagModel>(Models);
            }
        }

        public Int32 NumberOfModels
        {
            get
            {
                if (Models == null) return -1;

                return Models.Count;
            }
        }
    }

    public class GeoMagModel
    {
        public GeoMagModel()
        {
            Type = string.Empty;
            Year = -1;
            //EarthRadius = Constants.EarthsRadiusInKm;

            SharmCoeff = new List<double>();
        }

        public GeoMagModel(GeoMagModel other)
        {
            Type = other.Type;
            Year = other.Year;
            //EarthRadius = other.EarthRadius;

            SharmCoeff = new List<double>();
            if (other.SharmCoeff.Any()) SharmCoeff.AddRange(SharmCoeff);
        }

        public string Type { get; set; }
        public double Year { get; set; }
        public List<double> SharmCoeff;
        //public double EarthRadius;

        public Int32 Num_Coeff
        {
            get
            {
                if (SharmCoeff == null) return -1;

                return SharmCoeff.Count();
            }
        }

        public Int32 Max_Degree
        {
            get
            {
                Int32 j = Num_Coeff + 1;  /* number of coefficients */

                double rmax = Math.Sqrt(j) - 1.0;

                if (Math.IEEERemainder(rmax, 1.0) != 0) return -1;
                //throw new GeoMagExceptionBadNumberOfCoefficients(string.Format("Error: Bad Number of Coefficients in file{0}Number of Coefficients: {1}{0}Max Degree: {2}", Environment.NewLine, Num_Coeff, rmax.ToString("F2")));

                return Convert.ToInt32(rmax);
            }
        }
    }

    public class GeoMagCoefficients
    {
        public GeoMagCoefficients()
        {
            coeffs = new List<double>();
            MaxDegree = 0;
        }

        public GeoMagCoefficients(GeoMagCoefficients other)
        {
            coeffs = new List<double>();
            if (other.coeffs.Any()) coeffs.AddRange(other.coeffs);

            MaxDegree = other.MaxDegree;
        }

        public List<double> coeffs { get; set; }
        public Int32 MaxDegree { get; set; }
    }

    public class GeoMagVector
    {
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

        public double d { get; set; } /* declination (deg +ve east) */
        public double s { get; set; } /* inclination (deg +ve down) */
        public double h { get; set; } /* horizontal intensity (nT) */
        public double x { get; set; } /* north intensity (NT) */
        public double y { get; set; } /* east intensity (nT) */
        public double z { get; set; } /* vertical intensity (nT) */
        public double f { get; set; } /* total intensity (nT) */
    }

}
