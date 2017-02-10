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
            Assert.AreEqual("en-Latn-US", locale.Id.ToString());
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
            Assert.AreEqual(9, chain.Length);
            Assert.AreEqual("sl_Latn_SI_NEDIS", chain[0]);
            Assert.AreEqual("sl_SI_NEDIS", chain[1]);
            Assert.AreEqual("sl", chain[7]);
            Assert.AreEqual("root", chain[8]);
        }

        [TestMethod]
        public void SearchChain_ParentLocale_1()
        {
            var locale = Locale.Create("az-Cyrl");
            var chain = locale.SearchChain().ToArray();
            Assert.AreEqual(4, chain.Length);
            Assert.AreEqual("az_Cyrl_AZ", chain[0]);
            Assert.AreEqual("az_AZ", chain[1]);
            Assert.AreEqual("az_Cyrl", chain[2]);
            // "az" is not searched because parent locale override to "root"
            Assert.AreEqual("root", chain[3]);
        }

        [TestMethod]
        public void SearchChain_ParentLocale_2()
        {
            var locale = Locale.Create("es-AR");
            var chain = locale.SearchChain().ToArray();
            Assert.AreEqual(5, chain.Length);
            Assert.AreEqual("es_Latn_AR", chain[0]);
            Assert.AreEqual("es_AR", chain[1]);
            Assert.AreEqual("es_419", chain[2]);
            Assert.AreEqual("es", chain[3]);
            Assert.AreEqual("root", chain[4]);
        }

        [TestMethod]
        public void Find_Simple()
        {
            var locale = Locale.Create("ar");
            var ns = locale.Find("ldml/numbers/defaultNumberingSystem/text()").Value;
            Assert.AreEqual("arab", ns);
        }

        [TestMethod]
        public void Find_Alias()
        {
            var locale = Locale.Create("th");
            var xpath = "/ldml/dates/calendars/calendar[@type='buddhist']/dateTimeFormats/dateTimeFormatLength[@type='full']/dateTimeFormat/pattern";
            var pattern = locale.Find(xpath).Value;
            Assert.AreEqual("{1} {0}", pattern);
        }

        [TestMethod]
        public void Find_Alias_1()
        {
            var locale = Locale.Create("th");
            var xpath = "/ldml/dates/calendars/calendar[@type='buddhist']/months/monthContext[@type='format']/monthWidth[@type='abbreviated']/month[@type='1']";
            var month = locale.Find(xpath).Value;
            Assert.AreEqual("ม.ค.", month);
        }

        [TestMethod]
        public void Find_Lateral_Inheritance()
        {
            var locale = Locale.Create("fr-CA");

            var other = locale.Find(@"ldml/units/unitLength[@type='long']/unit[@type='mass-gram']/unitPattern[@count='other']").Value;
            Assert.AreEqual("{0} grammes", other);

            var one = locale.Find(@"ldml/units/unitLength[@type='long']/unit[@type='mass-gram']/unitPattern[@count='one']").Value;
            Assert.AreEqual("{0} gramme", one);

            var x = locale.Find(@"ldml/units/unitLength[@type='long']/unit[@type='mass-gram']/unitPattern[@count='x']").Value;
            Assert.AreEqual(other, x);
        }

        [TestMethod]
        public void Find_Lateral_Inheritance_Currencies()
        {
            var locale = Locale.Create("fr-CA");

            var other = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='other']").Value;
            Assert.AreEqual("dollars canadiens", other);

            var one = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='one']").Value;
            Assert.AreEqual("dollar canadien", one);

            var x = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='x']").Value;
            Assert.AreEqual(other, x);
        }

        [TestMethod]
        public void Find_Cache()
        {
            var locale = Locale.Create("fr-CA");

            var a = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='other']");
            var b = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='other']");
            var c = locale.Find(@"ldml/numbers/currencies/currency[@type='CAD']/displayName[@count='one']");
            Assert.AreSame(a, b);
            Assert.AreNotSame(a, c);
        }
    }
}