using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace GeoMagSharp
{
    public class GeoMag
    {
        //private List<MagModel> ModelList;

        private MagModelSet Models;

        private const Int32 RECL = 80;

        /** Max number of models in a file **/
        private const Int32 MAXMOD = 30;

        public GeoMag(string mdFile = null)
        {
            Models = null;

            if (!string.IsNullOrEmpty(mdFile)) LoadModel(mdFile);
        }

        public void LoadModel(string mdFile)
        {
            //ModelList = new List<MagModel>();

            Models = new MagModelSet();

            using (var stream = new System.IO.StreamReader(mdFile)) 
            {
                string inbuff;

                double tempDbl;
                Int32 tempInt;

                Int32 fileline = 0;                            /* First line will be 1 */

                Int32 modelI = -1;                             /* First model will be 0 */

                var irec_pos = new Int64[MAXMOD];

                while ((inbuff = stream.ReadLine()) != null)
                {
                    fileline++;

                    if (!inbuff.Length.Equals(RECL))
                    {
                        Console.WriteLine("Corrupt record in file {0} on line {1}.", mdFile, fileline);
                        stream.Close();
                        Models = null;
                        return;
                    }


                    /* old statement Dec 1999 */
                    /*       if (!strncmp(inbuff,"    ",4)){         /* If 1st 4 chars are spaces */
                    /* New statement Dec 1999 changed by wmd  required by year 2000 models */
                    //if (!strncmp(inbuff, "   ", 3))         /* If 1st 3 chars are spaces */
                    if (inbuff.Substring(0, 3).Equals("   ", StringComparison.Ordinal)) /* If 1st 3 chars are spaces */
                    {
                        modelI++;                           /* New model */

                        if (modelI > MAXMOD)                /* If too many headers */
                        {
                            Console.WriteLine("Too many models in file {0} on line {1}.", mdFile, fileline);
                            stream.Close();
                            Models = null;
                            return;
                        }

                        irec_pos[modelI] = stream.BaseStream.Position;
                        /* Get fields from buffer into individual vars.  */
                        var lineParase = inbuff.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        MagModel CurrentModel = new MagModel();

                        for (Int32 itemIdx = 0; itemIdx < lineParase.Count(); itemIdx++)
                        {
                            switch (itemIdx)
                            {
                                //model (string)
                                case 0:
                                    CurrentModel.Model = lineParase[itemIdx];
                                    break;

                                //epoch (double)
                                case 1:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    CurrentModel.Epoch = tempDbl;
                                    break;

                                //max1 (int)
                                case 2:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    CurrentModel.Max1 = tempInt;
                                    break;

                                //max2 (int)
                                case 3:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    CurrentModel.Max2 = tempInt;
                                    break;

                                //max3 (int)
                                case 4:
                                    Int32.TryParse(lineParase[itemIdx], NumberStyles.Integer, CultureInfo.InvariantCulture, out tempInt);
                                    CurrentModel.Max3 = tempInt;
                                    break;

                                //yrmin (double)
                                case 5:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    CurrentModel.YearMin = tempDbl;
                                    break;

                                //yrmax (double)
                                case 6:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    CurrentModel.YearMax = tempDbl;
                                    break;

                                //altmin (double)
                                case 7:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    CurrentModel.AltitudeMin = tempDbl;
                                    break;

                                //altmax (double)
                                case 8:
                                    double.TryParse(lineParase[itemIdx], NumberStyles.Float, CultureInfo.InvariantCulture, out tempDbl);
                                    CurrentModel.AltitudeMax = tempDbl;
                                    break;
                            }
                        }

                        Models.AddModel(CurrentModel);

                    } /* If 1st 3 chars are spaces */
                } /* While not end of model file */
            }
        }

    }
}
