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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace GeoMagSharp
{
    public static class ModelReader
    {
        public static GeoMagModelSet Load(string modelFile)
        {
            if (IsFileLocked(modelFile))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(modelFile)));

            switch(Path.GetExtension(modelFile).ToUpper())
            {
                case ".COF":
                    return Reader_COF(modelFile);

                case ".DAT":
                    return Reader_DAT(modelFile);

            }

            throw new GeoMagExceptionModelNotLoaded(string.Format(String.Format("Error: The file type '{0}' is not supported",
                                    Path.GetExtension(modelFile).ToUpper())));
        }

        private static GeoMagModelSet Reader_COF(string modelFile)
        {
            var outModels = new GeoMagModelSet
            {
                FileName = modelFile
            };

            double tempDbl = 0;

            using (var stream = new StreamReader(modelFile))
            {
                string inbuff;

                Int32 mModelIdx = -1;                             /* First model will be 0 */

                Int32 eModelIdx = -1;                             /* First model will be 0 */

                Int32 lineNumber = 0;

                //List<double> one = null;

                //List<double> two = null;

                while ((inbuff = stream.ReadLine()) != null)
                {
                    lineNumber++;

                    inbuff = inbuff.Trim();

                    if (!string.IsNullOrEmpty(inbuff))
                    {
                        var lineParase = inbuff.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (inbuff.IndexOf("IGRF", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("DGRF", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("WMM", StringComparison.OrdinalIgnoreCase).Equals(0))
                        {
                                                                      /* New model */

                            //one = new List<double>();

                            //two = new List<double>();

                            double.TryParse(lineParase[1], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

                            outModels.AddModel(new GeoMagModel
                            {
                                Type = @"M",
                                Year = tempDbl
                            });

                            mModelIdx = outModels.GetModels.Count() - 1; 

                            outModels.AddModel(new GeoMagModel
                            {
                                Type = @"S",
                                Year = tempDbl
                            });

                            eModelIdx = outModels.GetModels.Count() - 1;
                        }
                        else if (mModelIdx > -1)
                        {
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

        private static GeoMagModelSet Reader_DAT(string modelFile)
        {
            var outModels = new GeoMagModelSet
                {
                    FileName = modelFile
                };

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

                            outModels.AddModel(new GeoMagModel
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
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
    }
}
