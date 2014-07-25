using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoMagSharp
{
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

    }
}
