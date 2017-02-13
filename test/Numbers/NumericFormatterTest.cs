using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers
{

    [TestClass]
    public class NumericFormatterTest
    {
        [TestMethod]
        public void Decimal_Symbol_Replacement()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("-123", formatter.Format(-123));
            Assert.AreEqual("0", formatter.Format(0));
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("1,234", formatter.Format(1234));
            Assert.AreEqual("1,234.568", formatter.Format(1234.56789));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("1\u00A0234", formatter.Format(1234));
            Assert.AreEqual("1\u00A0234,568", formatter.Format(1234.56789));
        }

        [TestMethod]
        public void Decimal_Digit_Replacement()
        {
            var locale = Locale.Create("zh-u-nu-native");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("一,二三四.五六八", formatter.Format(1234.56789m));
        }

        [TestMethod]
        public void Percent()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Percent });
            Assert.AreEqual("15%", formatter.Format(0.15));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Percent });
            Assert.AreEqual("15\u00A0%", formatter.Format(0.15));
        }

        [TestMethod]
        public void Scientific()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1.2345679E3", formatter.Format(1234.56789));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1,2345679E3", formatter.Format(1234.56789));
        }

        [TestMethod]
        public void Scientific_Posix()
        {
            var locale = Locale.Create("en-u-va-posix");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Scientific });
            Assert.AreEqual("1.234568E+003", formatter.Format(1234.56789));
        }

        [TestMethod]
        public void Currency_Standard()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("CA$123,456.79", formatter.Format(123456.789, "CAD"));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("123\u00A0456,79\u00A0$CA", formatter.Format(123456.789, "CAD"));
        }

        [TestMethod]
        public void Currency_Accounting()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyAccounting });
            Assert.AreEqual("CA$123,456.79", formatter.Format(123456.789, "CAD"));
            Assert.AreEqual("(CA$123,456.79)", formatter.Format(-123456.789, "CAD"));

            locale = Locale.Create("fr");
            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyAccounting });
            Assert.AreEqual("123\u00A0456,79\u00A0$CA", formatter.Format(123456.789, "CAD"));
            Assert.AreEqual("(123\u00A0456,79\u00A0$CA)", formatter.Format(-123456.789, "CAD"));
        }

        [TestMethod]
        public void Currency_Grouping()
        {
            var locale = Locale.Create("de-AT");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.Decimal });
            Assert.AreEqual("123\u00A0456,78", formatter.Format(123456.78));

            formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("€ 123.456,78", formatter.Format(123456.78, "EUR"));
        }

        [TestMethod]
        public void Currency_Code_Defaults_to_Locale()
        {
            var locale = Locale.Create("zh");
            var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = NumberStyle.CurrencyStandard });
            Assert.AreEqual("￥123.00", formatter.Format(123.00));
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
            Assert.AreEqual("￥0", formatter.Format(0));
            Assert.AreEqual("￥124", formatter.Format(123.78));
            Assert.AreEqual("(￥124)", formatter.Format(-123.78));
        }

        [TestMethod, Ignore]
        public void Minimum_Grouping_Digits()
        {
            var locale = Locale.Create("es_419");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("1", formatter.Format(1));
            Assert.AreEqual("12", formatter.Format(12));
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("1234", formatter.Format(1234));
            Assert.AreEqual("12,345", formatter.Format(12345));
            Assert.AreEqual("123,456", formatter.Format(123456));
            Assert.AreEqual("1,234,567", formatter.Format(1234567));
            Assert.AreEqual("12,345,678", formatter.Format(12345678));
            Assert.AreEqual("123,456,789", formatter.Format(12345679));
            Assert.AreEqual("1,234,567,890", formatter.Format(123456790));

            locale = Locale.Create("es");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("1", formatter.Format(1));
            Assert.AreEqual("12", formatter.Format(12));
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("123", formatter.Format(123));
            Assert.AreEqual("1234", formatter.Format(1234));
            Assert.AreEqual("12345", formatter.Format(12345));
            Assert.AreEqual("123,456", formatter.Format(123456));
            Assert.AreEqual("1,234,567", formatter.Format(1234567));
            Assert.AreEqual("12,345,678", formatter.Format(12345678));
            Assert.AreEqual("123,456,789", formatter.Format(12345679));
            Assert.AreEqual("1,234,567,890", formatter.Format(123456790));
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
            Assert.AreEqual("∞", formatter.Format(Double.PositiveInfinity));
            Assert.AreEqual("-∞", formatter.Format(Double.NegativeInfinity));

            locale = Locale.Create("en-u-va-posix");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("INF", formatter.Format(Double.PositiveInfinity));
            Assert.AreEqual("-INF", formatter.Format(Double.NegativeInfinity));
        }

        [TestMethod]
        public void Not_A_Number()
        {
            var locale = Locale.Create("en");
            var formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("NaN", formatter.Format(Double.NaN));

            locale = Locale.Create("fi");
            formatter = NumberFormatter.Create(locale);
            Assert.AreEqual("epäluku", formatter.Format(Double.NaN));
        }
    }
}
