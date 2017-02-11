using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization.Numbers
{

    [TestClass]
    public class NumberSymbolsTest
    {
        [TestMethod]
        public void Currency_Defaults()
        {
            var symbols = new NumberSymbols
            {
                Decimal = ".",
                Group = ","
            };
            Assert.AreEqual(".", symbols.CurrencyDecimal);
            Assert.AreEqual(",", symbols.CurrencyGroup);

            symbols = new NumberSymbols
            {
                CurrencyDecimal = ".X",
                CurrencyGroup = ",X"
            };
            Assert.AreEqual(".X", symbols.CurrencyDecimal);
            Assert.AreEqual(",X", symbols.CurrencyGroup);
        }

        [TestMethod]
        public void Create_From_Locale()
        {
            var locale = Locale.Create("de");
            var symbols = NumberSymbols.Create(locale);
            Assert.AreEqual(",", symbols.CurrencyDecimal);
            Assert.AreEqual(".", symbols.CurrencyGroup);
            Assert.AreEqual(",", symbols.Decimal);
            Assert.AreEqual("E", symbols.Exponential);
            Assert.AreEqual(".", symbols.Group);
            Assert.AreEqual("∞", symbols.Infinity);
            Assert.AreEqual(";", symbols.List);
            Assert.AreEqual("-", symbols.MinusSign);
            Assert.AreEqual("NaN", symbols.NotANumber);
            Assert.AreEqual("%", symbols.PercentSign);
            Assert.AreEqual("‰", symbols.PerMille);
            Assert.AreEqual("+", symbols.PlusSign);
            Assert.AreEqual("·", symbols.SuperscriptingExponent);

            locale = Locale.Create("de-AT");
            symbols = NumberSymbols.Create(locale);
            Assert.AreEqual(",", symbols.CurrencyDecimal);
            Assert.AreEqual(".", symbols.CurrencyGroup);
            Assert.AreEqual(",", symbols.Decimal);
            Assert.AreEqual("E", symbols.Exponential);
            Assert.AreEqual("\u00A0", symbols.Group);
            Assert.AreEqual("∞", symbols.Infinity);
            Assert.AreEqual(";", symbols.List);
            Assert.AreEqual("-", symbols.MinusSign);
            Assert.AreEqual("NaN", symbols.NotANumber);
            Assert.AreEqual("%", symbols.PercentSign);
            Assert.AreEqual("‰", symbols.PerMille);
            Assert.AreEqual("+", symbols.PlusSign);
            Assert.AreEqual("·", symbols.SuperscriptingExponent);
        }
    }
}
