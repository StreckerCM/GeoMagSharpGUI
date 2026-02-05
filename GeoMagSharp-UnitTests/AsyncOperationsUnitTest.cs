/****************************************************************************
 * File:            AsyncOperationsUnitTest.cs
 * Description:     Unit tests for async operations (ReadAsync,
 *                  MagneticCalculationsAsync, SaveResultsAsync,
 *                  LoadAsync, SaveAsync)
 * Author:          Christopher Strecker
 * Website:         https://github.com/StreckerCM/GeoMagSharpGUI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeoMagSharp;

namespace GeoMagSharp_UnitTests
{
    [TestClass]
    public class AsyncOperationsUnitTest
    {
        private static string TestDataPath;

        [ClassInitialize]
        public static void ClassInit(TestContext _)
        {
            var possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "TestData"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "GeoMagSharp-UnitTests", "TestData"),
                @"C:\GitHub\GeoMagSharpGUI\GeoMagSharp-UnitTests\TestData"
            };

            foreach (var path in possiblePaths)
            {
                var fullPath = Path.GetFullPath(path);
                if (Directory.Exists(fullPath))
                {
                    TestDataPath = fullPath;
                    break;
                }
            }

            if (string.IsNullOrEmpty(TestDataPath))
            {
                throw new DirectoryNotFoundException("Could not find TestData directory");
            }
        }

        #region ModelReader.ReadAsync Tests

        [TestMethod]
        public async Task ReadAsync_ValidCofFile_ReturnsModel()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            // Act
            var modelSet = await ModelReader.ReadAsync(filePath);

            // Assert
            Assert.IsNotNull(modelSet);
            Assert.AreEqual(knownModels.WMM, modelSet.Type, "Model type should be WMM");
            Assert.AreEqual(2025.0, modelSet.MinDate, 0.01, "Model year should be 2025.0");
            Assert.IsTrue(modelSet.NumberOfModels >= 2, "Should have at least M and S models");
        }

        [TestMethod]
        public async Task ReadAsync_MatchesSyncRead()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            // Act
            var syncResult = ModelReader.Read(filePath);
            var asyncResult = await ModelReader.ReadAsync(filePath);

            // Assert - async should produce same result as sync
            Assert.AreEqual(syncResult.Type, asyncResult.Type, "Type should match");
            Assert.AreEqual(syncResult.MinDate, asyncResult.MinDate, 0.001, "MinDate should match");
            Assert.AreEqual(syncResult.MaxDate, asyncResult.MaxDate, 0.001, "MaxDate should match");
            Assert.AreEqual(syncResult.NumberOfModels, asyncResult.NumberOfModels, "NumberOfModels should match");

            // Compare coefficient counts
            var syncModels = syncResult.GetModels;
            var asyncModels = asyncResult.GetModels;
            Assert.AreEqual(syncModels.Count, asyncModels.Count, "Model count should match");

            for (int i = 0; i < syncModels.Count; i++)
            {
                Assert.AreEqual(syncModels[i].SharmCoeff.Count, asyncModels[i].SharmCoeff.Count,
                    string.Format("Coefficient count should match for model {0}", i));
            }
        }

        [TestMethod]
        public async Task ReadAsync_CancelledToken_ThrowsOperationCancelled()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var cts = new CancellationTokenSource();
            cts.Cancel(); // Pre-cancel

            // Act & Assert
            await AssertThrowsAsync<OperationCanceledException>(async () =>
                await ModelReader.ReadAsync(filePath, null, cts.Token));
        }

        [TestMethod]
        public async Task ReadAsync_InvalidFile_ThrowsException()
        {
            // Act & Assert
            await AssertThrowsAsync<ArgumentNullException>(async () =>
                await ModelReader.ReadAsync(null));

            await AssertThrowsAsync<ArgumentNullException>(async () =>
                await ModelReader.ReadAsync(string.Empty));
        }

        [TestMethod]
        public async Task ReadAsync_NonExistentFile_ThrowsFileNotFoundException()
        {
            // Act & Assert
            await AssertThrowsAsync<GeoMagExceptionFileNotFound>(async () =>
                await ModelReader.ReadAsync(@"C:\NonExistent\Path\File.COF"));
        }

        [TestMethod]
        public async Task ReadAsync_ReportsProgress()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var progressReports = new List<CalculationProgressInfo>();
            var progress = new SynchronousProgress(progressReports);

            // Act
            var modelSet = await ModelReader.ReadAsync(filePath, progress);

            // Assert
            Assert.IsNotNull(modelSet);
            Assert.AreEqual(2, progressReports.Count, "Should have received 2 progress reports (start and complete)");
            Assert.AreEqual(1, progressReports[0].CurrentStep, "First report should be step 1");
            Assert.AreEqual(2, progressReports[1].CurrentStep, "Second report should be step 2");
            Assert.AreEqual("Model loaded successfully", progressReports[1].StatusMessage);
        }

        [TestMethod]
        public async Task ReadAsync_ValidDatFile_ReturnsModel()
        {
            // Arrange - Use a DAT file if available in TestData
            string filePath = Path.Combine(TestDataPath, "IGRF13.DAT");
            if (!File.Exists(filePath))
            {
                var datFiles = Directory.GetFiles(TestDataPath, "*.DAT");
                if (datFiles.Length == 0)
                    Assert.Inconclusive("No DAT files found in TestData folder");
                filePath = datFiles[0];
            }

            // Act
            var modelSet = await ModelReader.ReadAsync(filePath);

            // Assert
            Assert.IsNotNull(modelSet);
            Assert.IsTrue(modelSet.NumberOfModels > 0, "Should have at least one model");
        }

        #endregion

        #region GeoMag.MagneticCalculationsAsync Tests

        [TestMethod]
        public async Task MagneticCalculationsAsync_ValidInput_ReturnsResults()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Act
            await geoMag.MagneticCalculationsAsync(calcOptions);

            // Assert
            Assert.IsNotNull(geoMag.ResultsOfCalculation);
            Assert.IsTrue(geoMag.ResultsOfCalculation.Count > 0, "Should have calculation results");

            var result = geoMag.ResultsOfCalculation[0];
            Assert.AreNotEqual(0, result.TotalField.Value, "Total field should not be zero");
        }

        [TestMethod]
        public async Task MagneticCalculationsAsync_MatchesSyncResults()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Sync calculation
            var syncGeoMag = new GeoMag();
            syncGeoMag.LoadModel(filePath);
            syncGeoMag.MagneticCalculations(new CalculationOptions(calcOptions));

            // Async calculation
            var asyncGeoMag = new GeoMag();
            asyncGeoMag.LoadModel(filePath);
            await asyncGeoMag.MagneticCalculationsAsync(new CalculationOptions(calcOptions));

            // Assert - results should match
            Assert.AreEqual(syncGeoMag.ResultsOfCalculation.Count, asyncGeoMag.ResultsOfCalculation.Count,
                "Result count should match");

            for (int i = 0; i < syncGeoMag.ResultsOfCalculation.Count; i++)
            {
                var syncResult = syncGeoMag.ResultsOfCalculation[i];
                var asyncResult = asyncGeoMag.ResultsOfCalculation[i];

                Assert.AreEqual(syncResult.Declination.Value, asyncResult.Declination.Value, 0.001,
                    string.Format("Declination should match at step {0}", i));
                Assert.AreEqual(syncResult.Inclination.Value, asyncResult.Inclination.Value, 0.001,
                    string.Format("Inclination should match at step {0}", i));
                Assert.AreEqual(syncResult.TotalField.Value, asyncResult.TotalField.Value, 0.1,
                    string.Format("TotalField should match at step {0}", i));
            }
        }

        [TestMethod]
        public async Task MagneticCalculationsAsync_CancelledToken_ThrowsOperationCancelled()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
                StepInterval = 1,
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var cts = new CancellationTokenSource();
            cts.Cancel(); // Pre-cancel

            // Act & Assert
            await AssertThrowsAsync<OperationCanceledException>(async () =>
                await geoMag.MagneticCalculationsAsync(calcOptions, null, cts.Token));
        }

        [TestMethod]
        public async Task MagneticCalculationsAsync_ReportsProgress()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 3, 1),
                StepInterval = 30,
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            var progressReports = new List<CalculationProgressInfo>();
            var progress = new SynchronousProgress(progressReports);

            // Act
            await geoMag.MagneticCalculationsAsync(calcOptions, progress);

            // Assert
            Assert.IsNotNull(geoMag.ResultsOfCalculation);
            Assert.IsTrue(progressReports.Count >= 2, "Should have received at least 2 progress reports");

            // Verify progress steps are monotonically non-decreasing
            for (int i = 1; i < progressReports.Count; i++)
            {
                Assert.IsTrue(progressReports[i].CurrentStep >= progressReports[i - 1].CurrentStep,
                    string.Format("Progress steps should be non-decreasing: step {0} vs {1}",
                        progressReports[i - 1].CurrentStep, progressReports[i].CurrentStep));
            }

            // Last progress report should indicate completion
            var lastReport = progressReports[progressReports.Count - 1];
            Assert.AreEqual("Calculation complete", lastReport.StatusMessage,
                "Last progress report should say 'Calculation complete'");
        }

        [TestMethod]
        public async Task MagneticCalculationsAsync_DateRange_ReturnsMultipleResults()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 4, 1),
                StepInterval = 30,
                SecularVariation = false,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Act
            await geoMag.MagneticCalculationsAsync(calcOptions);

            // Assert
            Assert.IsNotNull(geoMag.ResultsOfCalculation);
            Assert.IsTrue(geoMag.ResultsOfCalculation.Count > 1,
                "Date range calculation should return multiple results");
        }

        #endregion

        #region GeoMag.SaveResultsAsync Tests

        [TestMethod]
        public async Task SaveResultsAsync_ValidResults_SavesFile()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            await geoMag.MagneticCalculationsAsync(calcOptions);

            string outputFile = Path.Combine(Path.GetTempPath(), "async_test_output.txt");

            try
            {
                // Act
                await geoMag.SaveResultsAsync(outputFile);

                // Assert
                Assert.IsTrue(File.Exists(outputFile), "Output file should exist");
                var content = File.ReadAllText(outputFile);
                Assert.IsTrue(content.Length > 0, "Output file should have content");
            }
            finally
            {
                if (File.Exists(outputFile))
                    File.Delete(outputFile);
            }
        }

        [TestMethod]
        public async Task SaveResultsAsync_NoResults_ThrowsException()
        {
            // Arrange
            var geoMag = new GeoMag();
            string outputFile = Path.Combine(Path.GetTempPath(), "async_test_no_results.txt");

            // Act & Assert
            await AssertThrowsAsync<GeoMagExceptionModelNotLoaded>(async () =>
                await geoMag.SaveResultsAsync(outputFile));
        }

        #endregion

        #region MagneticModelCollection LoadAsync/SaveAsync Tests

        [TestMethod]
        public async Task LoadAsync_SaveAsync_RoundTrip()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var model = ModelReader.Read(filePath);
            var collection = new MagneticModelCollection();
            collection.Add(model);

            string tempFile = Path.Combine(Path.GetTempPath(), "async_collection_test.json");

            try
            {
                // Act - Save
                bool saveResult = await collection.SaveAsync(tempFile);
                Assert.IsTrue(saveResult, "Save should succeed");
                Assert.IsTrue(File.Exists(tempFile), "Saved file should exist");

                // Act - Load
                var loadedCollection = await MagneticModelCollection.LoadAsync(tempFile);

                // Assert
                Assert.IsNotNull(loadedCollection);
                Assert.AreEqual(1, loadedCollection.TList.Count, "Should have one model");

                var loadedModel = loadedCollection.TList[0];
                Assert.AreEqual(model.Name, loadedModel.Name, "Model name should match");
                Assert.AreEqual(model.MinDate, loadedModel.MinDate, 0.001, "MinDate should match");
                Assert.AreEqual(model.MaxDate, loadedModel.MaxDate, 0.001, "MaxDate should match");
                Assert.AreEqual(model.Type, loadedModel.Type, "Type should match");
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public async Task LoadAsync_NonExistentFile_ReturnsEmptyCollection()
        {
            // Act
            var collection = await MagneticModelCollection.LoadAsync(@"C:\NonExistent\Path\File.json");

            // Assert
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.TList.Count, "Should return empty collection");
        }

        [TestMethod]
        public async Task SaveAsync_CancelledToken_ThrowsOperationCancelled()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            string tempFile = Path.Combine(Path.GetTempPath(), "async_cancel_test.json");
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Pre-cancel

            // Act & Assert
            await AssertThrowsAsync<OperationCanceledException>(async () =>
                await collection.SaveAsync(tempFile, cts.Token));
        }

        [TestMethod]
        public async Task LoadAsync_EmptyFilename_ReturnsEmptyCollection()
        {
            // Act
            var collection = await MagneticModelCollection.LoadAsync(string.Empty);

            // Assert
            Assert.IsNotNull(collection);
            Assert.AreEqual(0, collection.TList.Count);
        }

        #endregion

        #region CalculationProgressInfo Tests

        [TestMethod]
        public void CalculationProgressInfo_PercentComplete_CalculatesCorrectly()
        {
            // Arrange & Act
            var info = new CalculationProgressInfo
            {
                CurrentStep = 5,
                TotalSteps = 10,
                StatusMessage = "Testing"
            };

            // Assert
            Assert.AreEqual(50.0, info.PercentComplete, 0.001, "50% complete");
        }

        [TestMethod]
        public void CalculationProgressInfo_ZeroTotalSteps_ReturnsZeroPercent()
        {
            // Arrange & Act
            var info = new CalculationProgressInfo
            {
                CurrentStep = 5,
                TotalSteps = 0
            };

            // Assert
            Assert.AreEqual(0.0, info.PercentComplete, 0.001, "Should return 0 when TotalSteps is 0");
        }

        #endregion

        #region Additional Edge Case Tests

        [TestMethod]
        public async Task MagneticCalculationsAsync_NoModelLoaded_ThrowsException()
        {
            // Arrange
            var geoMag = new GeoMag();
            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Act & Assert
            await AssertThrowsAsync<GeoMagExceptionModelNotLoaded>(async () =>
                await geoMag.MagneticCalculationsAsync(calcOptions));
        }

        [TestMethod]
        public async Task MagneticCalculationsAsync_DateOutOfRange_ThrowsException()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(1900, 1, 1), // Well outside WMM2025 range
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Act & Assert
            await AssertThrowsAsync<GeoMagExceptionOutOfRange>(async () =>
                await geoMag.MagneticCalculationsAsync(calcOptions));
        }

        [TestMethod]
        public async Task ReadAsync_UnsupportedExtension_ThrowsModelNotLoaded()
        {
            // Arrange - Create a temp file with unsupported extension
            string tempFile = Path.Combine(Path.GetTempPath(), "test_async.xyz");
            try
            {
                File.WriteAllText(tempFile, "test content");

                // Act & Assert
                await AssertThrowsAsync<GeoMagExceptionModelNotLoaded>(async () =>
                    await ModelReader.ReadAsync(tempFile));
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public async Task SaveResultsAsync_CancelledToken_ThrowsOperationCancelled()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var geoMag = new GeoMag();
            geoMag.LoadModel(filePath);

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            await geoMag.MagneticCalculationsAsync(calcOptions);

            var cts = new CancellationTokenSource();
            cts.Cancel(); // Pre-cancel

            string outputFile = Path.Combine(Path.GetTempPath(), "async_cancel_save_test.txt");

            // Act & Assert
            await AssertThrowsAsync<OperationCanceledException>(async () =>
                await geoMag.SaveResultsAsync(outputFile, false, cts.Token));
        }

        [TestMethod]
        public async Task LoadAsync_CancelledToken_ThrowsOperationCancelled()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Pre-cancel

            string tempFile = Path.Combine(Path.GetTempPath(), "async_cancel_load_test.json");
            try
            {
                File.WriteAllText(tempFile, "[]");

                // Act & Assert
                await AssertThrowsAsync<OperationCanceledException>(async () =>
                    await MagneticModelCollection.LoadAsync(tempFile, cts.Token));
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [TestMethod]
        public async Task SaveResultsAsync_MatchesSyncSave()
        {
            // Arrange
            string filePath = Path.Combine(TestDataPath, "WMM2025.COF");
            if (!File.Exists(filePath))
                Assert.Inconclusive("WMM2025.COF not found in TestData folder");

            var calcOptions = new CalculationOptions
            {
                Latitude = 45.0,
                Longitude = 0.0,
                StartDate = new DateTime(2025, 7, 1),
                SecularVariation = true,
                CalculationMethod = Algorithm.BGS
            };
            calcOptions.SetElevation(0, Distance.Unit.meter, true);

            // Sync save
            var syncGeoMag = new GeoMag();
            syncGeoMag.LoadModel(filePath);
            syncGeoMag.MagneticCalculations(new CalculationOptions(calcOptions));

            string syncFile = Path.Combine(Path.GetTempPath(), "sync_save_test.txt");
            syncGeoMag.SaveResults(syncFile);

            // Async save
            var asyncGeoMag = new GeoMag();
            asyncGeoMag.LoadModel(filePath);
            await asyncGeoMag.MagneticCalculationsAsync(new CalculationOptions(calcOptions));

            string asyncFile = Path.Combine(Path.GetTempPath(), "async_save_test.txt");
            await asyncGeoMag.SaveResultsAsync(asyncFile);

            try
            {
                // Assert - file contents should be identical
                string syncContent = File.ReadAllText(syncFile);
                string asyncContent = File.ReadAllText(asyncFile);
                Assert.AreEqual(syncContent, asyncContent, "Sync and async save should produce identical output");
            }
            finally
            {
                if (File.Exists(syncFile)) File.Delete(syncFile);
                if (File.Exists(asyncFile)) File.Delete(asyncFile);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper to assert that an async operation throws the expected exception type.
        /// MSTest v1 [ExpectedException] does not work with async Task methods reliably.
        /// </summary>
        private static async Task AssertThrowsAsync<TException>(Func<Task> action) where TException : Exception
        {
            try
            {
                await action();
                Assert.Fail("Expected {0} but no exception was thrown", typeof(TException).Name);
            }
            catch (TException)
            {
                // Expected
            }
            catch (AggregateException ex) when (ex.InnerException is TException)
            {
                // Also acceptable - Task.Run can wrap in AggregateException
            }
        }

        /// <summary>
        /// Synchronous IProgress implementation that captures reports immediately
        /// without posting to SynchronizationContext. Avoids race conditions in tests.
        /// </summary>
        private class SynchronousProgress : IProgress<CalculationProgressInfo>
        {
            private readonly List<CalculationProgressInfo> _reports;

            public SynchronousProgress(List<CalculationProgressInfo> reports)
            {
                _reports = reports;
            }

            public void Report(CalculationProgressInfo value)
            {
                _reports.Add(value);
            }
        }

        #endregion
    }
}
