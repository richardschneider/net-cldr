using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization
{

    [TestClass]
    public class CldrTest
    {
        [TestMethod]
        public void Current_Version()
        {
            var minVersion = new Version("30.0");
            var curVersion = Cldr.Instance.CurrentVersion();
            Assert.IsTrue(curVersion >= minVersion);
        }

        [TestMethod]
        public void Latest_Version()
        {
            var minVersion = new Version("30.0");
            var latest = Cldr.LatestVersionAsync().Result;
            Assert.IsTrue(latest > minVersion);
        }

        [TestMethod, Ignore]
        public void Download_Specific_Version()
        {
            var files = Cldr.Instance.DownloadAsync(new Version("30.0.0")).Result;
            CollectionAssert.Contains(files, "core.zip");
        }

        [TestMethod]
        public void GetDocuments()
        {
            var files = Cldr.Instance.GetDocuments("common/supplemental/supplementalData.xml");
            Assert.AreNotEqual(0, files.Count());
        }

        [TestMethod]
        public void GetDocuments_NotFound()
        {
            ExceptionAssert.Throws<FileNotFoundException>(() => Cldr.Instance.GetDocuments("common/x.xml").ToArray());
        }

        [TestMethod]
        public void Query()
        {
            var bhd = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .FirstElement("supplementalData/currencyData/fractions/info[@iso4217='BHD']");
            Assert.AreEqual("3", bhd.Attribute("digits").Value);
        }

        [TestMethod]
        public void Query_Not_Found()
        {
            ExceptionAssert.Throws<KeyNotFoundException>(() =>
            {
                Cldr.Instance
                    .GetDocuments("common/supplemental/supplementalData.xml")
                    .FirstElement("supplementalData/currencyData/fractions/info[@iso4217='unknown']");
            });
        }

        [TestMethod]
        public void Query_Default()
        {
            var bhd = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .FirstElementOrDefault("supplementalData/currencyData/fractions/info[@iso4217='BHD']");
            Assert.AreEqual("3", bhd.Attribute("digits").Value);

            var unknown = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .FirstElementOrDefault("supplementalData/currencyData/fractions/info[@iso4217='unknown']");
            Assert.IsNull(unknown);
        }

        [TestMethod]
        public void Query_Default_Action()
        {
            var bhd = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .FirstElementOrDefault("supplementalData/currencyData/fractions/info[@iso4217='BHD']", docs => null);
            Assert.AreEqual("3", bhd.Attribute("digits").Value);

            var unknown = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalData.xml")
                .FirstElementOrDefault(
                    "supplementalData/currencyData/fractions/info[@iso4217='unknown']",
                    docs => docs.FirstElement("supplementalData/currencyData/fractions/info[@iso4217='DEFAULT']"));
            Assert.AreEqual("2", unknown.Attribute("digits").Value);
        }
    }
}
