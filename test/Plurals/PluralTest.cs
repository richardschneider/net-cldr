using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Sepia.Globalization.Plurals
{

    [TestClass]
    public class PluralTest
    {
        [TestMethod]
        public void Category_en()
        {
            var locale = Locale.Create("en");
            var plural = Plural.Create(locale);
            Assert.IsNotNull(plural);

            Assert.AreEqual("one", plural.Category(1));
            Assert.AreEqual("other", plural.Category(2));
        }

        [TestMethod]
        public void Category_ksh()
        {
            var locale = Locale.Create("ksh");
            var plural = Plural.Create(locale);
            Assert.IsNotNull(plural);

            Assert.AreEqual("zero", plural.Category(0));
            Assert.AreEqual("one", plural.Category(1));
            Assert.AreEqual("other", plural.Category(2));
        }

        [TestMethod]
        public void Category_ar()
        {
            var locale = Locale.Create("ar");
            var plural = Plural.Create(locale);
            Assert.IsNotNull(plural);

            Assert.AreEqual("zero", plural.Category(0));
            Assert.AreEqual("one", plural.Category(1));
            Assert.AreEqual("two", plural.Category(2));
            Assert.AreEqual("few", plural.Category(3));
            Assert.AreEqual("few", plural.Category(10));
            Assert.AreEqual("many", plural.Category(11));
            Assert.AreEqual("many", plural.Category(99));
            Assert.AreEqual("other", plural.Category(100));
            Assert.AreEqual("other", plural.Category(101));
            Assert.AreEqual("other", plural.Category(102));
        }

        [TestMethod]
        public void Category_No_Language_Rules()
        {
            var locale = Locale.Create("zza");
            var plural = Plural.Create(locale);
            Assert.IsNotNull(plural);

            Assert.AreEqual("other", plural.Category(1));
        }

        [TestMethod]
        public void Language_Cache()
        {
            var a = Plural.Create(Locale.Create("en-US"));
            var b = Plural.Create(Locale.Create("en-NZ"));
            Assert.AreSame(a, b);
        }

        [TestMethod]
        public void Example()
        {
            var en = Plural.Create(Locale.Create("en"));
            var cy = Plural.Create(Locale.Create("cy"));
            for (int i = 0; i <= 10; ++i)
            {
                Console.WriteLine($"| {i} | {en.Category(i)} | {cy.Category(i)} |");
            }
        }
    }
}
