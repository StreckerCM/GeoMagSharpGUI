/****************************************************************************
 * File:            GeoMag.cs
 * Description:     Routines to provide an interface to the calculation methods
 * Author:          Christopher Strecker   
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI  
 * Warnings:
 * Current version: 
 *  ****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace GeoMagSharp
{
    public class GeoMag
    {
        public List<MagneticCalculations> ResultsOfCalculation;

        private MagneticModelSet _Models;

        private CalculationOptions _CalculationOptions;

        public GeoMag()
        {
            _Models = null;
        }

        public void LoadModel(string modelFile)
        {
            _Models = null;

            if (string.IsNullOrEmpty(modelFile))
                throw new GeoMagExceptionFileNotFound("Error coefficient file name not specified");

            _Models = ModelReader.Read(modelFile);

        }

        public void LoadModel(MagneticModelSet modelSet)
        {
            _Models = null;

            if (modelSet == null)
                throw new GeoMagExceptionFileNotFound("Error coefficient file name not specified");

            _Models = modelSet;

        }

        public void LoadModel(string modelFile, string svFile)
        {
            _Models = null;

            if (string.IsNullOrEmpty(modelFile))
                throw new GeoMagExceptionFileNotFound("Error coefficient file name not specified");

            _Models = ModelReader.Read(modelFile, svFile);

        }

        public void MagneticCalculations(CalculationOptions inCalculationOptions)
        {
            _CalculationOptions = null;
            ResultsOfCalculation = null;

            if (_Models == null || _Models.NumberOfModels.Equals(0))
                throw new GeoMagExceptionModelNotLoaded("Error: No models avaliable for calculation");
                    
            if (!_Models.IsDateInRange(inCalculationOptions.StartDate))
            {
                throw new GeoMagExceptionOutOfRange(string.Format("Error: the date {0} is out of range for this model{1}The valid date range for the is {2} to {3}",
                    inCalculationOptions.StartDate.ToShortDateString(), Environment.NewLine, _Models.MinDate.ToDateTime().ToShortDateString(),
                    _Models.MaxDate.ToDateTime().ToShortDateString()));

            }

            if (inCalculationOptions.EndDate.Equals(DateTime.MinValue)) inCalculationOptions.EndDate = inCalculationOptions.StartDate;

            if (!_Models.IsDateInRange(inCalculationOptions.EndDate))
            {
                throw new GeoMagExceptionOutOfRange(string.Format("Error: the date {0} is out of range for this model{1}The valid date range for the is {2} to {3}",
                    inCalculationOptions.EndDate.ToShortDateString(), Environment.NewLine, _Models.MinDate.ToDateTime().ToShortDateString(),
                    _Models.MaxDate.ToDateTime().ToShortDateString()));
            }

            TimeSpan timespan = (inCalculationOptions.EndDate.Date - inCalculationOptions.StartDate.Date);

            double dayInc = inCalculationOptions.StepInterval < 1 ? 1 : inCalculationOptions.StepInterval;

            double dateIdx = 0;

            ResultsOfCalculation = new List<MagneticCalculations>();

            _CalculationOptions = new CalculationOptions(inCalculationOptions);

            while (dateIdx <= timespan.Days)
            {

                var internalSH = new Coefficients();

                var externalSH = new Coefficients();

                DateTime intervalDate = _CalculationOptions.StartDate.AddDays(dateIdx);

                _Models.GetIntExt(intervalDate.ToDecimal(), out internalSH, out externalSH);

                var magCalcDate = Calculator.SpotCalculation(_CalculationOptions, intervalDate, _Models, internalSH, externalSH, _Models.EarthRadius);

                if (magCalcDate != null) ResultsOfCalculation.Add(magCalcDate);

                dateIdx = ((dateIdx < timespan.Days) && ((dateIdx + dayInc) > timespan.Days))
                            ? timespan.Days
                            : dateIdx + dayInc;

            }

        }


        public void SaveResults(string fileName, bool loadAfterSave = false)
        {
            if (ResultsOfCalculation == null)
                throw new GeoMagExceptionModelNotLoaded("Error: No calculation results to save");

            if (ModelReader.IsFileLocked(fileName))
                throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' is locked by another user or application",
                    Path.GetFileName(fileName)));

            if (File.Exists(fileName))
            {

                try
                {
                    File.Delete(fileName);
                }
                catch (Exception e)
                {

                    throw new GeoMagExceptionOpenError(string.Format("Error: The file '{0}' could not be deleted: {1}",
                    System.IO.Path.GetFileName(fileName), e.ToString()));
                }

            }

            Int32 lineCount = 0;

            //Int32 lineNumColIdx = -1;

            var tabStrRight = new StringBuilder();

            //Build header

            

            tabStrRight.AppendFormat("{0}:\t{1}{2}", "Model".PadLeft(15, ' '), Path.GetFileNameWithoutExtension(_Models.Name).ToUpper(), Environment.NewLine);
            lineCount++;

            tabStrRight.AppendFormat("{0}:\t{1}{2}", "latitude".PadLeft(15, ' '), _CalculationOptions.Latitude.ToString("F7"), Environment.NewLine);
            lineCount++;

            tabStrRight.AppendFormat("{0}:\t{1}{2}", "longitude".PadLeft(15, ' '), _CalculationOptions.Longitude.ToString("F7"), Environment.NewLine);
            lineCount++;

            var elevation =  _CalculationOptions.GetElevation;

            tabStrRight.AppendFormat("{0}:\t{1}\t{2}{3}", string.Format("{0}", elevation[0]).PadLeft(15, ' '), Convert.ToDouble(elevation[1]).ToString("F4"), elevation[2], Environment.NewLine);
            lineCount++;
             
            tabStrRight.AppendFormat("{0}", Environment.NewLine);
            lineCount++;

            const Int32 padlen = 25;

            const string rowFormat = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}{8}";

            //Build Column Header

            tabStrRight.AppendFormat(rowFormat,
                "Date".PadRight(padlen, ' '), "Declination (+E/W)".PadRight(padlen, ' '), "Inclination (+D/-U)".PadRight(padlen, ' '),
                "Horizontal Intensity".PadRight(padlen, ' '), "North Comp (+N/-S)".PadRight(padlen, ' '), "East Comp (+E/-W)".PadRight(padlen, ' '),
                "Vertical Comp (+D/-U)".PadRight(padlen, ' '), "Total Field".PadRight(padlen, ' '), Environment.NewLine);
            lineCount++;

            tabStrRight.AppendFormat(rowFormat,
            "".PadRight(padlen, ' '), "deg".PadRight(padlen, ' '), "deg".PadRight(padlen, ' '),
            "nT".PadRight(padlen, ' '), "nT".PadRight(padlen, ' '), "nT".PadRight(padlen, ' '),
            "nT".PadRight(padlen, ' '), "nT".PadRight(padlen, ' '), Environment.NewLine);
            lineCount++;

            tabStrRight.AppendFormat("{0}", Environment.NewLine);
            lineCount++;

            //Build result rows

            foreach(var result in ResultsOfCalculation)
            {   
                //Date
                tabStrRight.AppendFormat(rowFormat,
                    result.Date.ToString("MM/dd/yyyy").PadRight(padlen, ' '), result.Declination.Value.ToString("F3").PadRight(padlen, ' '),
                    result.Inclination.Value.ToString("F3").PadRight(padlen, ' '), result.HorizontalIntensity.Value.ToString("F2").PadRight(padlen, ' '),
                    result.NorthComp.Value.ToString("F2").PadRight(padlen, ' '), result.EastComp.Value.ToString("F2").PadRight(padlen, ' '),
                    result.VerticalComp.Value.ToString("F2").PadRight(padlen, ' '), result.TotalField.Value.ToString("F2").PadRight(padlen, ' '),
                    Environment.NewLine);
                
                lineCount++;
            }
            
            tabStrRight.AppendFormat(rowFormat,
                "Change Per year".PadRight(padlen, ' '), ResultsOfCalculation.First().Declination.ChangePerYear.ToString("F3").PadRight(padlen, ' '),
                ResultsOfCalculation.First().Inclination.ChangePerYear.ToString("F3").PadRight(padlen, ' '), ResultsOfCalculation.First().HorizontalIntensity.ChangePerYear.ToString("F2").PadRight(padlen, ' '),
                ResultsOfCalculation.First().NorthComp.ChangePerYear.ToString("F2").PadRight(padlen, ' '), ResultsOfCalculation.First().EastComp.ChangePerYear.ToString("F2").PadRight(padlen, ' '),
                ResultsOfCalculation.First().VerticalComp.ChangePerYear.ToString("F2").PadRight(padlen, ' '), ResultsOfCalculation.First().TotalField.ChangePerYear.ToString("F2").PadRight(padlen, ' '),
                Environment.NewLine);

            // Write the stream contents to a text fle
            using (StreamWriter outFile = File.AppendText(fileName))
            {
                outFile.Write(tabStrRight.ToString());
            }
                
            


        }

    }
}
