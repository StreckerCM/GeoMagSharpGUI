/****************************************************************************
 * File:            DataModel.cs
 * Description:     Contains Data Types used for calculations
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Newtonsoft.Json;

namespace GeoMagSharp
{
    #region Enumeration

    public enum CoordinateSystem
    {
        Geodetic = 1,
        Geocentric = 2
    }

    public enum Algorithm
    {
        BGS = 1,
        NOAA = 2,
        MAGVAR = 3
    }

    public enum knownModels
    {
        NONE = 0,
        DGRF = 1,
        EMM = 2,
        IGRF = 3,
        WMM = 4
    }

    #endregion

    #region Constants 
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
    #endregion

    /// <summary>
    /// 
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

                switch(ElevationUnit)
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

    public class MagneticCalculations
    {
        #region Constructors

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

        #endregion

        #region Getters & Setters

        public DateTime Date { get; set; }
        public MagneticValue Declination { get; set; }
        public MagneticValue Inclination { get; set; }
        public MagneticValue HorizontalIntensity { get; set; }
        public MagneticValue NorthComp { get; set; }
        public MagneticValue EastComp { get; set; }
        public MagneticValue VerticalComp { get; set; }
        public MagneticValue TotalField { get; set; }

        #endregion
    }

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

        public double Value { get; set; }
        public double ChangePerYear { get; set; }

        #endregion
    }

    public class Latitude
    {
        #region Constructors

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

        #endregion

        #region Getters & Setters

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

        #endregion
    }

    public class Longitude
    {
        #region Constructors

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

        #endregion

        #region Getters & Setters

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

        #endregion
    }

    /// <summary>
    /// Magnetic Model Collection Object
    /// </summary>
    public class MagneticModelCollection : IEnumerable<MagneticModelSet>
    {
        public List<MagneticModelSet> TList { get; private set; }

        public MagneticModelCollection()
        {
            TList = new List<MagneticModelSet>();
        }

        public void Add(MagneticModelSet item)
        {
            TList.Add(item);
        }
        public IEnumerator<MagneticModelSet> GetEnumerator()
        {
            return TList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Object Serializers

        public bool Save(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return false;

            bool wasSucessful;

            var inData = this;

            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };


                using (StreamWriter sw = new StreamWriter(filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, inData);
                }

                wasSucessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("MagneticModelCollection Error: {0}", ex.ToString());
                wasSucessful = false;

            }

            return wasSucessful;
        }

        public static MagneticModelCollection Load(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return new MagneticModelCollection();

            if (!System.IO.File.Exists(filename)) return new MagneticModelCollection();

            MagneticModelCollection outData = null;

            try
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                using (var sr = new System.IO.StreamReader(filename))
                using (var reader = new JsonTextReader(sr))
                {
                    outData = JsonConvert.DeserializeObject<MagneticModelCollection>(serializer.Deserialize(reader).ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MagneticModelCollection Error: {0}", ex.ToString());
                outData = null;
            }

            return outData;

        }

        #endregion

    }

    /// <summary>
    /// Magnetic Model Set Object
    /// </summary>
    public class MagneticModelSet
    {
        #region Constructors

        public MagneticModelSet()
        {

            Models = new List<MagneticModel>();
        }

        public MagneticModelSet(MagneticModelSet other)
        {
            FileName = other.FileName;
            Type = other.Type;
            MinDate = other.MinDate;
            MaxDate = other.MaxDate;
            EarthRadius = other.EarthRadius;

            Models = new List<MagneticModel>();
            if (other.Models.Any()) Models.AddRange(other.Models);

        }

        #endregion

        #region Public Methods

        public void AddModel(MagneticModel newModel)
        {
            if (newModel == null) return;

            if (Models == null) Models = new List<MagneticModel>();

            Models.Add(newModel);

            //Update data range to include added model
            MinDate = newModel.Year;

            MaxDate = newModel.Year;

            //if (MinDate.Equals(-1) ||
            //    MinDate > newModel.Year) MinDate = newModel.Year;

            //if (MaxDate.Equals(-1) ||
            //    MaxDate < newModel.Year) MaxDate = newModel.Year;

        }

        public void AddCoefficients(Int32 modelIdx, double coeff)
        {
            if (modelIdx.Equals(-1)) return;

            if (Models == null) return;

            Models[modelIdx].SharmCoeff.Add(coeff);

        }

        #endregion

        #region Properties

        /// <summary>
        /// Validate that the given date is in range for the loaded models
        /// </summary>
        /// <param name="date">The date of the calculation</param>
        /// <returns>Boolean representing if the date is valid for the loaded model</returns>
        public bool IsDateInRange(DateTime date)
        {
            return !((date.ToDecimal() < MinDate) || (date.ToDecimal() > MaxDate));
        }

        /// <summary>
        /// given models of different types for different epochs, determine internal and external coefficients for given date.
        /// </summary>
        /// <param name="date">decimal year</param>
        /// <param name="internalSH">Output: internal coefficents</param>
        /// <param name="externalSH">Output: external coefficents</param>
        /// <remarks>SUCCESS for coefficients calculated OK, NOT_ENOUGH_MEM is there was an allocation error </remarks>
        public void GetIntExt(double date, out Coefficients internalSH, out Coefficients externalSH)
        {
            internalSH = new Coefficients();

            externalSH = new Coefficients();

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

        #endregion

        private string _Name = string.Empty;
        private string _FileName = string.Empty;
        private knownModels _Type = knownModels.NONE;
        private double? _MinDate = null;
        private double? _MaxDate = null;
        private double _EarthRadius = Constants.EarthsRadiusInKm;

        [JsonProperty]
        private List<MagneticModel> Models { get; set; }

        #region getters & setters

        public string Name
        {
            get
            {
                if (Models == null) return string.Empty;

                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string FileName
        {
            get
            {
                if (Models == null) return string.Empty;

                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        public knownModels Type
        {
            get
            {
                if (Models == null) return knownModels.NONE;

                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public double MinDate
        {
            get
            {
                if (Models == null) return double.NaN;

                return Convert.ToDouble(_MinDate);
            }
            set
            {
                if (!value.IsValidYear()) return;

                if(_MinDate == null || value <= _MinDate) _MinDate = value;
            }
        }

        public double MaxDate
        {
            get
            {
                if (Models == null) return double.NaN;

                return Convert.ToDouble(_MaxDate);
            }
            set
            {
                if (!value.IsValidYear()) return;

                if (_MaxDate == null || _MaxDate <= value) _MaxDate = value;
            }
        }

        public double EarthRadius
        {
            get
            {
                if (Models == null) return double.NaN;

                return _EarthRadius;
            }
            set
            {
                _EarthRadius = value;
            }
        }

        [JsonIgnore]
        public List<MagneticModel> GetModels
        {
            get
            {
                return new List<MagneticModel>(Models);
            }
        }

        [JsonIgnore]
        public Int32 NumberOfModels
        {
            get
            {
                if (Models == null) return -1;

                return Models.Count;
            }
        }
        #endregion
    }

    public class MagneticModel
    {
        #region Constructors

        public MagneticModel()
        {
            Type = string.Empty;
            Year = -1;
            //EarthRadius = Constants.EarthsRadiusInKm;

            SharmCoeff = new List<double>();
        }

        public MagneticModel(MagneticModel other)
        {
            Type = other.Type;
            Year = other.Year;
            //EarthRadius = other.EarthRadius;

            SharmCoeff = new List<double>();
            if (other.SharmCoeff.Any()) SharmCoeff.AddRange(SharmCoeff);
        }

        #endregion
        
        public string Type { get; set; }
        public double Year { get; set; }
        public List<double> SharmCoeff;

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

                return Convert.ToInt32(rmax);
            }
        }
    }

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

    /// <summary>
    /// Magnetic Vector Object
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

        #region Public Memembers

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
        /// North Intensity (NT)
        /// </summary>
        public double x { get; set; }

        /// <summary>
        ///  East Intensity (nT)
        /// </summary>
        public double y { get; set; }

        /// <summary>
        ///  Vertical Intensity (nT)
        /// </summary>
        public double z { get; set; }

        /// <summary>
        ///  Total Intensity (nT)
        /// </summary>
        public double f { get; set; }

        #endregion

    }

}
