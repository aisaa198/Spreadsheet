using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Spreadsheet.Test
{
    [TestClass]
    public class DownloadingDataTests
    {
        [TestMethod]
        public void GetNumbers_ValidData_ValidResult()
        {
            var calculatingMock = new Mock<ICalculating>();
            var downloading = new DownloadingData(calculatingMock.Object);
            var input = "1|2,3|13";
            List<Data> table = new List<Data>();
            calculatingMock.Setup(x => x.CalculateOperation("1", table)).Returns("1");
            calculatingMock.Setup(x => x.CalculateOperation("2,3", table)).Returns("2,3");
            calculatingMock.Setup(x => x.CalculateOperation("13", table)).Returns("13");

            var result = downloading.GetNumbers(input, table);

            Assert.AreEqual("A1", result[0].Address);
            Assert.AreEqual(1, result[0].Value);
            Assert.AreEqual("A2", result[1].Address);
            Assert.AreEqual(2,3, result[1].Value);
            Assert.AreEqual("A3", result[2].Address);
            Assert.AreEqual(13, result[2].Value);
        }

        [TestMethod]
        public void GetNumbers_ValidData_ValidTableAddress()
        {
            var calculatingMock = new Mock<ICalculating>();
            var downloading = new DownloadingData(calculatingMock.Object);
            var input1 = "1";
            var input2 = "2";
            List<Data> table = new List<Data>();
            calculatingMock.Setup(x => x.CalculateOperation("1", table)).Returns("1");
            calculatingMock.Setup(x => x.CalculateOperation("2", table)).Returns("2");
            
            var result1 = downloading.GetNumbers(input1, table);
            var result2 = downloading.GetNumbers(input2, table);
            
            Assert.AreEqual("A1", result1[0].Address);
            Assert.AreEqual(1, result1[0].Value);
            Assert.AreEqual("B1", result2[0].Address);
            Assert.AreEqual(2, result2[0].Value);
        }

        [TestMethod]
        public void GetNumbers_LastRow_ValidResult()
        {
            var calculatingMock = new Mock<ICalculating>();
            var downloading = new DownloadingData(calculatingMock.Object);
            var input = "1;";
            List<Data> table = new List<Data>();
            calculatingMock.Setup(x => x.CalculateOperation("1", table)).Returns("1");

            var result = downloading.GetNumbers(input, table);

            Assert.AreEqual("A1", result[0].Address);
            Assert.AreEqual(1, result[0].Value);
        }

        [TestMethod]
        public void GetNumbers_EmptyInput_Null()
        {
            var calculatingMock = new Mock<ICalculating>();
            var downloading = new DownloadingData(calculatingMock.Object);
            var input = "";
            List<Data> table = new List<Data>();
            calculatingMock.Setup(x => x.CalculateOperation("", table)).Returns("");

            var result = downloading.GetNumbers(input, table);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetNumbers_InvalidInput_Null()
        {
            var calculatingMock = new Mock<ICalculating>();
            var downloading = new DownloadingData(calculatingMock.Object);
            var input = "ola";
            List<Data> table = new List<Data>();
            calculatingMock.Setup(x => x.CalculateOperation("ola", table)).Returns("ola");

            var result = downloading.GetNumbers(input, table);

            Assert.IsNull(result);
        }
    }
}
