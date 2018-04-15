using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Spreadsheet.Test
{
    [TestClass]
    public class CalculatingTests
    {
        [TestMethod]
        public void TransformCellAddressToNumber_ValidData_ValidResult()
        {
            var calculating = new Calculating();
            var input = "A1 + B121 + 3";
            List<Data> table = new List<Data>
            {
                new Data
                {
                    Address = "A1",
                    Value = 1
                },
                new Data
                {
                    Address = "B121",
                    Value = 2
                }
            };

            var result = calculating.TransformCellAddressToNumber(input, table);

            Assert.AreEqual("1 + 2 + 3", result);
        }

        [TestMethod]
        public void TransformCellAddressToNumber_EmptyInput_EmptyString()
        {
            var calculating = new Calculating();
            var input = "";
            List<Data> table = new List<Data>();

            var result = calculating.TransformCellAddressToNumber(input, table);

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void Calculate_ValidDataWithSimpleOperations_ValidResult()
        {
            var calculating = new Calculating();
            var input = new List<string> { "10", "2", "4", "/", "+", "3", "3", "*", "-" };

            var result = calculating.Calculate(input);

            Assert.AreEqual("1,5", result);
        }

        [TestMethod]
        public void Calculate_ValidDataNonIntegerNumbers_ValidResult()
        {
            var calculating = new Calculating();
            var input = new List<string> { "2,3", "1,223", "+" };

            var result = calculating.Calculate(input);

            Assert.AreEqual("3,523", result);
        }

        [TestMethod]
        public void Calculate_Null_Null()
        {
            var calculating = new Calculating();
            List<string> input = null;

            var result = calculating.Calculate(input);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Calculate_ListWithEmptyString_Null()
        {
            var calculating = new Calculating();
            var input = new List<string> { "" };

            var result = calculating.Calculate(input);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Calculate_EmptyList_Null()
        {
            var calculating = new Calculating();
            var input = new List<string>();

            var result = calculating.Calculate(input);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void Calculate_InvalidData_Null()
        {
            var calculating = new Calculating();
            var input = new List<string> { "1", "+", "2" };
            List<Data> table = new List<Data>();

            var result = calculating.Calculate(input);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Calculate_InvalidDataWithLetters_Null()
        {
            var calculating = new Calculating();
            var input = new List<string> { "ola" };
            List<Data> table = new List<Data>();

            var result = calculating.Calculate(input);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TransformToPostfixNotation_ValidData_ValidResult()
        {
            var calculating = new Calculating();
            var input = "2+3*(4+6)-2/4";
            var expectedList = new List<string> { "2", "3", "4", "6", "+", "*", "+", "2", "4", "/", "-" };

            var result = calculating.TransformToPostfixNotation(input);

            Assert.AreEqual(expectedList[0], result[0]);
            Assert.AreEqual(expectedList[1], result[1]);
            Assert.AreEqual(expectedList[2], result[2]);
            Assert.AreEqual(expectedList[3], result[3]);
            Assert.AreEqual(expectedList[4], result[4]);
            Assert.AreEqual(expectedList[5], result[5]);
            Assert.AreEqual(expectedList[6], result[6]);
            Assert.AreEqual(expectedList[7], result[7]);
            Assert.AreEqual(expectedList[8], result[8]);
            Assert.AreEqual(expectedList[9], result[9]);
            Assert.AreEqual(expectedList[10], result[10]);
        }

        [TestMethod]
        public void TransformToPostfixNotation_InvalidDataWithForbiddenCharacters_Null()
        {
            var calculating = new Calculating();
            var input = "ala - ola";

            var result = calculating.TransformToPostfixNotation(input);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TransformToPostfixNotation_InvalidDataWithIncorrectSyntax_Null()
        {
            var calculating = new Calculating();
            var input = "(((2-1)";

            var result = calculating.TransformToPostfixNotation(input);

            Assert.AreEqual(null, result);
        }

    }
}
