using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
    public class MagModelSet
    {
        public MagModelSet()
        {
            Models = new List<MagModel>();
            YearMin = 0.0;
            YearMax = 0.0;
        }

        public MagModelSet(MagModelSet other)
        {
            Models = new List<MagModel>();
            if(other.Models!=null)Models.AddRange(other.Models);

            YearMin = other.YearMin;
            YearMax = other.YearMax;
        }

        public void AddModel(MagModel newModel)
        {
            if (Models == null) Models = new List<MagModel>();

            Models.Add(newModel);

            /* Compute date range for all models */
            if (Models.Count.Equals(1))
            {
                YearMin = newModel.YearMin;
                YearMax = newModel.YearMax;
            }
            else if(Models.Count > 1)
            {
                if (newModel.YearMin < YearMin) YearMin = newModel.YearMin;

                if (newModel.YearMax > YearMax) YearMax = newModel.YearMax;
            }
        }

        public List<MagModel> Models { get; set; }
        public double YearMin { get; set; }
        public double YearMax { get; set; }
    }

    public class MagModel
    {
        public MagModel()
        {
            Model = string.Empty;
            Epoch = 0.0;
            Max1 = 0;
            Max2 = 0;
            Max3 = 0;
            YearMin = 0.0;
            YearMax = 0.0;
            AltitudeMin = 0.0;
            AltitudeMax = 0.0;

            Coefficients = new List<SphericalHarmonicCoefficient>();
        }

        public MagModel(MagModel other)
        {
            Model = other.Model;
            Epoch = other.Epoch;
            Max1 = other.Max1;
            Max2 = other.Max2;
            Max3 = other.Max3;
            YearMin = other.YearMin;
            YearMax = other.YearMax;
            AltitudeMin = other.AltitudeMin;
            AltitudeMax = other.AltitudeMax;

            Coefficients = new List<SphericalHarmonicCoefficient>();

            if (other.Coefficients != null && other.Coefficients.Any()) Coefficients.AddRange(other.Coefficients);
        }

        public string Model {get; set;}
        public double Epoch {get; set;}
        public Int32 Max1 {get; set;}
        public Int32 Max2 {get; set;}
        public Int32 Max3 {get; set;}
        public double YearMin {get; set;}
        public double YearMax {get; set;}
        public double AltitudeMin {get; set;}
        public double AltitudeMax {get; set;}

        public List<SphericalHarmonicCoefficient> Coefficients { get; set; }

    }

    public class SphericalHarmonicCoefficient
    {
        public SphericalHarmonicCoefficient()
        {
            LineNum = -1;
            N = 0;
            M = 0;
            G = 0.0;
            HH = 0.0;
            Irat = string.Empty;
            Trash = new List<double>();
        }

        public SphericalHarmonicCoefficient(SphericalHarmonicCoefficient other)
        {
            LineNum = other.LineNum;
            N = other.N;
            M = other.M;
            G = other.G;
            HH = other.HH;
            Irat = other.Irat;
            Trash = new List<double>();

            if (other.Trash != null && other.Trash.Any()) Trash.AddRange(other.Trash);
        }

        public Int32 LineNum { get; set; }
        public Int32 N { get; set; }
        public Int32 M { get; set; }
        public double G { get; set; }
        public double HH { get; set; }
        public string Irat { get; set; }
        public List<double> Trash { get; set; }
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
