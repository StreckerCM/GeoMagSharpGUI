using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace GeoMagSharp
{
    public class GeoMag
    {
        //private List<MagModel> ModelList;

        public List<MagneticCalculations> MagneticResults;

        private CalculationModel _CalculationModel;

        private MagModelSet _ModelsIGRFWMM;

        private ModelSetBGGM _ModelsBGGM;
        
        /* Geomag global variables */
        //private List<double> gh1 = new List<double>();
        //private List<double> gh2 = new List<double>();
        //private double[] gha;
        //private double[] ghb;

        //private double d = 0, f = 0, h = 0, i = 0;
        //private double dtemp, ftemp, htemp, itemp;
        //private double x = 0, y = 0, z = 0;
        //private double xtemp, ytemp, ztemp;

        public GeoMag(string mdFile = null)
        {
            _CalculationModel = CalculationModel.Unknown;

            _ModelsIGRFWMM = null;


            if (!string.IsNullOrEmpty(mdFile)) LoadModel(mdFile);
        }

        public void LoadModel(string modelFile)
        {
            try
            {

                switch (Path.GetExtension(modelFile).ToLower())
                {
                    case ".cof":
                        _ModelsIGRFWMM = FileReader.ReadFileCOF(modelFile);
                        _CalculationModel = CalculationModel.IGRFWMM;
                        break;

                    case ".dat":
                        _ModelsBGGM = FileReader.ReadFileDAT(modelFile);
                        _CalculationModel = CalculationModel.BGGM;
                        break;
                }

            }
            catch(Exception ex)
            {
                _ModelsIGRFWMM = null;
                _ModelsBGGM = null;
                _CalculationModel = CalculationModel.Unknown;
            }

        }

        //public void MagneticCalculations(DateTime startDate, DateTime endDate, double latitude, double longitude, double altitude, double stepInterval = 0)
        public void MagneticCalculations(Options CalculationOptions)
        {

            switch (_CalculationModel)
            {
                case CalculationModel.IGRFWMM:
                    if (_ModelsIGRFWMM == null) throw new GeoMagExceptionModelNotLoaded("Model Not Loaded");
                    MagneticCalculationsIGRFWMM(_ModelsIGRFWMM, CalculationOptions);
                    break;

                case CalculationModel.BGGM:
                    if (_ModelsBGGM == null) throw new GeoMagExceptionModelNotLoaded("Model Not Loaded");
                    GeoMagBGGM.MagneticCalculations(_ModelsBGGM, CalculationOptions);
                    break;

                case CalculationModel.HDGM:
                    break;

                default:
                    break;
            }
        }

        //public void MagneticCalculationsIGRFWMM(DateTime startDate, DateTime endDate, double latitude, double longitude, double altitude, double stepInterval = 0)
        public void MagneticCalculationsIGRFWMM(MagModelSet magModels, Options CalculationOptions)
        {

            MagneticResults = new List<MagneticCalculations>();

            TimeSpan timespan = (CalculationOptions.EndDate.Date - CalculationOptions.StartDate.Date);

            double incrament = (timespan.TotalDays + 0.25) * CalculationOptions.StepInterval;

            if (timespan.TotalDays.Equals(0)) timespan = (CalculationOptions.EndDate.AddDays(1) - CalculationOptions.StartDate);

            if (incrament.Equals(0)) incrament = 1;

            var avaliableModels = _ModelsIGRFWMM.GetModels;

            for (double dateIdx = 0; dateIdx < timespan.TotalDays; dateIdx += incrament)
            {
                DateTime intervalDate = CalculationOptions.StartDate.AddDays(dateIdx);

                Int32 ghaNmax = 0;
                Int32 ghbNmax = 0;

                double[] gha = null;
                double[] ghb = null;

                var modelToUse =
                    avaliableModels.Find(m => m.YearMin <= intervalDate.ToDecimal() && intervalDate.ToDecimal() <= m.YearMax);

                if (modelToUse == null) throw new GeoMagExceptionOutOfRange("No suitble model could be located for the given date(s)");

                if (modelToUse.Max2.Equals(0))
                {
                    var laterModelToUse = avaliableModels[avaliableModels.IndexOf(modelToUse) + 1];

                    if (laterModelToUse == null) throw new GeoMagExceptionOutOfRange("No suitble model could be located for the given date(s)");
                    
                    //nmaxGh3
                    //    nmaxGh4
                    gha = InterpolatesSphericalHarmonic(intervalDate.ToDecimal(), out ghaNmax, modelToUse, laterModelToUse);
                    ghb = InterpolatesSphericalHarmonic(intervalDate.ToDecimal() + 1, out ghbNmax, modelToUse, laterModelToUse);
                    
                }
                else
                {
                    gha = ExtrapolatesSphericalHarmonic(intervalDate.ToDecimal(), out ghaNmax, modelToUse);
                    ghb = ExtrapolatesSphericalHarmonic(intervalDate.ToDecimal() + 1, out ghbNmax, modelToUse);
                }


                var aPoint = (gha == null ? null : CalculateSphericalHarmonicField(CoordinateSystem.Geodetic, CalculationOptions.Latitude, CalculationOptions.Longitude, CalculationOptions.Depth, ghaNmax, gha));
                var bPoint = (ghb == null ? null : CalculateSphericalHarmonicField(CoordinateSystem.Geodetic, CalculationOptions.Latitude, CalculationOptions.Longitude, CalculationOptions.Depth, ghbNmax, ghb));

                double da = 0;
                double fa = 0;
                double ha = 0;
                double ia = 0;

                CalculateDIHF(aPoint, out da, out fa, out ha, out ia);

                double db = 0;
                double fb = 0;
                double hb = 0;
                double ib = 0;

                CalculateDIHF(bPoint, out db, out fb, out hb, out ib);

                double ddot = (db - da).ToDegree();

                if (ddot > 180.0) ddot -= 360.0;

                if (ddot <= -180.0) ddot += 360.0;

                double idot = ((ib - ia).ToDegree());

                var dDeg = da.ToDegree();
                var iDeg = ia.ToDegree();

                double hdot = hb - ha;
                double fdot = fb - fa;

                double xdot = bPoint.X - aPoint.X;
                double ydot = bPoint.Y - aPoint.Y;
                double zdot = bPoint.Z - aPoint.Z;
   
                /* deal with geographic and magnetic poles */

                double warn_H_val = 0;
                double warn_H_strong_val = 0;

                bool warn_H = false;
                bool warn_H_strong = false;
                bool warn_P = false;

                if (ha < 100.0) /* at magnetic poles */
                {
                    dDeg = Double.NaN;
                    ddot = Double.NaN;
                    /* while rest is ok */
                }

                if (ha < 1000.0)
                {
                    warn_H = false;
                    warn_H_strong = true;
                    if (ha < warn_H_strong_val) warn_H_strong_val = ha;
                }
                else if (ha < 5000.0 && !warn_H_strong)
                {
                    warn_H = true;
                    if (ha < warn_H_val) warn_H_val = ha;
                }

                if (90.0 - Math.Abs(CalculationOptions.Latitude) <= 0.001) /* at geographic poles */
                {
                    aPoint.X = Double.NaN;
                    aPoint.Y = Double.NaN;
                    dDeg = Double.NaN;
                    xdot = Double.NaN;
                    ydot = Double.NaN;
                    ddot = Double.NaN;
                    warn_P = true;
                    warn_H = false;
                    warn_H_strong = false;
                    /* while rest is ok */
                }

                MagneticResults.Add(new MagneticCalculations
                {
                    Date = intervalDate,
                    Declination = new MagneticValue
                    {
                        Value = dDeg,
                        ChangePerYear = ddot
                    },
                    Inclination = new MagneticValue
                    {
                        Value = ia.ToDegree(),
                        ChangePerYear = idot
                    },
                    HorizontalIntensity = new MagneticValue
                    {
                        Value = ha,
                        ChangePerYear = hdot
                    },
                    NorthComp = new MagneticValue
                    {
                        Value = aPoint.X,
                        ChangePerYear = xdot
                    },
                    EastComp = new MagneticValue
                    {
                        Value = aPoint.Y,
                        ChangePerYear = ydot
                    },
                    VerticalComp = new MagneticValue
                    {
                        Value = aPoint.Z,
                        ChangePerYear = zdot
                    },
                    TotalField = new MagneticValue
                    {
                        Value = fa,
                        ChangePerYear = fdot
                    }
                });

            }
        }



        //public Int32 ExtrapolatesSphericalHarmonic(double date, double dte1, Int32 nmax1, Int32 nmax2, Int32 gh)
   // public Int32 ExtrapolatesSphericalHarmonic(double date, MagModel baseModel , Int32 gh)
        private double[] ExtrapolatesSphericalHarmonic(double date, out Int32 nmax, MagModel baseModel)
    {
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

        //int   nmax;
        nmax = 0;
        int   k;
        int   ii;

        var gh = new double[baseModel.GH1.Count()];

        double factor = date - baseModel.Epoch;

        if (baseModel.Max1.Equals(baseModel.Max2))
        {
            k = baseModel.Max1 * (baseModel.Max1 + 2);
            nmax = baseModel.Max1;
        }
        else
        {
            int   l;

            if (baseModel.Max1 > baseModel.Max2)
            {
                k = baseModel.Max2*(baseModel.Max2 + 2);
                l = baseModel.Max1*(baseModel.Max1 + 2);

                for (ii = k; ii < l; ii++)
                {
                    gh[ii] = baseModel.GH1[ii];
                }

                //switch (gh)
                //{
                //    case 3: //for ( ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            gha[ii] = baseModel.GH1[ii];
                //        }
                //        break;

                //    case 4: //for ( ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            ghb[ii] = baseModel.GH1[ii];
                //        }
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Extrapolates Spherical Harmonic");
                //        break;

                //}
                nmax = baseModel.Max1;
            }
            else
            {
                k = baseModel.Max1*(baseModel.Max1 + 2);
                l = baseModel.Max2*(baseModel.Max2 + 2);

                for (ii = k; ii < l; ii++)
                {
                    gh[ii] = factor * baseModel.GH2[ii];
                }

                //switch (gh)
                //{
                //    case 3: //for ( ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            gha[ii] = factor*baseModel.GH2[ii];
                //        }
                //        break;

                //    case 4: //for ( ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            ghb[ii] = factor*baseModel.GH2[ii];
                //        }
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Extrapolates Spherical Harmonic");
                //        break;

                //}

                nmax = baseModel.Max2;
            }
        }

        for (ii = 0; ii < k; ii++)
        {
            gh[ii] = baseModel.GH1[ii] + factor * baseModel.GH2[ii];
        }

        //switch (gh)
        //{
        //    case 3: //for ( ii = 1; ii <= k; ++ii)
        //        for (ii = 0; ii < k; ii++)
        //        {
        //            gha[ii] = baseModel.GH1[ii] + factor*baseModel.GH2[ii];
        //        }
        //        break;

        //    case 4: //for ( ii = 1; ii <= k; ++ii)
        //        for (ii = 0; ii < k; ii++)
        //        {
        //            ghb[ii] = baseModel.GH1[ii] + factor*baseModel.GH2[ii];
        //        }
        //        break;

        //    default:
        //        Console.WriteLine("Error in subroutine Extrapolates Spherical Harmonic");
        //        break;
        //}

        return gh;
    }
        
    //private int InterpolatesSphericalHarmonic(double date, MagModel ealierModel, MagModel laterModel, Int32 gh)
    private double[] InterpolatesSphericalHarmonic(double date, out Int32 nmax, MagModel ealierModel, MagModel laterModel)
    {
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

        //int nmax;
        nmax = 0;
        int k;
        int ii;

        var gh = new double[ealierModel.GH1.Count()];

        double factor = (date - ealierModel.Epoch) / (laterModel.Epoch - ealierModel.Epoch);

        if (ealierModel.Max1 == laterModel.Max2)
        {
            k = ealierModel.Max1 * (ealierModel.Max1 + 2);
            nmax = ealierModel.Max1;
        }
        else
        {
            int l;
            if (ealierModel.Max1 > laterModel.Max2)
            {
                k = laterModel.Max2 * (laterModel.Max2 + 2);
                l = ealierModel.Max1 * (ealierModel.Max1 + 2);

                for (ii = k; ii < l; ii++)
                {
                    gh[ii] = ealierModel.GH1[ii] + factor * (-ealierModel.GH1[ii]);
                }

                //switch (gh)
                //{
                //    case 3:
                //        //for (ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            gha[ii] = ealierModel.GH1[ii] + factor * (-ealierModel.GH1[ii]);
                //        }
                //        break;

                //    case 4:
                //        //for (ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            ghb[ii] = ealierModel.GH1[ii] + factor * (-ealierModel.GH1[ii]);
                //        }
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Interpolates Spherical Harmonic");
                //        break;
                //}

                nmax = ealierModel.Max1;
            }
            else
            {
                k = ealierModel.Max1 * (ealierModel.Max1 + 2);
                l = laterModel.Max2 * (laterModel.Max2 + 2);

                for (ii = k; ii < l; ii++)
                {
                    gh[ii] = factor * laterModel.GH2[ii];
                }

                //switch (gh)
                //{
                //    case 3:
                //        //for (ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            gha[ii] = factor * laterModel.GH2[ii];
                //        }
                //        break;

                //    case 4:
                //        //for (ii = k + 1; ii <= l; ++ii)
                //        for (ii = k; ii < l; ii++)
                //        {
                //            ghb[ii] = factor * laterModel.GH2[ii];
                //        }
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Interpolates Spherical Harmonic");
                //        break;
                //}

                nmax = laterModel.Max2;
            }
        }

        for (ii = 0; ii < k; ii++)
        {
            gh[ii] = ealierModel.GH1[ii] + factor * (laterModel.GH2[ii] - ealierModel.GH1[ii]);
        }

        //switch (gh)
        //{
        //    case 3:
        //        //for (ii = 1; ii <= k; ++ii)
        //        for (ii = 0; ii < k; ii++)
        //        {
        //            gha[ii] = ealierModel.GH1[ii] + factor * (laterModel.GH2[ii] - ealierModel.GH1[ii]);
        //        }
        //        break;

        //    case 4:
        //        //for (ii = 1; ii <= k; ++ii)
        //        for (ii = 0; ii < k; ii++)
        //        {
        //            ghb[ii] = ealierModel.GH1[ii] + factor * (laterModel.GH2[ii] - ealierModel.GH1[ii]);
        //        }
        //        break;

        //    default:
        //        Console.WriteLine("Error in subroutine Interpolates Spherical Harmonic");
        //        break;
        //}

        return gh;
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


    //private Point3D CalculateSphericalHarmonicField(CoordinateSystem igdgc, double flat, double flon, double elev,
    //    Int32 nmax, Int32 gh, Int32 iext = 0, double ext1 = 0.0, double ext2 = 0.0, double ext3 = 0.0)
    private Point3D CalculateSphericalHarmonicField(CoordinateSystem igdgc, double flat, double flon, double elev,
        Int32 nmax, double[] gh, Int32 iext = 0, double ext1 = 0.0, double ext2 = 0.0, double ext3 = 0.0)
    {
        //double earths_radius = 6371.2;
        //double dtr = 0.01745329;
        double slat;
        double clat;
        double ratio;
        double aa, bb, cc, dd;
        double sd = 0;
        double cd = 0;
        double r = 0;
        //double a2;
        //double b2;
        double rr = 0;
        double fm = 0;
        double fn = 0;
        var sl = new double[14];
        var cl = new double[14];
        var p = new double[119];
        var q = new double[119];
        int ii, j, k, l, m, n;
        int npq;
        int ios;
        //double argument;
        double power;
        //a2 = 40680631.59;            /* WGS84 */
        //b2 = 40408299.98;            /* WGS84 */
        ios = 0;
        r = elev;
        //argument = flat * dtr;
        slat = Math.Sin(flat.ToRadian());

        if ((90.0 - flat) < 0.001)
        {
            aa = Constants.ThreeHundredFeetFromNorthPole;            /*  300 ft. from North pole  */
        }
        else
        {
            if ((90.0 + flat) < 0.001)
            {
                aa = Constants.ThreeHundredFeetFromSouthPole;        /*  300 ft. from South pole  */
            }
            else
            {
                aa = flat;
            }
        }

        //argument = aa * dtr;
        clat = Math.Cos(aa.ToRadian());
        //argument = flon * dtr;
        sl[1] = Math.Sin(flon.ToRadian());
        cl[1] = Math.Cos(flon.ToRadian());

        //switch (gh)
        //{
        //    case 3:
        //        x = 0;
        //        y = 0;
        //        z = 0;
        //        break;

        //    case 4:
        //        xtemp = 0;
        //        ytemp = 0;
        //        ztemp = 0;
        //        break;

        //    default:
        //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
        //        break;
        //}

        var outPoint = new Point3D();

        sd = 0.0;
        cd = 1.0;
        l = 1;
        n = 0;
        m = 1;
        npq = (nmax * (nmax + 3)) / 2;

        if (igdgc.Equals(CoordinateSystem.Geodetic))
        {
            aa = Constants.A2WGS84 * clat * clat;
            bb = Constants.B2WGS84 * slat * slat;
            cc = aa + bb;
            //argument = cc;
            dd = Math.Sqrt(cc);
            //argument = elev * (elev + 2.0 * dd) + (a2 * aa + b2 * bb) / cc;
            r = Math.Sqrt(elev * (elev + 2.0 * dd) + (Constants.A2WGS84 * aa + Constants.B2WGS84 * bb) / cc);
            cd = (elev + dd) / r;
            sd = (Constants.A2WGS84 - Constants.B2WGS84) / dd * slat * clat / r;
            aa = slat;
            slat = slat * cd - clat * sd;
            clat = clat * cd + aa * sd;
        }

        ratio = Constants.EarthsRadiusInKm / r;
        //argument = 3.0;
        aa = Math.Sqrt(3.0);
        p[1] = 2.0 * slat;
        p[2] = 2.0 * clat;
        p[3] = 4.5 * slat * slat - 1.5;
        p[4] = 3.0 * aa * clat * slat;
        q[1] = -clat;
        q[2] = slat;
        q[3] = -3.0 * clat * slat;
        q[4] = aa * (slat * slat - clat * clat);

        for (k = 1; k <= npq; k++)
        {
            if (n < m)
            {
                m = 0;
                n++;
                //argument = ratio;
                power = n + 2;
                rr = Math.Pow(ratio, power);
                fn = n;
            }

            fm = m;

            if (k >= 5)
            {
                if (m == n)
                {
                    //argument = (1.0 - 0.5/fm);
                    aa = Math.Sqrt((1.0 - 0.5 / fm));
                    j = k - n - 1;
                    p[k] = (1.0 + 1.0 / fm) * aa * clat * p[j];
                    q[k] = aa * (clat * q[j] + slat / fm * p[j]);
                    sl[m] = sl[m - 1] * cl[1] + cl[m - 1] * sl[1];
                    cl[m] = cl[m - 1] * cl[1] - sl[m - 1] * sl[1];
                }
                else
                {
                    //argument = fn*fn - fm*fm;
                    aa = Math.Sqrt(fn * fn - fm * fm);
                    //argument = ((fn - 1.0)*(fn-1.0)) - (fm * fm);
                    bb = Math.Sqrt(((fn - 1.0) * (fn - 1.0)) - (fm * fm)) / aa;
                    cc = (2.0 * fn - 1.0) / aa;
                    ii = k - n;
                    j = k - 2 * n + 1;
                    p[k] = (fn + 1.0) * (cc * slat / fn * p[ii] - bb / (fn - 1.0) * p[j]);
                    q[k] = cc * (slat * q[ii] - clat / fn * p[ii]) - bb * q[j];
                }
            }

            aa = rr * gh[l-1];

            //switch (gh)
            //{
            //    case 3: 
            //        aa = rr * gha[l-1];
            //        break;

            //    case 4: 
            //        aa = rr * ghb[l-1];
            //        break;

            //    default:
            //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
            //        break;
            //}

            if (m == 0)
            {
                //switch (gh)
                //{
                //    case 3:
                //        outPoint.X = outPoint.X + aa * q[k];
                //        outPoint.Z = outPoint.Z - aa * p[k];
                //        //x = x + aa * q[k];
                //        //z = z - aa * p[k];
                //        break;

                //    case 4: 
                //        outPoint.X = outPoint.X + aa * q[k];
                //        outPoint.Z = outPoint.Z - aa * p[k];
                //        xtemp = xtemp + aa * q[k];
                //        ztemp = ztemp - aa * p[k];
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
                //        break;
                //}

                outPoint.X = outPoint.X + aa * q[k];
                outPoint.Z = outPoint.Z - aa * p[k];

                l = l + 1;
            }
            else
            {
                bb = rr * gh[l];
                cc = aa * cl[m] + bb * sl[m];
                outPoint.X = outPoint.X + cc * q[k];
                outPoint.Z = outPoint.Z - cc * p[k];

                if (clat > 0)
                {
                    outPoint.Y = outPoint.Y + (aa * sl[m] - bb * cl[m]) * fm * p[k] / ((fn + 1.0) * clat);
                }
                else
                {
                    outPoint.Y = outPoint.Y + (aa * sl[m] - bb * cl[m]) * q[k] * slat;
                }

                l += 2;

                //switch (gh)
                //{
                //    case 3: 
                //        //bb = rr * gha[l + 1];
                //        bb = rr * gha[l];
                //        cc = aa * cl[m] + bb * sl[m];
                //        x = x + cc * q[k];
                //        z = z - cc * p[k];

                //        if (clat > 0)
                //        {
                //            y = y + (aa * sl[m] - bb * cl[m]) * fm * p[k] / ((fn + 1.0) * clat);
                //        }
                //        else
                //        {
                //            y = y + (aa * sl[m] - bb * cl[m]) * q[k] * slat;
                //        }

                //        l += 2;
                //        break;

                //    case 4: 
                //        //bb = rr * ghb[l + 1];
                //        bb = rr * ghb[l];
                //        cc = aa * cl[m] + bb * sl[m];
                //        xtemp = xtemp + cc * q[k];
                //        ztemp = ztemp - cc * p[k];

                //        if (clat > 0)
                //        {
                //            ytemp = ytemp + (aa * sl[m] - bb * cl[m]) * fm * p[k] / ((fn + 1.0) * clat);
                //        }
                //        else
                //        {
                //            ytemp = ytemp + (aa * sl[m] - bb * cl[m]) * q[k] * slat;
                //        }

                //        l += 2;
                //        break;

                //    default:
                //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
                //        break;
                //}
            }


            m++;
        }

        if (iext != 0)
        {
            aa = ext2 * cl[1] + ext3 * sl[1];

            outPoint.X = outPoint.X - ext1 * clat + aa * slat;
            outPoint.Y = outPoint.Y + ext2 * sl[1] - ext3 * cl[1];
            outPoint.Z = outPoint.Z + ext1 * slat + aa * clat;

            //switch (gh)
            //{
            //    case 3:
            //        x = x - ext1 * clat + aa * slat;
            //        y = y + ext2 * sl[1] - ext3 * cl[1];
            //        z = z + ext1 * slat + aa * clat;
            //        break;

            //    case 4:
            //        xtemp = xtemp - ext1 * clat + aa * slat;
            //        ytemp = ytemp + ext2 * sl[1] - ext3 * cl[1];
            //        ztemp = ztemp + ext1 * slat + aa * clat;
            //        break;

            //    default:
            //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
            //        break;
            //}
        }

        aa = outPoint.X;
        outPoint.X = outPoint.X * cd + outPoint.Z * sd;
        outPoint.Z = outPoint.Z * cd - aa * sd;

        //switch (gh)
        //{
        //    case 3:
        //        aa = x;
        //        x = x * cd + z * sd;
        //        z = z * cd - aa * sd;
        //        break;

        //    case 4:
        //        aa = xtemp;
        //        xtemp = xtemp * cd + ztemp * sd;
        //        ztemp = ztemp * cd - aa * sd;
        //        break;

        //    default:
        //        Console.WriteLine("Error in subroutine Calculate Spherical Harmonic Field");
        //        break;
        //}

        return outPoint;

    }


   //int CalculateDIHF(Int32 gh)
    private void CalculateDIHF(Point3D point, out double d, out double f, out double h, out double i)
    {
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
        //double 
        
        d = 0;
        f = 0;
        h = 0;
        i = 0;

        //int ios = gh;
        int j;

        double h2;
        double hpx;
        //double argument, argument2;

        for (j = 1; j <= 1; ++j)
        {
            h2 = point.X * point.X + point.Y * point.Y;
            //argument = h2;
            h = Math.Sqrt(h2);       /* calculate horizontal intensity */
            //argument = h2 + z * z;
            f = Math.Sqrt(h2 + point.Z * point.Z);      /* calculate total intensity */
            if (f < Constants.SN)
            {
                d = double.NaN;        /* If d and i cannot be determined, */
                i = double.NaN;        /*       set equal to NaN         */
            }
            else
            {
                //argument = z;
                //argument2 = h;
                i = Math.Atan2(point.Z, h);
                if (h < Constants.SN)
                {
                    d = double.NaN;
                }
                else
                {
                    hpx = h + point.X;
                    if (hpx < Constants.SN)
                    {
                        d = Math.PI;
                    }
                    else
                    {
                        //argument = y;
                        //argument2 = hpx;
                        d = 2.0 * Math.Atan2(point.Y, hpx);
                    }
                }

            }
        }


        //switch (gh)
        //{
        //    case 3: for (j = 1; j <= 1; ++j)
        //        {
        //            h2 = x * x + y * y;
        //            argument = h2;
        //            h = Math.Sqrt(argument);       /* calculate horizontal intensity */
        //            argument = h2 + z * z;
        //            f = Math.Sqrt(argument);      /* calculate total intensity */
        //            if (f < Constants.SN)
        //            {
        //                d = double.NaN;        /* If d and i cannot be determined, */
        //                i = double.NaN;        /*       set equal to NaN         */
        //            }
        //            else
        //            {
        //                argument = z;
        //                argument2 = h;
        //                i = Math.Atan2(argument, argument2);
        //                if (h < Constants.SN)
        //                {
        //                    d = double.NaN;
        //                }
        //                else
        //                {
        //                    hpx = h + x;
        //                    if (hpx < Constants.SN)
        //                    {
        //                        d = Math.PI;
        //                    }
        //                    else
        //                    {
        //                        argument = y;
        //                        argument2 = hpx;
        //                        d = 2.0 * Math.Atan2(argument, argument2);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    case 4: for (j = 1; j <= 1; ++j)
        //        {
        //            h2 = xtemp * xtemp + ytemp * ytemp;
        //            argument = h2;
        //            htemp = Math.Sqrt(argument);
        //            argument = h2 + ztemp * ztemp;
        //            ftemp = Math.Sqrt(argument);
        //            if (ftemp < Constants.SN)
        //            {
        //                dtemp = double.NaN;    /* If d and i cannot be determined, */
        //                itemp = double.NaN;    /*       set equal to 999.0         */
        //            }
        //            else
        //            {
        //                argument = ztemp;
        //                argument2 = htemp;
        //                itemp = Math.Atan2(argument, argument2);
        //                if (htemp < Constants.SN)
        //                {
        //                    dtemp = double.NaN;
        //                }
        //                else
        //                {
        //                    hpx = htemp + xtemp;
        //                    if (hpx < Constants.SN)
        //                    {
        //                        dtemp = Math.PI;
        //                    }
        //                    else
        //                    {
        //                        argument = ytemp;
        //                        argument2 = hpx;
        //                        dtemp = 2.0 * Math.Atan2(argument, argument2);
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    default: Console.WriteLine("Error in subroutine dihf");
        //        break;
        //}
        //return (ios);
    }

    }
}
