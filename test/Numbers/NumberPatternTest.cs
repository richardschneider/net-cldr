using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Sepia.Globalization.Numbers
{

    [TestClass]
    public class NumberPatternTest
    {
        [TestMethod]
        public void Parse_Simple()
        {
            var locale = Locale.Create("en");
            var path = $"ldml/numbers/decimalFormats[@numberSystem='latn']/decimalFormatLength[not(@type)]/decimalFormat/pattern";
            var pattern = NumberPattern.Parse(locale.Find(path));
            Assert.AreEqual("", pattern.Count);
            Assert.IsFalse(pattern.MinValue.HasValue);
            Assert.AreEqual("#,##0.###", pattern.FormatString);
        }

        [TestMethod]
        public void Parse_ValueRange()
        {
            var locale = Locale.Create("en");
            var path = $"ldml/numbers/decimalFormats[@numberSystem='latn']/decimalFormatLength[@type='long']/decimalFormat/pattern";
            var pattern = NumberPattern.Parse(locale.Find(path));
            Assert.AreEqual("one", pattern.Count);
            Assert.IsTrue(pattern.MinValue.HasValue);
            Assert.AreEqual(1000L, pattern.MinValue.Value);
            Assert.AreEqual("0 thousand", pattern.FormatString);
        }

        [TestMethod]
        public void Parse_NotAPattern()
        {
            var locale = Locale.Create("en");
            var path = $"ldml/numbers/decimalFormats[@numberSystem='latn']/decimalFormatLength[@type='long']/decimalFormat";
            ExceptionAssert.Throws<Exception>(() => NumberPattern.Parse(locale.Find(path)));
        }

        [TestMethod]
        public void Adjust_Simple()
        {
            var pattern = new NumberPattern { FormatString = "#,##0" };
            Assert.AreEqual(12345, pattern.Adjust(12345));
        }

        [TestMethod]
        public void Adjust_ValueRange()
        {
            var pattern = new NumberPattern
            {
                MinValue = 10000,
                Count = "other",
                FormatString = "00 K"
            };
            Assert.AreEqual(12, pattern.Adjust(12345));
        }
    }
}
