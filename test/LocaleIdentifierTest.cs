using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization
{

    [TestClass]
    public class LocaleIdentifierTest
    {
        [TestMethod]
        public void Parsing_Language()
        {
            var id = LocaleIdentifier.Parse("en");
            Assert.AreEqual("en", id.Language);

            Assert.IsFalse(LocaleIdentifier.TryParse("NotALanguageCode", out id));
        }

        [TestMethod]
        public void Parsing_Language_Country()
        {
            var id = LocaleIdentifier.Parse("en-US");
            Assert.AreEqual("en", id.Language);
            Assert.AreEqual("us", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("en-NotACountryCode", out id));
        }

        [TestMethod]
        public void Parsing_Language_Region()
        {
            var id = LocaleIdentifier.Parse("es-419");
            Assert.AreEqual("es", id.Language);
            Assert.AreEqual("419", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("es-1234", out id));
        }

        [TestMethod]
        public void Parsing_Language_Script()
        {
            var  id = LocaleIdentifier.Parse("uz-Cyrl");
            Assert.AreEqual("uz", id.Language);
            Assert.AreEqual("cyrl", id.Script);

            Assert.IsFalse(LocaleIdentifier.TryParse("es-NotScript", out id));
        }

        [TestMethod]
        public void Parsing_Language_Script_Country()
        {
            var id = LocaleIdentifier.Parse("zh-Hant-CN");
            Assert.AreEqual("zh", id.Language);
            Assert.AreEqual("hant", id.Script);
            Assert.AreEqual("cn", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("zh-Hant-NotACountry", out id));
        }

        [TestMethod]
        public void Parsing_Script()
        {
            var id = LocaleIdentifier.Parse("Cyrl");
            Assert.AreEqual("cyrl", id.Script);

            Assert.IsFalse(LocaleIdentifier.TryParse("NotAScript", out id));
        }

        [TestMethod]
        public void Parsing_Script_Region()
        {
            var id = LocaleIdentifier.Parse("Cyrl-RU");
            Assert.AreEqual("cyrl", id.Script);
            Assert.AreEqual("ru", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("Cyrl-NotACounty", out id));
        }

        [TestMethod]
        public void Parsing_Underscore_Separator()
        {
            var id = LocaleIdentifier.Parse("zh_Hant_CN");
            Assert.AreEqual("zh", id.Language);
            Assert.AreEqual("hant", id.Script);
            Assert.AreEqual("cn", id.Region);
        }

        [TestMethod]
        public void Parsing_Throws()
        {
            ExceptionAssert.Throws<FormatException>(() => LocaleIdentifier.Parse("ThisIsNotALanguage"));
        }

    }
}
