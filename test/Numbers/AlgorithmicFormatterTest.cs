using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers
{

    [TestClass]
    public class AlgorithmicFormatterTest
    {
        [TestMethod]
        public void RomanNumerals()
        {
            var locale = Locale.Create("it-u-nu-roman");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("−CXXIII", formatter.Format(-123));
            Assert.AreEqual("−XX", formatter.Format(-20));
            Assert.AreEqual("−VII", formatter.Format(-7));
            Assert.AreEqual("−I", formatter.Format(-1));
            Assert.AreEqual("N", formatter.Format(0));
            Assert.AreEqual("I", formatter.Format(1));
            Assert.AreEqual("XX", formatter.Format(20));
            Assert.AreEqual("CXXIII", formatter.Format(123));
            Assert.AreEqual("MCCXXXIV", formatter.Format(1234));

            Assert.AreEqual("9,223,372,036,854,775,807", formatter.Format(long.MaxValue));
        }

        [TestMethod]
        public void TraditionalChinese()
        {
            var locale = Locale.Create("zh-u-nu-hant");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("一百二十三", formatter.Format(123));
        }

        [TestMethod]
        public void Japan()
        {
            var locale = Locale.Create("ja-u-nu-jpan");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("百二十三", formatter.Format(123));
        }

        [TestMethod]
        public void Hebrew()
        {
            var locale = Locale.Create("he-u-nu-hebr");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("קכ״ג", formatter.Format(123));
        }

        [TestMethod]
        public void Decimal_Finance()
        {
            var locale = Locale.Create("zh-TW-u-nu-finance");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("零", formatter.Format(0m));
            Assert.AreEqual("參拾", formatter.Format(30m));
            Assert.AreEqual("伍仟", formatter.Format(5000m));
        }

        [TestMethod]
        public void Fraction()
        {
            var locale = Locale.Create("en-u-nu-grek");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("𐆊´.Α´Β´Β´", formatter.Format(0.122m));
           
        }

    }
}
