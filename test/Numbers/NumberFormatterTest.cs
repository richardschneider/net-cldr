using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers
{

    [TestClass]
    public class NumberFormatterTest
    {
        [TestMethod]
        public void Algorithmic_Systems_Are_Supported()
        {
            var locale = Locale.Create("zh-u-nu-cyrl");
            var formatter = NumberFormatter.Create(locale);
            Assert.IsNotNull(formatter);
        }

        [TestMethod]
        public void Numeric_Systems_Are_Supported()
        {
            var locale = Locale.Create("zh-u-nu-hanidec");
            var formatter = NumberFormatter.Create(locale);
            Assert.IsNotNull(formatter);
        }
    }
}
