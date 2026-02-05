/****************************************************************************
 * File:            ModelReader.cs
 * Description:     Routines to read magnetic model coefficient files into
 *                  the model structure for calculation
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;
using System.Globalization;
using System.Linq;
using System.IO;

namespace GeoMagSharp
{
    /// <summary>
    /// Reads and parses magnetic model coefficient files (COF/DAT) into model structures.
    /// </summary>
    public static class ModelReader
    {
        /// <summary>
        /// Reads a magnetic model from a coefficient file.
        /// </summary>
        /// <param name="modelFile">Path to the coefficient file (.COF or .DAT)</param>
        /// <returns>A MagneticModelSet containing the parsed model data</returns>
        /// <exception cref="GeoMagExceptionFileNotFound">File does not exist</exception>
        /// <exception cref="GeoMagExceptionOpenError">File is locked by another process</exception>
        /// <exception cref="GeoMagExceptionModelNotLoaded">File type not supported or no models found</exception>
        /// <exception cref="GeoMagExceptionBadCharacter">File contains invalid or malformed data</exception>
        public static MagneticModelSet Read(string modelFile)
        {
            if (string.IsNullOrWhiteSpace(modelFile))
                throw new ArgumentNullException(nameof(modelFile), "Model file path cannot be null or empty");

            if (!File.Exists(modelFile))
                throw new GeoMagExceptionFileNotFound(string.Format("Error: The file '{0}' was not found",
                    modelFile));

            if (IsFileLocked(modelFile))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(modelFile)));

            switch(Path.GetExtension(modelFile).ToUpper())
            {
                case ".COF":
                    return COFreader(modelFile);

                case ".DAT":
                    return DATreader(modelFile);

            }

            throw new GeoMagExceptionModelNotLoaded(string.Format(String.Format("Error: The file type '{0}' is not supported",
                                    Path.GetExtension(modelFile).ToUpper())));
        }

        /// <summary>
        /// Reads a magnetic model from a coefficient file with a separate secular variation file.
        /// </summary>
        /// <param name="modelFile">Path to the main coefficient file (.COF or .DAT)</param>
        /// <param name="svFile">Path to the secular variation file</param>
        /// <returns>A MagneticModelSet containing the parsed model data</returns>
        public static MagneticModelSet Read(string modelFile, string svFile)
        {
            if (string.IsNullOrWhiteSpace(modelFile))
                throw new ArgumentNullException(nameof(modelFile), "Model file path cannot be null or empty");

            if (!File.Exists(modelFile))
                throw new GeoMagExceptionFileNotFound(string.Format("Error: The file '{0}' was not found",
                    modelFile));

            if (IsFileLocked(modelFile))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(modelFile)));

            if (!string.IsNullOrWhiteSpace(svFile))
            {
                if (!File.Exists(svFile))
                    throw new GeoMagExceptionFileNotFound(string.Format("Error: The secular variation file '{0}' was not found",
                        svFile));

                if (IsFileLocked(svFile))
                    throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                        Path.GetFileName(svFile)));
            }

            switch (Path.GetExtension(modelFile).ToUpper())
            {
                case ".COF":
                    return COFreader(modelFile);

                case ".DAT":
                    return DATreader(modelFile);

            }

            throw new GeoMagExceptionModelNotLoaded(string.Format(String.Format("Error: The file type '{0}' is not supported",
                                    Path.GetExtension(modelFile).ToUpper())));
        }

        /// <summary>
        /// Detects whether a COF file header uses the new format (year first) or old format (model name first).
        /// New format (WMM2020+): "    2020.0            WMM-2020        12/10/2019"
        /// Old format (WMM2015):  "   WMM-2015  2015.00 12 12  0 2014.75 2020.00..."
        /// </summary>
        /// <param name="headerLine">The first line of the COF file</param>
        /// <returns>True if new format (year first), false if old format (model name first)</returns>
        private static bool IsNewHeaderFormat(string headerLine)
        {
            if (string.IsNullOrWhiteSpace(headerLine))
                return false;

            string trimmed = headerLine.TrimStart();
            return trimmed.Length > 0 && char.IsDigit(trimmed[0]);
        }

        /// <summary>
        /// Checks if a line is the end-of-file marker (line of 9's) used in WMM2020+ files.
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <returns>True if this is an EOF marker line</returns>
        private static bool IsEndOfFileMarker(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            // WMM2020+ uses a line of 48 nines as terminator
            // Check if line starts with many 9's (at least 10)
            string trimmed = line.Trim();
            return trimmed.Length >= 10 && trimmed.StartsWith("9999999999");
        }

        /// <summary>
        /// Extracts the model year from a COF header line, handling both old and new formats.
        /// </summary>
        /// <param name="headerLine">The header line</param>
        /// <param name="lineFields">The whitespace-split fields of the header line</param>
        /// <param name="isNewFormat">Whether this is the new (WMM2020+) format</param>
        /// <param name="modelType">The detected model type</param>
        /// <param name="lineNumber">Line number for error reporting</param>
        /// <returns>The model epoch year as a decimal</returns>
        private static double ExtractModelYear(string headerLine, string[] lineFields, bool isNewFormat,
            knownModels modelType, int lineNumber)
        {
            if (modelType.Equals(knownModels.EMM))
            {
                // EMM format: year is first field
                ValidateArrayLength(lineFields, 1, lineNumber);
                return ParseDouble(lineFields[0], lineNumber, "model year");
            }

            if (isNewFormat)
            {
                // New format (WMM2020+): year is first field
                // Example: "    2020.0            WMM-2020        12/10/2019"
                ValidateArrayLength(lineFields, 1, lineNumber);
                return ParseDouble(lineFields[0], lineNumber, "model year");
            }
            else
            {
                // Old format (WMM2015/IGRF): year is second field after model name
                // Example: "   WMM-2015  2015.00 12 12  0 2014.75 2020.00..."
                ValidateArrayLength(lineFields, 2, lineNumber);
                return ParseDouble(lineFields[1], lineNumber, "model year");
            }
        }

        private static MagneticModelSet COFreader(string modelFile)
        {
            var outModels = new MagneticModelSet();

            outModels.FileNames.Add(Path.GetFileName(modelFile));

            double tempDbl = 0;
            bool isNewFormat = false;

            using (var stream = new StreamReader(modelFile))
            {
                string inbuff;

                Int32 mModelIdx = -1;                             /* First model will be 0 */

                Int32 eModelIdx = -1;                             /* First model will be 0 */

                Int32 lineNumber = 0;

                while ((inbuff = stream.ReadLine()) != null)
                {
                    lineNumber++;

                    inbuff = inbuff.Trim();

                    if (!string.IsNullOrEmpty(inbuff))
                    {
                        // Check for end-of-file marker (WMM2020+ format uses 999...999)
                        if (IsEndOfFileMarker(inbuff))
                        {
                            break; // End of coefficient data
                        }

                        var lineParase = inbuff.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (lineNumber.Equals(1))
                        {
                            outModels.Type = inbuff.CheckStringForModel();
                            isNewFormat = IsNewHeaderFormat(inbuff);
                        }

                        if (!inbuff.CheckStringForModel().Equals(knownModels.NONE))
                        {
                            /* New model - extract year using format-aware parsing */
                            tempDbl = ExtractModelYear(inbuff, lineParase, isNewFormat, outModels.Type, lineNumber);

                            outModels.AddModel(new MagneticModel
                            {
                                Type = @"M",
                                Year = tempDbl
                            });

                            mModelIdx = outModels.GetModels.Count() - 1;

                            outModels.AddModel(new MagneticModel
                            {
                                Type = @"S",
                                Year = tempDbl
                            });

                            eModelIdx = outModels.GetModels.Count() - 1;

                        }
                        else if (mModelIdx > -1)
                        {
                            // Parse coefficient line - both EMM and standard COF use identical format
                            ParseCOFCoefficients(lineParase, lineNumber, outModels, mModelIdx, eModelIdx);
                        }
                    }
                }

            }

            //Add 5 years to the start date of the final model
            outModels.MaxDate += 5;

            if (outModels.NumberOfModels.Equals(0))
                throw new GeoMagExceptionModelNotLoaded(string.Format("Error: No models were detected in the specified file{0}File Name: {1}",
                                                        Environment.NewLine, Path.GetFileName(modelFile)));

            return outModels;
        }

        private static MagneticModelSet DATreader(string modelFile)
        {
            var outModels = new MagneticModelSet();

            outModels.FileNames.Add(Path.GetFileName(modelFile));

            double tempDbl = 0;

            double earthRadius = 6371.001;                 /* old (default) reference radius */

            using (var stream = new StreamReader(modelFile))
            {
                string inbuff;

                Int32 modelI = -1;                             /* First model will be 0 */

                Int32 lineNumber = 0;

                while ((inbuff = stream.ReadLine()) != null)
                {
                    lineNumber++;

                    inbuff = inbuff.Trim();

                    if(!string.IsNullOrEmpty(inbuff))
                    {
                        var lineParase = inbuff.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (lineNumber.Equals(1))
                        {
                            //Min Year - must be a valid year
                            tempDbl = ParseDouble(inbuff, lineNumber, "minimum year");
                            outModels.MinDate = tempDbl;
                        }
                        else if (lineNumber.Equals(2))
                        {
                            //Max Year - must be a valid year
                            tempDbl = ParseDouble(inbuff, lineNumber, "maximum year");
                            outModels.MaxDate = tempDbl;
                        }
                        else if (inbuff.IndexOf("N", StringComparison.OrdinalIgnoreCase).Equals(0))
                        {
                            /* ignore the rest of the line */
                            earthRadius = Constants.EarthsRadiusInKm;	/* new reference radius */
                        }
                        else if (inbuff.IndexOf("M", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("S", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("E", StringComparison.OrdinalIgnoreCase).Equals(0)) /* Model type marker line */
                        {
                            modelI++;                                           /* New model */

                            // Validate we have at least 2 elements (type and year)
                            ValidateArrayLength(lineParase, 2, lineNumber);
                            tempDbl = ParseDouble(lineParase.Last(), lineNumber, "model year");

                            outModels.AddModel(new MagneticModel
                                {
                                    Type = lineParase.First(),
                                    Year = tempDbl
                                });

                        }
                        else if (modelI > -1)
                        {
                            /* read in coefficient values for this model */
                            foreach (var ptr in lineParase)
                            {
                                tempDbl = ParseDouble(ptr, lineNumber, "coefficient");
                                outModels.AddCoefficients(modelI, tempDbl);
                            }
                        }

                    }

                }

            }

            if (outModels.NumberOfModels.Equals(0))
                throw new GeoMagExceptionModelNotLoaded(string.Format("Error: No models were detected in the specified file{0}File Name: {1}",
                                                        Environment.NewLine, Path.GetFileName(modelFile)));

            outModels.EarthRadius = earthRadius;

            return outModels;

        }

        public static bool IsFileLocked(string filePath)
        {
            if (!filePath.Equals(string.Empty))
            {
                try
                {
                    using (File.Open(filePath, FileMode.Open)) { }
                }
                catch (IOException e)
                {
                    var errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(e) & ((1 << 16) - 1);

                    return errorCode == 32 || errorCode == 33;
                }
            }

            return false;
        }

        /// <summary>
        /// Parses a COF coefficient data line and adds coefficients to the model.
        /// This method handles both EMM and standard COF formats, which share identical parsing logic.
        /// </summary>
        /// <param name="lineParase">The whitespace-split fields of the data line</param>
        /// <param name="lineNumber">Line number for error reporting</param>
        /// <param name="outModels">The MagneticModelSet to add coefficients to</param>
        /// <param name="mModelIdx">Index of the main (M) model</param>
        /// <param name="eModelIdx">Index of the secular variation (S) model</param>
        private static void ParseCOFCoefficients(string[] lineParase, int lineNumber,
            MagneticModelSet outModels, int mModelIdx, int eModelIdx)
        {
            // Both EMM and standard COF format require at least 6 columns: degree, order, g1, h1, g2, h2
            ValidateArrayLength(lineParase, 6, lineNumber);

            // Parse degree and order for validation and coefficient placement logic
            int lineDegree = ParseInt(lineParase[0], lineNumber, "degree");
            int lineOrder = ParseInt(lineParase[1], lineNumber, "order");

            // Validate spherical harmonic constraints: degree >= 1, order >= 0, order <= degree
            if (lineDegree < 1)
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Invalid degree {0} at line {1}. Degree must be >= 1", lineDegree, lineNumber));
            if (lineOrder < 0 || lineOrder > lineDegree)
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Invalid order {0} at line {1}. Order must be between 0 and degree ({2})",
                    lineOrder, lineNumber, lineDegree));

            // g1 coefficient (main field)
            double g1 = ParseDouble(lineParase[2], lineNumber, "g1 coefficient");
            outModels.AddCoefficients(mModelIdx, g1);

            // h1 coefficient (main field, only if order > 0)
            double h1 = ParseDouble(lineParase[3], lineNumber, "h1 coefficient");
            if (lineOrder > 0) outModels.AddCoefficients(mModelIdx, h1);

            // g2 coefficient (secular variation)
            double g2 = ParseDouble(lineParase[4], lineNumber, "g2 coefficient");
            outModels.AddCoefficients(eModelIdx, g2);

            // h2 coefficient (secular variation, only if order > 0)
            double h2 = ParseDouble(lineParase[5], lineNumber, "h2 coefficient");
            if (lineOrder > 0) outModels.AddCoefficients(eModelIdx, h2);
        }

        /// <summary>
        /// Safely parses a double value with validation.
        /// </summary>
        /// <param name="value">String value to parse</param>
        /// <param name="lineNumber">Line number for error reporting</param>
        /// <param name="fieldName">Field name for error reporting</param>
        /// <returns>Parsed double value</returns>
        /// <exception cref="GeoMagExceptionBadCharacter">Value could not be parsed</exception>
        private static double ParseDouble(string value, int lineNumber, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Empty or null value for {0} at line {1}", fieldName, lineNumber));

            double result;
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Invalid numeric value '{0}' for {1} at line {2}", value, fieldName, lineNumber));

            return result;
        }

        /// <summary>
        /// Safely parses an integer value with validation.
        /// </summary>
        /// <param name="value">String value to parse</param>
        /// <param name="lineNumber">Line number for error reporting</param>
        /// <param name="fieldName">Field name for error reporting</param>
        /// <returns>Parsed integer value</returns>
        /// <exception cref="GeoMagExceptionBadCharacter">Value could not be parsed</exception>
        private static int ParseInt(string value, int lineNumber, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Empty or null value for {0} at line {1}", fieldName, lineNumber));

            int result;
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Invalid integer value '{0}' for {1} at line {2}", value, fieldName, lineNumber));

            return result;
        }

        /// <summary>
        /// Validates that an array has at least the required number of elements.
        /// </summary>
        /// <param name="array">Array to check</param>
        /// <param name="requiredCount">Minimum required elements</param>
        /// <param name="lineNumber">Line number for error reporting</param>
        /// <exception cref="GeoMagExceptionBadCharacter">Array does not have enough elements</exception>
        private static void ValidateArrayLength(string[] array, int requiredCount, int lineNumber)
        {
            if (array == null || array.Length < requiredCount)
                throw new GeoMagExceptionBadCharacter(string.Format(
                    "Error: Line {0} has insufficient data. Expected at least {1} values, found {2}",
                    lineNumber, requiredCount, array?.Length ?? 0));
        }
    }
}
