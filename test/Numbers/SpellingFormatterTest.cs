using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

// http://demo.icu-project.org/icu4jweb/numero.jsp

namespace Sepia.Globalization.Numbers
{
    [TestClass]
    public class SpellingFormatterTest
    {
        [TestMethod]
        public void Cardinal_Formatting_en_NZ_long()
        {
            var formatter = SpellingFormatter.Create(Locale.Create("en-NZ"));
            Assert.AreEqual("minus one", formatter.Format(-1));
            Assert.AreEqual("zero", formatter.Format(0));
            Assert.AreEqual("thirteen", formatter.Format(13));
            Assert.AreEqual("twenty", formatter.Format(20));
            Assert.AreEqual("twenty-one", formatter.Format(21));
            Assert.AreEqual("three hundred", formatter.Format(300));
            Assert.AreEqual("three hundred twenty-one", formatter.Format(321));
            Assert.AreEqual("one thousand two hundred thirty-four", formatter.Format(1234));
            Assert.AreEqual("minus one thousand two hundred thirty-four", formatter.Format(-1234));
            Assert.AreEqual("twenty quadrillion", formatter.Format(20000000000000000));
            Assert.AreEqual("1,000,000,000,000,000,000", formatter.Format(1000000000000000000));

            Assert.AreEqual("minus 9,223,372,036,854,775,808", formatter.Format(long.MinValue));
            Assert.AreEqual("9,223,372,036,854,775,807", formatter.Format(long.MaxValue));
        }

        [TestMethod]
        public void Cardinal_Formatting_en_NZ_decimal()
        {
            var formatter = SpellingFormatter.Create(Locale.Create("en-NZ"));
            Assert.AreEqual("minus one", formatter.Format(-1m));
            Assert.AreEqual("zero", formatter.Format(0m));
            Assert.AreEqual("thirteen", formatter.Format(13m));
            Assert.AreEqual("twenty", formatter.Format(20m));
            Assert.AreEqual("twenty-one", formatter.Format(21m));
            Assert.AreEqual("three hundred", formatter.Format(300m));
            Assert.AreEqual("three hundred twenty-one", formatter.Format(321m));
            Assert.AreEqual("one thousand two hundred thirty-four", formatter.Format(1234m));
            Assert.AreEqual("minus one thousand two hundred thirty-four", formatter.Format(-1234m));
            Assert.AreEqual("twenty quadrillion", formatter.Format(20000000000000000m));
            Assert.AreEqual("1,000,000,000,000,000,000", formatter.Format(1000000000000000000m));

            Assert.AreEqual("one point two", formatter.Format(1.2m));
            Assert.AreEqual("one point zero two", formatter.Format(1.02m));
            Assert.AreEqual("one point zero zero zero zero two", formatter.Format(1.00002m));
            Assert.AreEqual("one point two", formatter.Format(1.2));
            Assert.AreEqual("one point zero two", formatter.Format(1.02));
            Assert.AreEqual("one point zero zero zero zero two", formatter.Format(1.00002));

            Assert.AreEqual("minus 79,228,162,514,264,337,593,543,950,335", formatter.Format(decimal.MinValue));
            Assert.AreEqual("79,228,162,514,264,337,593,543,950,335", formatter.Format(decimal.MaxValue));
        }

        [TestMethod]
        public void Cardinal_Formatting_fr_FR_decimal()
        {
            var formatter = SpellingFormatter.Create(Locale.Create("fr-FR"));
            Assert.AreEqual("moins un", formatter.Format(-1m));
            Assert.AreEqual("zéro", formatter.Format(0m));
            Assert.AreEqual("treize", formatter.Format(13m));
            Assert.AreEqual("dix-neuf", formatter.Format(19m));
            Assert.AreEqual("vingt", formatter.Format(20m));
            Assert.AreEqual("vingt-et-un", formatter.Format(21m));
            Assert.AreEqual("vingt-deux", formatter.Format(22m));
            Assert.AreEqual("soixante-neuf", formatter.Format(69m));
            Assert.AreEqual("soixante-dix-neuf", formatter.Format(79m));
            Assert.AreEqual("quatre-vingt-dix-neuf", formatter.Format(99m));
            Assert.AreEqual("trois cents", formatter.Format(300m));
            Assert.AreEqual("trois cent vingt-et-un", formatter.Format(321m));
            Assert.AreEqual("mille deux cent trente-quatre", formatter.Format(1234m));
            Assert.AreEqual("moins mille deux cent trente-quatre", formatter.Format(-1234m));
            Assert.AreEqual("vingt billiards", formatter.Format(20000000000000000m));

            var sp = Cldr.Instance.CurrentVersion() < new Version(34, 0)
                ? "\u00A0" : "\u202F";
            Assert.AreEqual($"1{sp}000{sp}000{sp}000{sp}000{sp}000{sp}000", formatter.Format(1000000000000000000m));

            Assert.AreEqual("un virgule deux", formatter.Format(1.2m));
            Assert.AreEqual("un virgule zéro deux", formatter.Format(1.02m));
            Assert.AreEqual("un virgule zéro zéro zéro zéro deux", formatter.Format(1.00002m));
        }

        [TestMethod]
        public void Ordinal_Formatting_en_NZ()
        {
            var formatter = SpellingFormatter.Create(
                Locale.Create("en-NZ"),
                new SpellingOptions { Style = SpellingStyle.Ordinal} );
            Assert.AreEqual("first", formatter.Format(1));
            Assert.AreEqual("second", formatter.Format(2));
            Assert.AreEqual("third", formatter.Format(3));
            Assert.AreEqual("fourth", formatter.Format(4));
            Assert.AreEqual("fifth", formatter.Format(5));
            Assert.AreEqual("fourteenth", formatter.Format(14));
            Assert.AreEqual("twentieth", formatter.Format(20));
            Assert.AreEqual("twenty-first", formatter.Format(21));
        }

        [TestMethod]
        public void Ordinal_Formatting_fr_FR()
        {
            var formatter = SpellingFormatter.Create(
                Locale.Create("fr-FR"),
                new SpellingOptions { Style = SpellingStyle.Ordinal });
            Assert.AreEqual("unième", formatter.Format(1));
            Assert.AreEqual("dix-huitième", formatter.Format(18));
            Assert.AreEqual("soixante-dix-neuvième", formatter.Format(79));
        }
    }
}