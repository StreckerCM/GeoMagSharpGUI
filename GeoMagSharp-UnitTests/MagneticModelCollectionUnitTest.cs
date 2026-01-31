using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeoMagSharp;

namespace GeoMagSharp_UnitTests
{
    [TestClass]
    public class MagneticModelCollectionUnitTest
    {
        #region FindByFileName Tests

        [TestMethod]
        public void FindByFileName_ExistingFile_ReturnsModel()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");
            collection.Add(model);

            // Act
            var result = collection.FindByFileName("WMM2025.COF");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.ID, result.ID);
        }

        [TestMethod]
        public void FindByFileName_CaseInsensitive_ReturnsModel()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");
            collection.Add(model);

            // Act
            var result = collection.FindByFileName("wmm2025.cof");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.ID, result.ID);
        }

        [TestMethod]
        public void FindByFileName_NonExistingFile_ReturnsNull()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");
            collection.Add(model);

            // Act
            var result = collection.FindByFileName("WMM2020.COF");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindByFileName_NullFileName_ReturnsNull()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");
            collection.Add(model);

            // Act
            var result = collection.FindByFileName(null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindByFileName_EmptyFileName_ReturnsNull()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");
            collection.Add(model);

            // Act
            var result = collection.FindByFileName("");

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region AddOrReplace Tests

        [TestMethod]
        public void AddOrReplace_NewModel_AddsToCollection()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model = CreateTestModel("WMM2025.COF");

            // Act
            bool replaced = collection.AddOrReplace(model);

            // Assert
            Assert.IsFalse(replaced);
            Assert.AreEqual(1, collection.Count());
            Assert.IsNotNull(collection.FindByFileName("WMM2025.COF"));
        }

        [TestMethod]
        public void AddOrReplace_ExistingModel_ReplacesInCollection()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var originalModel = CreateTestModel("WMM2025.COF");
            originalModel.Name = "Original";
            collection.Add(originalModel);

            var newModel = CreateTestModel("WMM2025.COF");
            newModel.Name = "Replacement";

            // Act
            bool replaced = collection.AddOrReplace(newModel);

            // Assert
            Assert.IsTrue(replaced);
            Assert.AreEqual(1, collection.Count());
            var foundModel = collection.FindByFileName("WMM2025.COF");
            Assert.IsNotNull(foundModel);
            Assert.AreEqual("Replacement", foundModel.Name);
        }

        [TestMethod]
        public void AddOrReplace_DifferentFilenames_AddsBothModels()
        {
            // Arrange
            var collection = new MagneticModelCollection();
            var model2020 = CreateTestModel("WMM2020.COF");
            var model2025 = CreateTestModel("WMM2025.COF");
            collection.Add(model2020);

            // Act
            bool replaced = collection.AddOrReplace(model2025);

            // Assert
            Assert.IsFalse(replaced);
            Assert.AreEqual(2, collection.Count());
        }

        [TestMethod]
        public void AddOrReplace_NullModel_AddsWithoutError()
        {
            // Arrange
            var collection = new MagneticModelCollection();

            // Act
            bool replaced = collection.AddOrReplace(null);

            // Assert
            Assert.IsFalse(replaced);
            Assert.AreEqual(1, collection.Count()); // Null is added (existing behavior)
        }

        #endregion

        #region Helper Methods

        private MagneticModelSet CreateTestModel(string fileName)
        {
            var model = new MagneticModelSet();
            model.FileNames.Add(fileName);
            model.Name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            return model;
        }

        #endregion
    }
}
