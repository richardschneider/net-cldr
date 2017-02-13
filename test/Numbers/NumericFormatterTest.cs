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
            Assert.AreEqual("-123", formatter.ToString(-123));
            Assert.AreEqual("0", formatter.ToString(0));
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

        [TestMethod]
        public void Currency_Standard()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("CA$123,456.79", formatter.ToString(123456.789, "CAD"));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("123\u00A0456,79\u00A0$CA", formatter.ToString(123456.789, "CAD"));
        }

        [TestMethod]
        public void Currency_Accounting()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyAccounting });
            Assert.AreEqual("CA$123,456.79", formatter.ToString(123456.789, "CAD"));
            Assert.AreEqual("(CA$123,456.79)", formatter.ToString(-123456.789, "CAD"));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyAccounting });
            Assert.AreEqual("123\u00A0456,79\u00A0$CA", formatter.ToString(123456.789, "CAD"));
            Assert.AreEqual("(123\u00A0456,79\u00A0$CA)", formatter.ToString(-123456.789, "CAD"));
        }

        [TestMethod]
        public void Currency_Grouping()
        {
            var locale = Locale.Create("de-AT");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Decimal });
            Assert.AreEqual("123\u00A0456,78", formatter.ToString(123456.78));

            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("€ 123.456,78", formatter.ToString(123456.78, "EUR"));
        }

        [TestMethod]
        public void Currency_Code_Defaults_to_Locale()
        {
            var locale = Locale.Create("zh");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("￥123.00", formatter.ToString(123.00));
        }

        [TestMethod, Ignore]
        public void Currency_Spacing() // TODO
        {
        }

        [TestMethod]
        public void Currency_Decimal_Places()
        {
            var locale = Locale.Create("ja");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyAccounting });
            Assert.AreEqual("￥0", formatter.ToString(0));
            Assert.AreEqual("￥124", formatter.ToString(123.78));
            Assert.AreEqual("(￥124)", formatter.ToString(-123.78));
        }

        [TestMethod, Ignore]
        public void Minimum_Grouping_Digits() // TODO
        {
        }

        [TestMethod, Ignore]
        public void Compact_Short() // TODO
        {

        }

        [TestMethod, Ignore]
        public void Compact_Long() // TODO
        {
        }

        [TestMethod]
        public void Infinity()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("∞", formatter.ToString(Double.PositiveInfinity));
            Assert.AreEqual("-∞", formatter.ToString(Double.NegativeInfinity));

            locale = Locale.Create("en-u-va-posix");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("INF", formatter.ToString(Double.PositiveInfinity));
            Assert.AreEqual("-INF", formatter.ToString(Double.NegativeInfinity));
        }

        [TestMethod]
        public void Not_A_Number()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("NaN", formatter.ToString(Double.NaN));

            locale = Locale.Create("fi");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("epäluku", formatter.ToString(Double.NaN));
        }
    }
}
