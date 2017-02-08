using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization
{

    [TestClass]
    public class LocaleExtensionTest
    {
        [TestMethod]
        public void Parsing_Unicode_Extensions()
        {
            var extension = LocaleExtension.Parse("u-bar-foo-ca-buddhist-kk-nu-thai");
            CollectionAssert.Contains(extension.Attributes.ToArray(), "foo");
            CollectionAssert.Contains(extension.Attributes.ToArray(), "bar");
            Assert.AreEqual("buddhist", extension.Keywords["ca"]);
            Assert.AreEqual("true", extension.Keywords["kk"]);
            Assert.AreEqual("thai", extension.Keywords["nu"]);
        }

    }
}