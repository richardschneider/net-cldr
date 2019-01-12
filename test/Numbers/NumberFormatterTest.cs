using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

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

        [TestMethod]
        public void Style_Example()
        {
            var locale = Locale.Create("en-GB");
            foreach (var style in new[] { NumberStyle.Decimal, NumberStyle.CurrencyAccounting, NumberStyle.CurrencyStandard, NumberStyle.Percent, NumberStyle.Scientific})
            {
                var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = style });
                Console.WriteLine($"-12345 {style} {formatter.Format(-12345)}");
            }

        }

        [TestMethod]
        public void Currency_Example()
        {
            var locale = Locale.Create("en");
            foreach (var style in new[] { NumberStyle.CurrencyAccounting, NumberStyle.CurrencyStandard })
            {
                var formatter = NumberFormatter.Create(locale, new NumberOptions { Style = style });
                Console.WriteLine($"{style} {formatter.Format(-1234.56, "EUR")}");
                Console.WriteLine($"{style} {formatter.Format(-1234.56, "JPY")}");
                Console.WriteLine($"{style} {formatter.Format(-1234.56, "CNY")}");
                Console.WriteLine($"{style} {formatter.Format(-1234.56, "USD")}");
            }
        }

        [TestMethod]
        public void Length_Example()
        {
            var locale = Locale.Create("en-GB");
            foreach (var length in new[] {  NumberLength.Default, NumberLength.Short, NumberLength.Long })
            foreach (var style in new[] { NumberStyle.Decimal, NumberStyle.CurrencyStandard })
            {
                var formatter = NumberFormatter.Create(locale, new NumberOptions
                {
                    Style = style,
                    Length = length
                });
                Console.WriteLine($"| {length} | {style} | {formatter.Format(1234.56, "EUR")} |");
            }
        }
    }
}
