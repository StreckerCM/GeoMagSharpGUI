/****************************************************************************
 * File:            CalculatorUnitTest.cs
 * Description:     Unit tests for the Calculator class
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 * Notes:           Reference values from NOAA magnetic field calculators
 *                  https://www.ngdc.noaa.gov/geomag/calculators/magcalc.shtml
 *  ****************************************************************************/

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeoMagSharp;

namespace GeoMagSharp_UnitTests
{
    [TestClass]
    public class CalculatorUnitTest
    {
        // Tolerance values for magnetic field calculations
        // Angles: 0.1 degree tolerance
        // Intensities: 100 nT tolerance (accounting for model differences)
        private const double AngleTolerance = 0.1;
        private const double IntensityTolerance = 100;

        /// <summary>
        /// Test calculation at the equator on the prime meridian
        /// Location: 0N, 0E (Gulf of Guinea)
        /// </summary>
        [TestMethod]
        public void SpotCalculation_EquatorPrimeMeridian_ReturnsValidResults()
        {
            // Arrange
            var calcOptions = new CalculationOptions
            {
                Latitude = 0.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptions.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var result = Calculator.SpotCalculation(calcOptions, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - verify we get valid results (non-zero values)
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreNotEqual(0, result.TotalField.Value, "Total field should not be zero");
            Assert.AreNotEqual(0, result.HorizontalIntensity.Value, "Horizontal intensity should not be zero at equator");

            // At equator, inclination should be close to 0 (nearly horizontal field)
            Assert.IsTrue(Math.Abs(result.Inclination.Value) < 30,
                "Inclination at equator should be relatively small (< 30 degrees)");
        }

        /// <summary>
        /// Test calculation at a northern latitude
        /// Location: 45N, 0E (France)
        /// </summary>
        [TestMethod]
        public void SpotCalculation_NorthernLatitude_ReturnsPositiveInclination()
        {
            // Arrange
            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptions.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var result = Calculator.SpotCalculation(calcOptions, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - In northern hemisphere, inclination should be positive (field pointing down)
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Inclination.Value > 0,
                "Inclination in northern hemisphere should be positive");
            Assert.IsTrue(result.Inclination.Value > 45 && result.Inclination.Value < 85,
                "Inclination at 45N should be between 45 and 85 degrees");
        }

        /// <summary>
        /// Test calculation at a southern latitude
        /// Location: 45S, 0E (South Atlantic)
        /// </summary>
        [TestMethod]
        public void SpotCalculation_SouthernLatitude_ReturnsNegativeInclination()
        {
            // Arrange
            var calcOptions = new CalculationOptions
            {
                Latitude = -45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptions.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var result = Calculator.SpotCalculation(calcOptions, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - In southern hemisphere, inclination should be negative (field pointing up)
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Inclination.Value < 0,
                "Inclination in southern hemisphere should be negative");
            Assert.IsTrue(result.Inclination.Value > -85 && result.Inclination.Value < -45,
                "Inclination at 45S should be between -85 and -45 degrees");
        }

        /// <summary>
        /// Test that calculation at altitude produces different results than sea level
        /// </summary>
        [TestMethod]
        public void SpotCalculation_AtAltitude_ReducesFieldIntensity()
        {
            // Arrange
            var calcOptionsSeaLevel = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptionsSeaLevel.SetElevation(0, Distance.Unit.meter, true);

            var calcOptionsAltitude = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptionsAltitude.SetElevation(100, Distance.Unit.kilometer, true); // 100 km altitude

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptionsSeaLevel.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var resultSeaLevel = Calculator.SpotCalculation(calcOptionsSeaLevel, dateOfCalc, magModels, internalSH, externalSH);
            var resultAltitude = Calculator.SpotCalculation(calcOptionsAltitude, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - Field intensity decreases with altitude
            Assert.IsTrue(resultAltitude.TotalField.Value < resultSeaLevel.TotalField.Value,
                "Total field intensity should decrease with altitude");
        }

        /// <summary>
        /// Test secular variation calculation
        /// </summary>
        [TestMethod]
        public void SpotCalculation_WithSecularVariation_ReturnsChangePerYear()
        {
            // Arrange
            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = true
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptions.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var result = Calculator.SpotCalculation(calcOptions, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - Secular variation values should be calculated
            Assert.IsNotNull(result);
            // The change per year values should be non-zero when secular variation is enabled
            // Note: At least one component should have measurable change
            bool hasSecularVariation =
                result.Declination.ChangePerYear != 0 ||
                result.Inclination.ChangePerYear != 0 ||
                result.TotalField.ChangePerYear != 0;

            Assert.IsTrue(hasSecularVariation,
                "Secular variation should produce non-zero change values");
        }

        /// <summary>
        /// Test calculation returns consistent date
        /// </summary>
        [TestMethod]
        public void SpotCalculation_ReturnsCorrectDate()
        {
            // Arrange
            var expectedDate = new DateTime(2015, 7, 1);
            var calcOptions = new CalculationOptions
            {
                Latitude = 0.0,
                Longitude = 0.0,
                StartDate = expectedDate,
                SecularVariation = false
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(expectedDate.ToDecimal(), out internalSH, out externalSH);

            // Act
            var result = Calculator.SpotCalculation(calcOptions, expectedDate, magModels, internalSH, externalSH);

            // Assert
            Assert.AreEqual(expectedDate, result.Date, "Result date should match input date");
        }

        /// <summary>
        /// Test that declination varies with longitude
        /// </summary>
        [TestMethod]
        public void SpotCalculation_DifferentLongitudes_ProducesDifferentDeclinations()
        {
            // Arrange
            var calcOptionsWest = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = -90.0, // Western hemisphere
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptionsWest.SetElevation(0, Distance.Unit.meter, true);

            var calcOptionsEast = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 90.0, // Eastern hemisphere
                StartDate = new DateTime(2015, 7, 1),
                SecularVariation = false
            };
            calcOptionsEast.SetElevation(0, Distance.Unit.meter, true);

            var magModels = CreateTestModelSet();
            var dateOfCalc = calcOptionsWest.StartDate;

            var internalSH = new Coefficients();
            var externalSH = new Coefficients();
            magModels.GetIntExt(dateOfCalc.ToDecimal(), out internalSH, out externalSH);

            // Act
            var resultWest = Calculator.SpotCalculation(calcOptionsWest, dateOfCalc, magModels, internalSH, externalSH);
            var resultEast = Calculator.SpotCalculation(calcOptionsEast, dateOfCalc, magModels, internalSH, externalSH);

            // Assert - Declination should be different at different longitudes
            Assert.AreNotEqual(resultWest.Declination.Value, resultEast.Declination.Value,
                "Declination should differ between east and west longitudes");
        }

        /// <summary>
        /// Test CoLatitude property calculation
        /// </summary>
        [TestMethod]
        public void CalculationOptions_CoLatitude_CalculatesCorrectly()
        {
            // Arrange & Act
            var options1 = new CalculationOptions { Latitude = 0 };
            var options2 = new CalculationOptions { Latitude = 45 };
            var options3 = new CalculationOptions { Latitude = 90 };
            var options4 = new CalculationOptions { Latitude = -45 };

            // Assert
            Assert.AreEqual(90, options1.CoLatitude, "CoLatitude at equator should be 90");
            Assert.AreEqual(45, options2.CoLatitude, "CoLatitude at 45N should be 45");
            Assert.AreEqual(0, options3.CoLatitude, "CoLatitude at North Pole should be 0");
            Assert.AreEqual(135, options4.CoLatitude, "CoLatitude at 45S should be 135");
        }

        /// <summary>
        /// Test AltitudeInKm conversion from meters
        /// </summary>
        [TestMethod]
        public void CalculationOptions_AltitudeInKm_ConvertsFromMeters()
        {
            // Arrange
            var options = new CalculationOptions();
            options.SetElevation(1000, Distance.Unit.meter, true);

            // Act
            var altitudeKm = options.AltitudeInKm;

            // Assert
            Assert.AreEqual(1.0, altitudeKm, 0.001, "1000 meters should equal 1 km");
        }

        /// <summary>
        /// Test AltitudeInKm conversion from feet
        /// </summary>
        [TestMethod]
        public void CalculationOptions_AltitudeInKm_ConvertsFromFeet()
        {
            // Arrange
            var options = new CalculationOptions();
            options.SetElevation(32808.4, Distance.Unit.foot, true); // Approximately 10 km

            // Act
            var altitudeKm = options.AltitudeInKm;

            // Assert
            Assert.AreEqual(10.0, altitudeKm, 0.01, "32808.4 feet should be approximately 10 km");
        }

        /// <summary>
        /// Test depth (negative altitude) handling
        /// </summary>
        [TestMethod]
        public void CalculationOptions_Depth_ReturnsNegativeAltitude()
        {
            // Arrange
            var options = new CalculationOptions();
            options.SetElevation(1000, Distance.Unit.meter, false); // Depth, not altitude

            // Act
            var altitudeKm = options.AltitudeInKm;

            // Assert
            Assert.AreEqual(-1.0, altitudeKm, 0.001, "1000m depth should equal -1 km altitude");
        }

        /// <summary>
        /// Helper method to create a test MagneticModelSet by reading a COF file
        /// </summary>
        private MagneticModelSet CreateTestModelSet()
        {
            // Try to find the WMM coefficient file in various locations
            var possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "WMM2015.COF"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "coefficient", "WMM2015.COF"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "GeoMagGUI", "coefficient", "WMM2015.COF"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "TestData", "WMM2015.COF"),
                @"C:\GitHub\GeoMagSharpGUI\GeoMagGUI\coefficient\WMM2015.COF"
            };

            string modelPath = null;
            foreach (var path in possiblePaths)
            {
                var fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                {
                    modelPath = fullPath;
                    break;
                }
            }

            if (modelPath == null)
            {
                // If no COF file found, create a minimal synthetic model for testing
                return CreateSyntheticModelSet();
            }

            return ModelReader.Read(modelPath);
        }

        /// <summary>
        /// Creates a minimal synthetic model for testing when COF files are not available
        /// </summary>
        private MagneticModelSet CreateSyntheticModelSet()
        {
            var modelSet = new MagneticModelSet();

            // Add main field model (M type)
            var mainModel = new MagneticModel
            {
                Type = "M",
                Year = 2015.0
            };

            // Add simplified spherical harmonic coefficients
            // These are approximate values based on WMM2015 for basic testing
            // Degree 1 coefficients (dipole terms - most important)
            mainModel.SharmCoeff.Add(-29438.5); // g10
            mainModel.SharmCoeff.Add(-1501.1);  // g11
            mainModel.SharmCoeff.Add(4796.2);   // h11

            // Degree 2 coefficients
            mainModel.SharmCoeff.Add(-2445.3);  // g20
            mainModel.SharmCoeff.Add(3012.5);   // g21
            mainModel.SharmCoeff.Add(-2845.6);  // h21
            mainModel.SharmCoeff.Add(1676.6);   // g22
            mainModel.SharmCoeff.Add(-642.0);   // h22

            modelSet.AddModel(mainModel);

            // Add secular variation model (S type)
            var svModel = new MagneticModel
            {
                Type = "S",
                Year = 2015.0
            };

            // Secular variation coefficients (change per year in nT/year)
            svModel.SharmCoeff.Add(10.7);  // g10 SV
            svModel.SharmCoeff.Add(17.9);  // g11 SV
            svModel.SharmCoeff.Add(-26.8); // h11 SV
            svModel.SharmCoeff.Add(-8.6);  // g20 SV
            svModel.SharmCoeff.Add(-3.3);  // g21 SV
            svModel.SharmCoeff.Add(-27.1); // h21 SV
            svModel.SharmCoeff.Add(2.4);   // g22 SV
            svModel.SharmCoeff.Add(-13.3); // h22 SV

            modelSet.AddModel(svModel);

            // Set date range
            modelSet.MinDate = 2015.0;
            modelSet.MaxDate = 2020.0;

            return modelSet;
        }
    }
}
