/****************************************************************************
 * File:            Preferences.cs
 * Description:     Application preferences with JSON serialization
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using Newtonsoft.Json;
using System;
using System.IO;

namespace GeoMagSharp
{
    /// <summary>
    /// Application preferences for coordinate display, units, and defaults
    /// </summary>
    public class Preferences
    {
        private bool _UseDecimalDegrees = true;
        private bool _UseAltitude = true;
        private string _FieldUnit = @"nT";
        private string _AltitudeUnits = @"ft";
        private string _LatitudeHemisphere = @"N";
        private string _LongitudeHemisphere = @"W";

        #region Getters & Setters

        public bool UseDecimalDegrees
        {
            get { return _UseDecimalDegrees; }
            set { _UseDecimalDegrees = value; }
        }

        public bool UseAltitude
        {
            get { return _UseAltitude; }
            set { _UseAltitude = value; }
        }

        public string FieldUnit
        {
            get { return _FieldUnit; }
            set { _FieldUnit = value; }
        }

        public string AltitudeUnits
        {
            get { return _AltitudeUnits; }
            set { _AltitudeUnits = value; }
        }

        public string LatitudeHemisphere
        {
            get { return _LatitudeHemisphere; }
            set { _LatitudeHemisphere = value; }
        }

        public string LongitudeHemisphere
        {
            get { return _LongitudeHemisphere; }
            set { _LongitudeHemisphere = value; }
        }

        #endregion

        #region Constructors

        public Preferences()
        {
        }

        public Preferences(Preferences other)
        {
            UseDecimalDegrees = other.UseDecimalDegrees;
            UseAltitude = other.UseAltitude;
            FieldUnit = other.FieldUnit;
        }

        #endregion

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
                Console.WriteLine("Preferences Error: {0}", ex.ToString());
                wasSucessful = false;
            }

            return wasSucessful;
        }

        public static Preferences Load(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return new Preferences();

            if (!System.IO.File.Exists(filename)) return new Preferences();

            Preferences outData = null;

            try
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                using (var sr = new System.IO.StreamReader(filename))
                using (var reader = new JsonTextReader(sr))
                {
                    outData = JsonConvert.DeserializeObject<Preferences>(serializer.Deserialize(reader).ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Preferences Error: {0}", ex.ToString());
                outData = null;
            }

            return outData;
        }

        #endregion
    }
}
