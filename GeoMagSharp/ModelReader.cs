/****************************************************************************
 * File:            FileReader.cs
 * Description:     Routines read a given model file into the model structure
 *                  to be used for calculation
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;
using System.Globalization;
using System.Linq;
using System.IO;

namespace GeoMagSharp
{
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

        private static MagneticModelSet COFreader(string modelFile)
        {
            var outModels = new MagneticModelSet();

            outModels.FileNames.Add(Path.GetFileName(modelFile));

            double tempDbl = 0;

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
                        var lineParase = inbuff.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if(lineNumber.Equals(1)) outModels.Type = inbuff.CheckStringForModel();

                        if (!inbuff.CheckStringForModel().Equals(knownModels.NONE))
                        {
                            /* New model - validate we have enough columns for year parsing */
                            if (outModels.Type.Equals(knownModels.EMM))
                            {
                                ValidateArrayLength(lineParase, 1, lineNumber);
                                tempDbl = ParseDouble(lineParase[0], lineNumber, "model year");
                            }
                            else
                            {
                                ValidateArrayLength(lineParase, 2, lineNumber);
                                tempDbl = ParseDouble(lineParase[1], lineNumber, "model year");
                            }

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
                            if(outModels.Type.Equals(knownModels.EMM))
                            {
                                #region EMM File Line Reader
                                // EMM format requires at least 6 columns: degree, order, g1, h1, g2, h2
                                ValidateArrayLength(lineParase, 6, lineNumber);

                                Int32 lineDegree = ParseInt(lineParase[0], lineNumber, "degree");
                                Int32 lineOrder = ParseInt(lineParase[1], lineNumber, "order");

                                // g1 coefficient
                                tempDbl = ParseDouble(lineParase[2], lineNumber, "g1 coefficient");
                                outModels.AddCoefficients(mModelIdx, tempDbl);

                                // h1 coefficient (only if order > 0)
                                tempDbl = ParseDouble(lineParase[3], lineNumber, "h1 coefficient");
                                if (lineOrder > 0) outModels.AddCoefficients(mModelIdx, tempDbl);

                                // g2 coefficient
                                tempDbl = ParseDouble(lineParase[4], lineNumber, "g2 coefficient");
                                outModels.AddCoefficients(eModelIdx, tempDbl);

                                // h2 coefficient (only if order > 0)
                                tempDbl = ParseDouble(lineParase[5], lineNumber, "h2 coefficient");
                                if (lineOrder > 0) outModels.AddCoefficients(eModelIdx, tempDbl);
                                #endregion
                            }
                            else
                            {
                                #region Standard COF File Line Reader
                                // Standard COF format requires at least 6 columns: degree, order, g1, h1, g2, h2
                                ValidateArrayLength(lineParase, 6, lineNumber);

                                Int32 lineDegree = ParseInt(lineParase[0], lineNumber, "degree");
                                Int32 lineOrder = ParseInt(lineParase[1], lineNumber, "order");

                                // g1 coefficient
                                tempDbl = ParseDouble(lineParase[2], lineNumber, "g1 coefficient");
                                outModels.AddCoefficients(mModelIdx, tempDbl);

                                // h1 coefficient (only if order > 0)
                                tempDbl = ParseDouble(lineParase[3], lineNumber, "h1 coefficient");
                                if (lineOrder > 0) outModels.AddCoefficients(mModelIdx, tempDbl);

                                // g2 coefficient
                                tempDbl = ParseDouble(lineParase[4], lineNumber, "g2 coefficient");
                                outModels.AddCoefficients(eModelIdx, tempDbl);

                                // h2 coefficient (only if order > 0)
                                tempDbl = ParseDouble(lineParase[5], lineNumber, "h2 coefficient");
                                if (lineOrder > 0) outModels.AddCoefficients(eModelIdx, tempDbl);
                                #endregion
                            }
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
