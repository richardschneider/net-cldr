using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization.Numbers
{

    [TestClass]
    public class NumericFormatterTest
    {
        [TestMethod]
        public void Decimal_Symbol_Replacement()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("123", formatter.ToString(123));
            Assert.AreEqual("1,234", formatter.ToString(1234));
            Assert.AreEqual("1,234.568", formatter.ToString(1234.56789));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("123", formatter.ToString(123));
            Assert.AreEqual("1\u00A0234", formatter.ToString(1234));
            Assert.AreEqual("1\u00A0234,568", formatter.ToString(1234.56789));
        }

        [TestMethod]
        public void Decimal_Digit_Replacement()
        {
            var locale = Locale.Create("zh-u-nu-native");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("一,二三四.五六八", formatter.ToString(1234.56789m));
        }

        [TestMethod]
        public void Percent()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Percent });
            Assert.AreEqual("15%", formatter.ToString(0.15));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Percent });
            Assert.AreEqual("15\u00A0%", formatter.ToString(0.15));
        }

        [TestMethod]
        public void Scientific()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1.2345679E3", formatter.ToString(1234.56789));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1,2345679E3", formatter.ToString(1234.56789));
        }

        [TestMethod]
        public void Scientific_Posix()
        {
            var locale = Locale.Create("en-u-va-posix");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1.234568E+003", formatter.ToString(1234.56789));
        }

    }
}
