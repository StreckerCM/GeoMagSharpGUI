using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace GeoMagSharp
{
    public static class FileReader
    {
        public static MagModelSet ReadFileCOF(string modelFile)
        {
            //ModelList = new List<MagModel>();

            var outModels = new MagModelSet();

            using (var stream = new StreamReader(modelFile))
            {
                string inbuff;

                Int32 fileline = 0;                            /* First line will be 1 */

                Int32 modelI = -1;                             /* First model will be 0 */

                var irecPos = new Int64[Constants.MaxModules];

                while ((inbuff = stream.ReadLine()) != null)
                {
                    fileline++;

                    if (!inbuff.Length.Equals(Constants.RecordLen))
                    {
                        // Console.WriteLine("Corrupt record in file {0} on line {1}.", mdFile, fileline);
                        stream.Close();
                        outModels = null;
                        throw new GeoMagExceptionBadCharacter(string.Format("Corrupt record in file {0} on line {1}", modelFile, fileline));
                        //return;
  
                    }

                    if (inbuff.Substring(0, 3).Equals("   ", StringComparison.Ordinal)) /* If 1st 3 chars are spaces */
                    {
                        modelI++;                                           /* New model */

                        if (modelI > Constants.MaxModules)                  /* If too many headers */
                        {
                            //Console.WriteLine("Too many models in file {0} on line {1}.", mdFile, fileline);
                            stream.Close();
                            outModels = null;
                            throw new GeoMagExceptionBadCharacter(string.Format("Too many models in file {0} on line {1}", modelFile, fileline));
                            //return;
                        }

                        irecPos[modelI] = stream.BaseStream.Position;
                        /* Get fields from buffer into individual vars.  */
                        var lineParase = inbuff.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        var currentModel = new MagModel();

                        for (Int32 itemIdx = 0; itemIdx < lineParase.Count(); itemIdx++)
                        {
                            double tempDbl;
                            Int32 tempInt;

                            switch (itemIdx)
                            {
                                //model (string)
                                case 0:
                                    currentModel.Model = lineParase[itemIdx];
                                    break;

                                //epoch (double)
                                case 1:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    currentModel.Epoch = tempDbl;
                                    break;

                                //max1 (int)
                                case 2:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    currentModel.Max1 = tempInt;
                                    break;

                                //max2 (int)
                                case 3:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    currentModel.Max2 = tempInt;
                                    break;

                                //max3 (int)
                                case 4:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    currentModel.Max3 = tempInt;
                                    break;

                                //yrmin (double)
                                case 5:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    currentModel.YearMin = tempDbl;
                                    break;

                                //yrmax (double)
                                case 6:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    currentModel.YearMax = tempDbl;
                                    break;

                                //altmin (double)
                                case 7:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    currentModel.AltitudeMin = tempDbl;
                                    break;

                                //altmax (double)
                                case 8:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    currentModel.AltitudeMax = tempDbl;
                                    break;
                            }
                        }

                        outModels.AddModel(currentModel);

                    } /* If 1st 3 chars are spaces */
                    else
                    {
                        /* Get fields from buffer into individual vars.  */
                        var lineParase = inbuff.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        var coeffLine = new SphericalHarmonicCoefficient();

                        for (Int32 itemIdx = 0; itemIdx < lineParase.Count(); itemIdx++)
                        {
                            double tempDbl;
                            Int32 tempInt;

                            switch (itemIdx)
                            {
                                //Degree(n) (int)
                                case 0:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    coeffLine.Degree = tempInt;
                                    break;

                                //Order(m) (int)
                                case 1:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    coeffLine.Order = tempInt;
                                    break;

                                //g1 (double)
                                case 2:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    coeffLine.G1 = tempDbl;
                                    break;

                                //h1 (double)
                                case 3:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    coeffLine.H1 = tempDbl;
                                    break;

                                //g2 (double)
                                case 4:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    coeffLine.G2 = tempDbl;
                                    break;

                                //h2 (double)
                                case 5:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    coeffLine.H2 = tempDbl;
                                    break;

                                //irat (string)
                                case 6:
                                    coeffLine.Model = lineParase[itemIdx];
                                    break;

                                //LineNum (int)
                                case 7:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    coeffLine.LineNum = tempInt;
                                    break;
                            }
                        }

                        var modelResult = outModels.GetModels.Find(m => m.Model.Equals(coeffLine.Model, StringComparison.OrdinalIgnoreCase));

                        if (modelResult != null) modelResult.Coefficients.Add(coeffLine);
                    }


                } /* While not end of model file */
            }

            return outModels;
        }

        public static ModelSetBGGM ReadFileDAT(string modelFile)
        {
            var outModels = new ModelSetBGGM
                {
                    FileName = modelFile
                };

            double tempDbl = 0;

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
                        }
                        else if (inbuff.IndexOf("M", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("S", StringComparison.OrdinalIgnoreCase).Equals(0) ||
                            inbuff.IndexOf("E", StringComparison.OrdinalIgnoreCase).Equals(0)) /* If 1st 3 chars are spaces */
                        {
                            modelI++;                                           /* New model */

                            double.TryParse(lineParase.Last(), NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);

                            outModels.AddModel(new ModelBGGM
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

            return outModels;

        }
    }
}
