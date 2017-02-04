﻿using System;
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
            Assert.AreEqual("US", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("en-NotACountryCode", out id));
        }

        [TestMethod]
        public void Parsing_Language_Region()
        {
            var id = LocaleIdentifier.Parse("es-419");
            Assert.AreEqual("es", id.Language);
            Assert.AreEqual("419", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("es-12345", out id));
        }

        [TestMethod]
        public void Parsing_Language_Script()
        {
            var id = LocaleIdentifier.Parse("uz-Cyrl");
            Assert.AreEqual("uz", id.Language);
            Assert.AreEqual("Cyrl", id.Script);

            Assert.IsFalse(LocaleIdentifier.TryParse("es-NotScript", out id));
        }

        [TestMethod]
        public void Parsing_Language_Variant()
        {
            var id = LocaleIdentifier.Parse("sl-nedis");
            Assert.AreEqual("sl", id.Language);
            Assert.IsTrue(id.Variants.Contains("nedis"));
        }

        [TestMethod]
        public void Parsing_Language_Region_Variant()
        {
            var id = LocaleIdentifier.Parse("de-CH-1996");
            Assert.AreEqual("de", id.Language);
            Assert.AreEqual("CH", id.Region);
            Assert.IsTrue(id.Variants.Contains("1996"));
        }

        [TestMethod]
        public void Parsing_Language_Region_Multiple_Variants()
        {
            var id = LocaleIdentifier.Parse("de-CH-1996-1998");
            Assert.AreEqual("de", id.Language);
            Assert.AreEqual("CH", id.Region);
            Assert.IsTrue(id.Variants.Contains("1996"));
            Assert.IsTrue(id.Variants.Contains("1998"));
        }

        [TestMethod]
        public void Parsing_Language_Script_Country()
        {
            var id = LocaleIdentifier.Parse("zh-Hant-CN");
            Assert.AreEqual("zh", id.Language);
            Assert.AreEqual("Hant", id.Script);
            Assert.AreEqual("CN", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("zh-Hant-NotACountry", out id));
        }

        [TestMethod]
        public void Parsing_Script()
        {
            var id = LocaleIdentifier.Parse("Cyrl");
            Assert.AreEqual("Cyrl", id.Script);

            Assert.IsFalse(LocaleIdentifier.TryParse("NotAScript", out id));
        }

        [TestMethod]
        public void Parsing_Script_Region()
        {
            var id = LocaleIdentifier.Parse("Cyrl-RU");
            Assert.AreEqual("Cyrl", id.Script);
            Assert.AreEqual("RU", id.Region);

            Assert.IsFalse(LocaleIdentifier.TryParse("Cyrl-NotACounty", out id));
        }

        [TestMethod]
        public void Parsing_Underscore_Separator()
        {
            var id = LocaleIdentifier.Parse("zh_Hant_CN");
            Assert.AreEqual("zh", id.Language);
            Assert.AreEqual("Hant", id.Script);
            Assert.AreEqual("CN", id.Region);
        }

        [TestMethod]
        public void Parsing_Extension()
        {
            var id = LocaleIdentifier.Parse("en-Latn-GB-r-extended-sequence-r-foo");
            Assert.AreEqual("en", id.Language);
            Assert.AreEqual("Latn", id.Script);
            Assert.AreEqual("GB", id.Region);
            Assert.IsTrue(id.Extensions.Contains("r-extended-sequence"));
            Assert.IsTrue(id.Extensions.Contains("r-foo"));
        }

        [TestMethod]
        public void Parsing_Throws()
        {
            ExceptionAssert.Throws<FormatException>(() => LocaleIdentifier.Parse("ThisIsNotALanguage"));
        }

        [TestMethod]
        public void Parsing_Messages()
        {
            string message;
            LocaleIdentifier id;
            var ok = LocaleIdentifier.TryParse("en-r-alpha-r-alpha", out id, out message);
            Assert.AreEqual(false, ok);
            Assert.AreEqual(null, id);
            Assert.AreEqual("'en-r-alpha-r-alpha' is not a valid locale identifier because an extension is duplicated.", message);
        }

        [TestMethod]
        public void Parsing_Variants_Are_Not_Repeated()
        {
            string message;
            LocaleIdentifier id;
            var ok = LocaleIdentifier.TryParse("de-CH-1901-1901", out id, out message);
            Assert.AreEqual(false, ok);
            Assert.AreEqual(null, id);
            Assert.AreEqual("'de-CH-1901-1901' is not a valid locale identifier because a variant is duplicated.", message);

            ExceptionAssert.Throws<FormatException>(() => LocaleIdentifier.Parse("de-CH-1901-1901"));
        }

        [TestMethod]
        public void Empty_Tags()
        {
            var id = LocaleIdentifier.Parse("Cyrl");
            Assert.AreEqual("", id.Language);
            Assert.AreEqual("Cyrl", id.Script);
            Assert.AreEqual("", id.Region);
            Assert.AreEqual(0, id.Extensions.Count());
            Assert.AreEqual(0, id.Variants.Count());

            id = LocaleIdentifier.Parse("en");
            Assert.AreEqual("en", id.Language);
            Assert.AreEqual("", id.Script);
            Assert.AreEqual("", id.Region);
            Assert.AreEqual(0, id.Extensions.Count());
            Assert.AreEqual(0, id.Variants.Count());
        }

        [TestMethod]
        public void Parsing_Transforms()
        {
            Assert.AreEqual("en-US", LocaleIdentifier.Parse("en-US").ToString());
            Assert.AreEqual("root", LocaleIdentifier.Parse("und").ToString());
            Assert.AreEqual("und-US", LocaleIdentifier.Parse("und-US").ToString());
            Assert.AreEqual("root-u-cu-usd", LocaleIdentifier.Parse("und-u-cu-USD").ToString());
            Assert.AreEqual("zh-TW", LocaleIdentifier.Parse("cmn-TW").ToString());
            Assert.AreEqual("sr-RS", LocaleIdentifier.Parse("sr-CS").ToString());
            Assert.AreEqual("sr-Latn", LocaleIdentifier.Parse("sh").ToString());
            Assert.AreEqual("sr-Cyrl", LocaleIdentifier.Parse("sh-Cyrl").ToString());
            Assert.AreEqual("hy-AM", LocaleIdentifier.Parse("hy-SU").ToString());
        }

        [TestMethod]
        public void Formatting()
        {
            var id = LocaleIdentifier.Parse("EN_LATN_GB_R_EXTENDED_SEQUENCE_R-FOO");
            Assert.AreEqual("en-Latn-GB-r-extended-sequence-r-foo", id.ToString());

            id = LocaleIdentifier.Parse("EN_nz");
            Assert.AreEqual("en-NZ", id.ToString());

            id = LocaleIdentifier.Parse("EN");
            Assert.AreEqual("en", id.ToString());
        }

        [TestMethod]
        public void UnicodeLangugeTag()
        {
            var id = LocaleIdentifier.Parse("EN_LATN_GB_R_EXTENDED_SEQUENCE_R-FOO");
            Assert.AreEqual("en_Latn_GB", id.ToUnicodeLanguage());

            id = LocaleIdentifier.Parse("EN_nz");
            Assert.AreEqual("en_NZ", id.ToUnicodeLanguage());

            id = LocaleIdentifier.Parse("EN");
            Assert.AreEqual("en", id.ToUnicodeLanguage());
        }

        [TestMethod]
        public void MostLikely()
        {
            Assert.AreEqual("en-Latn-US", LocaleIdentifier.Parse("en").MostLikelySubtags().ToString());
            Assert.AreEqual("zh-Hans-CN", LocaleIdentifier.Parse("zh").MostLikelySubtags().ToString());
            Assert.AreEqual("zh-Hant-TW", LocaleIdentifier.Parse("zh-TW").MostLikelySubtags().ToString());
            Assert.AreEqual("zh-Hans-SG", LocaleIdentifier.Parse("ZH-ZZZZ-SG").MostLikelySubtags().ToString());
        }

    }
}