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
        public void Decimal()
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

    }
}
