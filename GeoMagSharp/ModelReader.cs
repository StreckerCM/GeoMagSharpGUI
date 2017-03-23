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
        public static MagneticModelSet Read(string modelFile)
        {
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

        public static MagneticModelSet Read(string modelFile, string svFile)
        {
            if (IsFileLocked(modelFile))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(modelFile)));

            if (IsFileLocked(svFile))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(svFile)));

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
                            /* New model */
                            if (outModels.Type.Equals(knownModels.EMM))
                            {
                                double.TryParse(lineParase[0], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                            }
                            else
                            {
                                double.TryParse(lineParase[1], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
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
                                #region Split File Line Reader
                                Int32 lineDegree = -1;

                                Int32 lineOrder = -1;

                                for (Int32 itemIdx = 0; itemIdx < lineParase.Count(); itemIdx++)
                                {
                                    switch (itemIdx)
                                    {
                                        //Degree(n) (int)
                                        case 0:
                                            Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out lineDegree);
                                            break;

                                        //Order(m) (int)
                                        case 1:
                                            Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out lineOrder);
                                            break;

                                        //g1 (double)
                                        case 2:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            outModels.AddCoefficients(mModelIdx, tempDbl);
                                            break;

                                        //h1 (double)
                                        case 3:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            if (lineOrder > 0) outModels.AddCoefficients(mModelIdx, tempDbl);
                                            break;

                                        //g2 (double)
                                        case 4:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            outModels.AddCoefficients(eModelIdx, tempDbl);
                                            break;

                                        //h2 (double)
                                        case 5:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            if (lineOrder > 0) outModels.AddCoefficients(eModelIdx, tempDbl);
                                            break;

                                        //irat (string)
                                        case 6:
                                            //coeffLine.Model = lineParase[itemIdx];
                                            break;

                                        //LineNum (int)
                                        case 7:
                                            //Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                            //coeffLine.LineNum = tempInt;
                                            break;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Single File Line Reader
                                Int32 lineDegree = -1;

                                Int32 lineOrder = -1;

                                for (Int32 itemIdx = 0; itemIdx < lineParase.Count(); itemIdx++)
                                {
                                    switch (itemIdx)
                                    {
                                        //Degree(n) (int)
                                        case 0:
                                            Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out lineDegree);
                                            break;

                                        //Order(m) (int)
                                        case 1:
                                            Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out lineOrder);
                                            break;

                                        //g1 (double)
                                        case 2:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            outModels.AddCoefficients(mModelIdx, tempDbl);
                                            break;

                                        //h1 (double)
                                        case 3:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            if (lineOrder > 0) outModels.AddCoefficients(mModelIdx, tempDbl);
                                            break;

                                        //g2 (double)
                                        case 4:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            outModels.AddCoefficients(eModelIdx, tempDbl);
                                            break;

                                        //h2 (double)
                                        case 5:
                                            double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                            if (lineOrder > 0) outModels.AddCoefficients(eModelIdx, tempDbl);
                                            break;

                                        //irat (string)
                                        case 6:
                                            //coeffLine.Model = lineParase[itemIdx];
                                            break;

                                        //LineNum (int)
                                        case 7:
                                            //Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                            //coeffLine.LineNum = tempInt;
                                            break;
                                    }
                                }
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
                            //Min Year
                            double.TryParse(inbuff, NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

                            outModels.MinDate = tempDbl;
                        }
                        else if (lineNumber.Equals(2))
                        {
                            //Max Year
                            double.TryParse(inbuff, NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

                            outModels.MaxDate = tempDbl;
                        }
                        else if (inbuff.IndexOf("N", StringComparison.OrdinalIgnoreCase).Equals(0))
                        {
                            /* ignore the rest of the line */
                            earthRadius = Constants.EarthsRadiusInKm;	/* new reference radius */
                        }
                        else if (inbuff.IndexOf("M", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("S", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("E", StringComparison.OrdinalIgnoreCase).Equals(0)) /* If 1st 3 chars are spaces */
                        {
                            modelI++;                                           /* New model */

                            double.TryParse(lineParase.Last(), NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

                            outModels.AddModel(new MagneticModel
                                {
                                    Type = lineParase.First(),
                                    Year = tempDbl
                                });

                        }
                        else if (modelI > -1)
                        {
                        
                            /* read in more values for this era */

                            foreach (var ptr in lineParase)
                            {
                         
                                double.TryParse(ptr, NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

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
    }
}
