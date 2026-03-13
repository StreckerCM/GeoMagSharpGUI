using GeoMagSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoMagGUI
{
    public static class ResultsExporter
    {
        public static async Task ExportCsvAsync(
            string fileName,
            IEnumerable<MagneticCalculations> results,
            CalculationOptions options,
            string modelName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultsList = results.ToList();
            var last = resultsList.Last();
            var ci = CultureInfo.InvariantCulture;
            var sb = new StringBuilder();

            // Metadata comment lines
            sb.AppendLine(string.Format(ci, "# Model: {0}", modelName));
            sb.AppendLine(string.Format(ci, "# Latitude: {0:F7}", options.Latitude));
            sb.AppendLine(string.Format(ci, "# Longitude: {0:F7}", options.Longitude));
            sb.AppendLine(string.Format(ci, "# Elevation: {0:F4} km", options.AltitudeInKm));

            // Column header
            sb.AppendLine("Date,Declination (deg),Inclination (deg),Horizontal Intensity (nT),North Comp (nT),East Comp (nT),Vertical Comp (nT),Total Field (nT)");

            // Data rows
            foreach (var mag in resultsList)
            {
                sb.AppendLine(string.Format(ci,
                    "{0},{1},{2},{3},{4},{5},{6},{7}",
                    mag.Date.ToString("yyyy-MM-dd"),
                    mag.Declination.Value.ToString("F4", ci),
                    mag.Inclination.Value.ToString("F4", ci),
                    mag.HorizontalIntensity.Value.ToString("F2", ci),
                    mag.NorthComp.Value.ToString("F2", ci),
                    mag.EastComp.Value.ToString("F2", ci),
                    mag.VerticalComp.Value.ToString("F2", ci),
                    mag.TotalField.Value.ToString("F2", ci)));
            }

            // Secular variation row (from last result)
            sb.AppendLine(string.Format(ci,
                "Change Per Year,{0},{1},{2},{3},{4},{5},{6}",
                last.Declination.ChangePerYear.ToString("F4", ci),
                last.Inclination.ChangePerYear.ToString("F4", ci),
                last.HorizontalIntensity.ChangePerYear.ToString("F2", ci),
                last.NorthComp.ChangePerYear.ToString("F2", ci),
                last.EastComp.ChangePerYear.ToString("F2", ci),
                last.VerticalComp.ChangePerYear.ToString("F2", ci),
                last.TotalField.ChangePerYear.ToString("F2", ci)));

            // Uncertainty row (only when available)
            if (last.Uncertainty != null)
            {
                var u = last.Uncertainty;
                sb.AppendLine(string.Format(ci,
                    "Uncertainty (1\u03C3),{0},{1},,,,,{2}",
                    u.Declination.ToString("F4", ci),
                    u.DipAngle.ToString("F4", ci),
                    u.TotalField.ToString("F2", ci)));
            }

            var content = sb.ToString();
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() => File.WriteAllText(fileName, content));
        }

        public static async Task ExportJsonAsync(
            string fileName,
            IEnumerable<MagneticCalculations> results,
            CalculationOptions options,
            string modelName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultsList = results.ToList();
            var last = resultsList.Last();
            var ci = CultureInfo.InvariantCulture;
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            var root = new JObject
            {
                ["model"] = modelName,
                ["version"] = version,
                ["latitude"] = options.Latitude,
                ["longitude"] = options.Longitude,
                ["elevation"] = new JObject
                {
                    ["value"] = options.AltitudeInKm,
                    ["units"] = "km"
                }
            };

            // Results array
            var resultsArray = new JArray();
            foreach (var mag in resultsList)
            {
                resultsArray.Add(new JObject
                {
                    ["date"] = mag.Date.ToString("yyyy-MM-dd"),
                    ["declination"] = Math.Round(mag.Declination.Value, 4),
                    ["inclination"] = Math.Round(mag.Inclination.Value, 4),
                    ["horizontalIntensity"] = Math.Round(mag.HorizontalIntensity.Value, 2),
                    ["northComp"] = Math.Round(mag.NorthComp.Value, 2),
                    ["eastComp"] = Math.Round(mag.EastComp.Value, 2),
                    ["verticalComp"] = Math.Round(mag.VerticalComp.Value, 2),
                    ["totalField"] = Math.Round(mag.TotalField.Value, 2)
                });
            }
            root["results"] = resultsArray;

            // Secular variation (from last result)
            root["secularVariation"] = new JObject
            {
                ["declination"] = Math.Round(last.Declination.ChangePerYear, 4),
                ["inclination"] = Math.Round(last.Inclination.ChangePerYear, 4),
                ["horizontalIntensity"] = Math.Round(last.HorizontalIntensity.ChangePerYear, 2),
                ["northComp"] = Math.Round(last.NorthComp.ChangePerYear, 2),
                ["eastComp"] = Math.Round(last.EastComp.ChangePerYear, 2),
                ["verticalComp"] = Math.Round(last.VerticalComp.ChangePerYear, 2),
                ["totalField"] = Math.Round(last.TotalField.ChangePerYear, 2)
            };

            // Uncertainty (only when available)
            if (last.Uncertainty != null)
            {
                var u = last.Uncertainty;
                root["uncertainty"] = new JObject
                {
                    ["source"] = "ISCWSA",
                    ["sigma"] = 1,
                    ["declination"] = Math.Round(u.Declination, 4),
                    ["inclination"] = Math.Round(u.DipAngle, 4),
                    ["totalField"] = Math.Round(u.TotalField, 2)
                };
            }

            // Units metadata
            root["units"] = new JObject
            {
                ["declination"] = "degrees",
                ["inclination"] = "degrees",
                ["horizontalIntensity"] = "nT",
                ["northComp"] = "nT",
                ["eastComp"] = "nT",
                ["verticalComp"] = "nT",
                ["totalField"] = "nT"
            };

            var content = root.ToString(Formatting.Indented);
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() => File.WriteAllText(fileName, content));
        }
    }
}
