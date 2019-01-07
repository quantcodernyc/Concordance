using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordCounter;

namespace UnitTests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void ConvertNumericIndexToStringIndex_SmallerThan26()
        {
            var stringIndex = Helpers.ConvertNumericIndexToStringIndex(1);
            Assert.AreEqual("b.", stringIndex);
        }

        [TestMethod]
        public void ConvertNumericIndexToStringIndex_BiggerThan26()
        {
            var stringIndex = Helpers.ConvertNumericIndexToStringIndex(27);
            Assert.AreEqual("bb.", stringIndex);
        }

        [TestMethod]
        public void ConvertNumericIndexToStringIndex_SomeRandomBigIndex()
        {
            var stringIndex = Helpers.ConvertNumericIndexToStringIndex(56);
            Assert.AreEqual("eee.", stringIndex);
        }
    }
}
