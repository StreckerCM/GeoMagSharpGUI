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

        private MagModelSet _models;

        private const double Ft2kM = (1.0 / 0.0003048);

        private const double M2kM = 1000.0;

        private const Int32 RECL = 80;

        /** Max number of models in a file **/
        private const Int32 MAXMOD = 30;

        private const Int32 MAXDEG = 13;

        private const Int32 MAXCOEFF = (MAXDEG*(MAXDEG+2)+1);

        /* Geomag global variables */
        private List<double> gh1 = new List<double>();
        private List<double> gh2 = new List<double>();
        private List<double> gha = new List<double>();
        private List<double> ghb = new List<double>();

        private double d = 0,f=0,h=0,i=0;
        private double dtemp,ftemp,htemp,itemp;
        private double x=0,y=0,z=0;
        private double xtemp,ytemp,ztemp;

        public GeoMag(string mdFile = null)
        {
            _models = null;

            if (!string.IsNullOrEmpty(mdFile)) LoadModel(mdFile);
        }

        public void LoadModel(string mdFile)
        {
            //ModelList = new List<MagModel>();

            _models = new MagModelSet();

            using (var stream = new StreamReader(mdFile)) 
            {
                string inbuff;

                Int32 fileline = 0;                            /* First line will be 1 */

                Int32 modelI = -1;                             /* First model will be 0 */

                var irecPos = new Int64[MAXMOD];

                while ((inbuff = stream.ReadLine()) != null)
                {
                    fileline++;

                    if (!inbuff.Length.Equals(RECL))
                    {
                        Console.WriteLine("Corrupt record in file {0} on line {1}.", mdFile, fileline);
                        stream.Close();
                        _models = null;
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
                            _models = null;
                            return;
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

                        _models.AddModel(currentModel);

                    } /* If 1st 3 chars are spaces */
                } /* While not end of model file */
            }
        }

        /****************************************************************************/
        /*                                                                          */
        /*                           Subroutine extrapsh                            */
        /*                                                                          */
        /****************************************************************************/
        /*                                                                          */
        /*     Extrapolates linearly a spherical harmonic model with a              */
        /*     rate-of-change model.                                                */
        /*                                                                          */
        /*     Input:                                                               */
        /*           date     - date of resulting model (in decimal year)           */
        /*           dte1     - date of base model                                  */
        /*           nmax1    - maximum degree and order of base model              */
        /*           gh1      - Schmidt quasi-normal internal spherical             */
        /*                      harmonic coefficients of base model                 */
        /*           nmax2    - maximum degree and order of rate-of-change model    */
        /*           gh2      - Schmidt quasi-normal internal spherical             */
        /*                      harmonic coefficients of rate-of-change model       */
        /*                                                                          */
        /*     Output:                                                              */
        /*           gha or b - Schmidt quasi-normal internal spherical             */
        /*                    harmonic coefficients                                 */
        /*           nmax   - maximum degree and order of resulting model           */
        /*                                                                          */
        /*     FORTRAN                                                              */
        /*           A. Zunde                                                       */
        /*           USGS, MS 964, box 25046 Federal Center, Denver, CO.  80225     */
        /*                                                                          */
        /*     C                                                                    */
        /*           C. H. Shaffer                                                  */
        /*           Lockheed Missiles and Space Company, Sunnyvale CA              */
        /*           August 16, 1988                                                */
        /*                                                                          */
        /****************************************************************************/

        public Int32 Extrapsh(double date, double dte1, Int32 nmax1, Int32 nmax2, Int32 gh)
        {
          int   nmax;
          int   k, l;
          int   ii;
          double factor;
  
          factor = date - dte1;
          if (nmax1 == nmax2)
            {
              k =  nmax1 * (nmax1 + 2);
              nmax = nmax1;
            }
          else
            {
              if (nmax1 > nmax2)
                {
                  k = nmax2 * (nmax2 + 2);
                  l = nmax1 * (nmax1 + 2);
                  switch(gh)
                    {
                    case 3:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          gha[ii] = gh1[ii];
                        }
                      break;
                    case 4:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          ghb[ii] = gh1[ii];
                        }
                      break;
                    default: 
                        Console.WriteLine("Error in subroutine extrapsh");
                      break;
                    }
                  nmax = nmax1;
                }
              else
                {
                  k = nmax1 * (nmax1 + 2);
                  l = nmax2 * (nmax2 + 2);
                  switch(gh)
                    {
                    case 3:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          gha[ii] = factor * gh2[ii];
                        }
                      break;
                    case 4:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          ghb[ii] = factor * gh2[ii];
                        }
                      break;
                    default:
                      Console.WriteLine("Error in subroutine extrapsh");
                      break;
                    }
                  nmax = nmax2;
                }
            }

          switch(gh)
            {
            case 3:  for ( ii = 1; ii <= k; ++ii)
                {
                  gha[ii] = gh1[ii] + factor * gh2[ii];
                }
              break;
            case 4:  for ( ii = 1; ii <= k; ++ii)
                {
                  ghb[ii] = gh1[ii] + factor * gh2[ii];
                }
              break;
            default:
              Console.WriteLine("Error in subroutine extrapsh");
              break;
            }
          return(nmax);
        }


        /****************************************************************************/
        /*                                                                          */
        /*                           Subroutine interpsh                            */
        /*                                                                          */
        /****************************************************************************/
        /*                                                                          */
        /*     Interpolates linearly, in time, between two spherical harmonic       */
        /*     models.                                                              */
        /*                                                                          */
        /*     Input:                                                               */
        /*           date     - date of resulting model (in decimal year)           */
        /*           dte1     - date of earlier model                               */
        /*           nmax1    - maximum degree and order of earlier model           */
        /*           gh1      - Schmidt quasi-normal internal spherical             */
        /*                      harmonic coefficients of earlier model              */
        /*           dte2     - date of later model                                 */
        /*           nmax2    - maximum degree and order of later model             */
        /*           gh2      - Schmidt quasi-normal internal spherical             */
        /*                      harmonic coefficients of internal model             */
        /*                                                                          */
        /*     Output:                                                              */
        /*           gha or b - coefficients of resulting model                     */
        /*           nmax     - maximum degree and order of resulting model         */
        /*                                                                          */
        /*     FORTRAN                                                              */
        /*           A. Zunde                                                       */
        /*           USGS, MS 964, box 25046 Federal Center, Denver, CO.  80225     */
        /*                                                                          */
        /*     C                                                                    */
        /*           C. H. Shaffer                                                  */
        /*           Lockheed Missiles and Space Company, Sunnyvale CA              */
        /*           August 17, 1988                                                */
        /*                                                                          */
        /****************************************************************************/


        int Interpsh(double date, double dte1, Int32 nmax1,double dte2, Int32 nmax2, Int32 gh)
        {
          int   nmax;
          int   k, l;
          int   ii;
          double factor;
  
          factor = (date - dte1) / (dte2 - dte1);
          if (nmax1 == nmax2)
            {
              k =  nmax1 * (nmax1 + 2);
              nmax = nmax1;
            }
          else
            {
              if (nmax1 > nmax2)
                {
                  k = nmax2 * (nmax2 + 2);
                  l = nmax1 * (nmax1 + 2);
                  switch(gh)
                    {
                    case 3:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          gha[ii] = gh1[ii] + factor * (-gh1[ii]);
                        }
                      break;
                    case 4:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          ghb[ii] = gh1[ii] + factor * (-gh1[ii]);
                        }
                      break;
                    default: Console.WriteLine("Error in subroutine interpsh");
                      break;
                    }
                  nmax = nmax1;
                }
              else
                {
                  k = nmax1 * (nmax1 + 2);
                  l = nmax2 * (nmax2 + 2);
                  switch(gh)
                    {
                    case 3:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          gha[ii] = factor * gh2[ii];
                        }
                      break;
                    case 4:  for ( ii = k + 1; ii <= l; ++ii)
                        {
                          ghb[ii] = factor * gh2[ii];
                        }
                      break;
                    default: Console.WriteLine("Error in subroutine interpsh");
                      break;
                    }
                  nmax = nmax2;
                }
            }
          switch(gh)
            {
            case 3:  for ( ii = 1; ii <= k; ++ii)
                {
                  gha[ii] = gh1[ii] + factor * (gh2[ii] - gh1[ii]);
                }
              break;
            case 4:  for ( ii = 1; ii <= k; ++ii)
                {
                  ghb[ii] = gh1[ii] + factor * (gh2[ii] - gh1[ii]);
                }
              break;
            default: Console.WriteLine("Error in subroutine interpsh");
              break;
            }
          return(nmax);
        }


        /****************************************************************************/
        /*                                                                          */
        /*                           Subroutine shval3                              */
        /*                                                                          */
        /****************************************************************************/
        /*                                                                          */
        /*     Calculates field components from spherical harmonic (sh)             */
        /*     models.                                                              */
        /*                                                                          */
        /*     Input:                                                               */
        /*           igdgc     - indicates coordinate system used; set equal        */
        /*                       to 1 if geodetic, 2 if geocentric                  */
        /*           latitude  - north latitude, in degrees                         */
        /*           longitude - east longitude, in degrees                         */
        /*           elev      - WGS84 altitude above ellipsoid (igdgc=1), or       */
        /*                       radial distance from earth's center (igdgc=2)      */
        /*           a2,b2     - squares of semi-major and semi-minor axes of       */
        /*                       the reference spheroid used for transforming       */
        /*                       between geodetic and geocentric coordinates        */
        /*                       or components                                      */
        /*           nmax      - maximum degree and order of coefficients           */
        /*           iext      - external coefficients flag (=0 if none)            */
        /*           ext1,2,3  - the three 1st-degree external coefficients         */
        /*                       (not used if iext = 0)                             */
        /*                                                                          */
        /*     Output:                                                              */
        /*           x         - northward component                                */
        /*           y         - eastward component                                 */
        /*           z         - vertically-downward component                      */
        /*                                                                          */
        /*     based on subroutine 'igrf' by D. R. Barraclough and S. R. C. Malin,  */
        /*     report no. 71/1, institute of geological sciences, U.K.              */
        /*                                                                          */
        /*     FORTRAN                                                              */
        /*           Norman W. Peddie                                               */
        /*           USGS, MS 964, box 25046 Federal Center, Denver, CO.  80225     */
        /*                                                                          */
        /*     C                                                                    */
        /*           C. H. Shaffer                                                  */
        /*           Lockheed Missiles and Space Company, Sunnyvale CA              */
        /*           August 17, 1988                                                */
        /*                                                                          */
        /****************************************************************************/


        int Shval3(Int32 igdgc,double flat,double flon,double elev, Int32 nmax, Int32 gh, Int32 iext,double ext1,double ext2,double ext3)
        {
          double earths_radius = 6371.2;
          double dtr = 0.01745329;
          double slat;
          double clat;
          double ratio;
          double aa, bb, cc, dd;
          double sd;
          double cd;
          double r;
          double a2;
          double b2;
          double rr = 0;
          double fm = 0,fn = 0;
          var sl = new double[14];
          var cl = new double[14];
          var p = new double[119];
          var q = new double[119];
          int ii,j,k,l,m,n;
          int npq;
          int ios;
          double argument;
          double power;
          a2 = 40680631.59;            /* WGS84 */
          b2 = 40408299.98;            /* WGS84 */
          ios = 0;
          r = elev;
          argument = flat * dtr;
          slat = Math.Sin( argument );
          if ((90.0 - flat) < 0.001)
            {
              aa = 89.999;            /*  300 ft. from North pole  */
            }
          else
            {
              if ((90.0 + flat) < 0.001)
                {
                  aa = -89.999;        /*  300 ft. from South pole  */
                }
              else
                {
                  aa = flat;
                }
            }
          argument = aa * dtr;
          clat = Math.Cos( argument );
          argument = flon * dtr;
          sl[1] = Math.Sin( argument );
          cl[1] = Math.Cos( argument );
          switch(gh)
            {
            case 3:  x = 0;
              y = 0;
              z = 0;
              break;
            case 4:  xtemp = 0;
              ytemp = 0;
              ztemp = 0;
              break;
            default: Console.WriteLine("Error in subroutine shval3");
              break;
            }
          sd = 0.0;
          cd = 1.0;
          l = 1;
          n = 0;
          m = 1;
          npq = (nmax * (nmax + 3)) / 2;
          if (igdgc == 1)
            {
              aa = a2 * clat * clat;
              bb = b2 * slat * slat;
              cc = aa + bb;
              argument = cc;
              dd = Math.Sqrt( argument );
              argument = elev * (elev + 2.0 * dd) + (a2 * aa + b2 * bb) / cc;
              r = Math.Sqrt( argument );
              cd = (elev + dd) / r;
              sd = (a2 - b2) / dd * slat * clat / r;
              aa = slat;
              slat = slat * cd - clat * sd;
              clat = clat * cd + aa * sd;
            }
          ratio = earths_radius / r;
          argument = 3.0;
          aa = Math.Sqrt( argument );
          p[1] = 2.0 * slat;
          p[2] = 2.0 * clat;
          p[3] = 4.5 * slat * slat - 1.5;
          p[4] = 3.0 * aa * clat * slat;
          q[1] = -clat;
          q[2] = slat;
          q[3] = -3.0 * clat * slat;
          q[4] = aa * (slat * slat - clat * clat);
          for ( k = 1; k <= npq; ++k)
            {
              if (n < m)
                {
                  m = 0;
                  n = n + 1;
                  argument = ratio;
                  power =  n + 2;
                  rr = Math.Pow(argument,power);
                  fn = n;
                }
              fm = m;
              if (k >= 5)
                {
                  if (m == n)
                    {
                      argument = (1.0 - 0.5/fm);
                      aa = Math.Sqrt( argument );
                      j = k - n - 1;
                      p[k] = (1.0 + 1.0/fm) * aa * clat * p[j];
                      q[k] = aa * (clat * q[j] + slat/fm * p[j]);
                      sl[m] = sl[m-1] * cl[1] + cl[m-1] * sl[1];
                      cl[m] = cl[m-1] * cl[1] - sl[m-1] * sl[1];
                    }
                  else
                    {
                      argument = fn*fn - fm*fm;
                      aa = Math.Sqrt( argument );
                      argument = ((fn - 1.0)*(fn-1.0)) - (fm * fm);
                      bb = Math.Sqrt( argument )/aa;
                      cc = (2.0 * fn - 1.0)/aa;
                      ii = k - n;
                      j = k - 2 * n + 1;
                      p[k] = (fn + 1.0) * (cc * slat/fn * p[ii] - bb/(fn - 1.0) * p[j]);
                      q[k] = cc * (slat * q[ii] - clat/fn * p[ii]) - bb * q[j];
                    }
                }
              switch(gh)
                {
                case 3:  aa = rr * gha[l];
                  break;
                case 4:  aa = rr * ghb[l];
                  break;
                default: Console.WriteLine("Error in subroutine shval3");
                  break;
                }
              if (m == 0)
                {
                  switch(gh)
                    {
                    case 3:  x = x + aa * q[k];
                      z = z - aa * p[k];
                      break;
                    case 4:  xtemp = xtemp + aa * q[k];
                      ztemp = ztemp - aa * p[k];
                      break;
                    default: Console.WriteLine("Error in subroutine shval3");
                      break;
                    }
                  l = l + 1;
                }
              else
                {
                  switch(gh)
                    {
                    case 3:  bb = rr * gha[l+1];
                      cc = aa * cl[m] + bb * sl[m];
                      x = x + cc * q[k];
                      z = z - cc * p[k];
                      if (clat > 0)
                        {
                          y = y + (aa * sl[m] - bb * cl[m]) *
                            fm * p[k]/((fn + 1.0) * clat);
                        }
                      else
                        {
                          y = y + (aa * sl[m] - bb * cl[m]) * q[k] * slat;
                        }
                      l = l + 2;
                      break;
                    case 4:  bb = rr * ghb[l+1];
                      cc = aa * cl[m] + bb * sl[m];
                      xtemp = xtemp + cc * q[k];
                      ztemp = ztemp - cc * p[k];
                      if (clat > 0)
                        {
                          ytemp = ytemp + (aa * sl[m] - bb * cl[m]) *
                            fm * p[k]/((fn + 1.0) * clat);
                        }
                      else
                        {
                          ytemp = ytemp + (aa * sl[m] - bb * cl[m]) *
                            q[k] * slat;
                        }
                      l = l + 2;
                      break;
                    default: Console.WriteLine("Error in subroutine shval3");
                      break;
                    }
                }
              m = m + 1;
            }
          if (iext != 0)
            {
              aa = ext2 * cl[1] + ext3 * sl[1];
              switch(gh)
                {
                case 3:   x = x - ext1 * clat + aa * slat;
                  y = y + ext2 * sl[1] - ext3 * cl[1];
                  z = z + ext1 * slat + aa * clat;
                  break;
                case 4:   xtemp = xtemp - ext1 * clat + aa * slat;
                  ytemp = ytemp + ext2 * sl[1] - ext3 * cl[1];
                  ztemp = ztemp + ext1 * slat + aa * clat;
                  break;
                default:  Console.WriteLine("Error in subroutine shval3");
                  break;
                }
            }
          switch(gh)
            {
            case 3:   aa = x;
		        x = x * cd + z * sd;
		        z = z * cd - aa * sd;
		        break;
            case 4:   aa = xtemp;
		        xtemp = xtemp * cd + ztemp * sd;
		        ztemp = ztemp * cd - aa * sd;
		        break;
            default:  Console.WriteLine("Error in subroutine shval3");
		        break;
            }
          return(ios);
        }


        /****************************************************************************/
        /*                                                                          */
        /*                           Subroutine dihf                                */
        /*                                                                          */
        /****************************************************************************/
        /*                                                                          */
        /*     Computes the geomagnetic d, i, h, and f from x, y, and z.            */
        /*                                                                          */
        /*     Input:                                                               */
        /*           x  - northward component                                       */
        /*           y  - eastward component                                        */
        /*           z  - vertically-downward component                             */
        /*                                                                          */
        /*     Output:                                                              */
        /*           d  - declination                                               */
        /*           i  - inclination                                               */
        /*           h  - horizontal intensity                                      */
        /*           f  - total intensity                                           */
        /*                                                                          */
        /*     FORTRAN                                                              */
        /*           A. Zunde                                                       */
        /*           USGS, MS 964, box 25046 Federal Center, Denver, CO.  80225     */
        /*                                                                          */
        /*     C                                                                    */
        /*           C. H. Shaffer                                                  */
        /*           Lockheed Missiles and Space Company, Sunnyvale CA              */
        /*           August 22, 1988                                                */
        /*                                                                          */
        /****************************************************************************/

        int dihf (Int32 gh)
        {
          int ios;
          int j;
          double sn;
          double h2;
          double hpx;
          double argument, argument2;
  
          ios = gh;
          sn = 0.0001;
  
          switch(gh)
            {
            case 3:   for (j = 1; j <= 1; ++j)
                {
                  h2 = x*x + y*y;
                  argument = h2;
                  h = Math.Sqrt(argument);       /* calculate horizontal intensity */
                  argument = h2 + z*z;
                  f = Math.Sqrt(argument);      /* calculate total intensity */
                  if (f < sn)
                    {
                      d = double.NaN;        /* If d and i cannot be determined, */
                      i = double.NaN;        /*       set equal to NaN         */
                    }
                  else
                    {
                      argument = z;
                      argument2 = h;
                      i = Math.Atan2(argument, argument2);
                      if (h < sn)
                        {
                            d = double.NaN;
                        }
                      else
                        {
                          hpx = h + x;
                          if (hpx < sn)
                            {
                              d = Math.PI;
                            }
                          else
                            {
                              argument = y;
                              argument2 = hpx;
                              d = 2.0 * Math.Atan2(argument,argument2);
                            }
                        }
                    }
                }
		        break;
            case 4:   for (j = 1; j <= 1; ++j)
                {
                  h2 = xtemp*xtemp + ytemp*ytemp;
                  argument = h2;
                  htemp = Math.Sqrt(argument);
                  argument = h2 + ztemp*ztemp;
                  ftemp = Math.Sqrt(argument);
                  if (ftemp < sn)
                    {
                        dtemp = double.NaN;    /* If d and i cannot be determined, */
                      itemp = double.NaN;    /*       set equal to 999.0         */
                    }
                  else
                    {
                      argument = ztemp;
                      argument2 = htemp;
                      itemp = Math.Atan2(argument, argument2);
                      if (htemp < sn)
                        {
                            dtemp = double.NaN;
                        }
                      else
                        {
                          hpx = htemp + xtemp;
                          if (hpx < sn)
                            {
                              dtemp = Math.PI;
                            }
                          else
                            {
                              argument = ytemp;
                              argument2 = hpx;
                              dtemp = 2.0 * Math.Atan2(argument, argument2);
                            }
                        }
                    }
                }
		        break;
            default:  Console.WriteLine("Error in subroutine dihf");
		        break;
            }
          return(ios);
        }

    }
}
