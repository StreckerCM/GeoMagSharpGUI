using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GeoMagSharp;

namespace GeoMagSharp_UnitTests
{
    [TestClass]
    public class GeoMagSharpTests
    {
        [TestMethod]
        public void DateTimeToDecimalDate()
        {
            //Arrange
            DateTime originalDate = DateTime.Today;

            //Act
            var decDate = originalDate.ToDecimal();
            
            DateTime finalDate = decDate.ToDateTime();

            //Assert
            Assert.AreEqual(originalDate, finalDate);
        }

        [TestMethod]
        public void DecimalDateToDateTime()
        {
            //Arrange
            double originalDecDate = 2015.3;

            //Act
            var dateTime = originalDecDate.ToDateTime();

            var finalDecDate = dateTime.ToDecimal();

            //Assert
            Assert.AreEqual(originalDecDate, finalDecDate);
        }
    }
}
