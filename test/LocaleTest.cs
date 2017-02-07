using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization
{

    [TestClass]
    public class LocaleTest
    {
        [TestMethod]
        public void Creation()
        {
            var locale = Locale.Create("en");
            Assert.AreEqual("en", locale.Id.ToString());
        }

        [TestMethod]
        public void Stringify()
        {
            var locale = Locale.Create("zh-u-nu-finance");
            Assert.AreEqual(locale.Id.ToString(), locale.ToString());
        }

        [TestMethod]
        public void ResourceBundle()
        {
            var locale = Locale.Create("sl-SI-nedis");
            var bundles = locale.ResourceBundle().ToArray();
            Assert.AreEqual(3, bundles.Length);
        }

    }
}