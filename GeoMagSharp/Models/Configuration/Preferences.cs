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

        /// <summary>Whether to display coordinates in decimal degrees (true) or degrees/minutes/seconds (false).</summary>
        public bool UseDecimalDegrees
        {
            get { return _UseDecimalDegrees; }
            set { _UseDecimalDegrees = value; }
        }

        /// <summary>Whether to use altitude (true) or depth (false) for elevation input.</summary>
        public bool UseAltitude
        {
            get { return _UseAltitude; }
            set { _UseAltitude = value; }
        }

        /// <summary>The unit for magnetic field intensity display (e.g., "nT").</summary>
        public string FieldUnit
        {
            get { return _FieldUnit; }
            set { _FieldUnit = value; }
        }

        /// <summary>The unit for altitude/depth display (e.g., "ft", "m").</summary>
        public string AltitudeUnits
        {
            get { return _AltitudeUnits; }
            set { _AltitudeUnits = value; }
        }

        /// <summary>Default latitude hemisphere ("N" or "S").</summary>
        public string LatitudeHemisphere
        {
            get { return _LatitudeHemisphere; }
            set { _LatitudeHemisphere = value; }
        }

        /// <summary>Default longitude hemisphere ("E" or "W").</summary>
        public string LongitudeHemisphere
        {
            get { return _LongitudeHemisphere; }
            set { _LongitudeHemisphere = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default preferences.
        /// </summary>
        public Preferences()
        {
        }

        /// <summary>
        /// Initializes a new instance by copying values from another <see cref="Preferences"/>.
        /// </summary>
        /// <param name="other">The source preferences to copy from.</param>
        public Preferences(Preferences other)
        {
            UseDecimalDegrees = other.UseDecimalDegrees;
            UseAltitude = other.UseAltitude;
            FieldUnit = other.FieldUnit;
        }

        #endregion

        #region Object Serializers

        /// <summary>
        /// Serializes the preferences to a JSON file.
        /// </summary>
        /// <param name="filename">The file path to save to.</param>
        /// <returns><c>true</c> if the save was successful; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Deserializes preferences from a JSON file.
        /// </summary>
        /// <param name="filename">The file path to load from.</param>
        /// <returns>The loaded <see cref="Preferences"/>, or a new default instance if the file is missing or invalid.</returns>
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
