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
        public void Creation_Uses_A_Cache()
        {
            var a = Locale.Create("en");
            var b = Locale.Create("en");
            var c = Locale.Create("en-NZ");
            Assert.AreSame(a, b);
            Assert.AreNotSame(a, c);

            var x = Locale.Create("zh-TW");
            var y = Locale.Create("cmn-TW");
            Assert.AreSame(x, y);
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
            var bundle = locale.ResourceBundle().ToArray();
            Assert.AreEqual(3, bundle.Length);
        }

        [TestMethod]
        public void SearchChain()
        {
            var locale = Locale.Create("sl-SI-nedis");
            var chain = locale.SearchChain().ToArray();
            Assert.AreEqual(4, chain.Length);
            Assert.AreEqual("sl_SI_NEDIS", chain[0]);
            Assert.AreEqual("sl_SI", chain[1]);
            Assert.AreEqual("sl", chain[2]);
            Assert.AreEqual("root", chain[3]);
        }

        [TestMethod]
        public void SearchChain_ParentLocale_1()
        {
            var locale = Locale.Create("az-Cyrl");
            var chain = locale.SearchChain().ToArray();
            Assert.AreEqual(2, chain.Length);
            Assert.AreEqual("az_Cyrl", chain[0]);
            Assert.AreEqual("root", chain[1]);
        }

        [TestMethod]
        public void SearchChain_ParentLocale_2()
        {
            var locale = Locale.Create("es-AR");
            var chain = locale.SearchChain().ToArray();
            Assert.AreEqual(4, chain.Length);
            Assert.AreEqual("es_AR", chain[0]);
            Assert.AreEqual("es_419", chain[1]);
            Assert.AreEqual("es", chain[2]);
            Assert.AreEqual("root", chain[3]);
        }

    }
}