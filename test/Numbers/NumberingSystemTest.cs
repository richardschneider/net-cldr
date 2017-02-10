using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization.Numbers
{

    [TestClass]
    public class NumberingSystemTest
    {
        [TestMethod]
        public void Create_From_Id()
        {
            var hanidec = NumberingSystem.Create("HANIDEC");
            Assert.AreEqual("hanidec", hanidec.Id);
            Assert.AreEqual("〇一二三四五六七八九", String.Join("", hanidec.Digits));
        }

        [TestMethod]
        public void Create_From_Undefined_Id()
        {
            ExceptionAssert.Throws<KeyNotFoundException>(() => NumberingSystem.Create("ns-is-not-defined"));
        }

        [TestMethod]
        public void Create_Uses_A_Cache()
        {
            var a = NumberingSystem.Create("arab");
            var b = NumberingSystem.Create("arab");
            var c = NumberingSystem.Create("arabext");
            Assert.AreSame(a, b);
            Assert.AreNotSame(a, c);
        }

        [TestMethod]
        public void Create_From_Locale_Default()
        {
            var en = Locale.Create("en");
            Assert.AreEqual("latn", NumberingSystem.Create(en).Id);

            var ar = Locale.Create("ar");
            Assert.AreEqual("arab", NumberingSystem.Create(ar).Id);
        }

        [TestMethod]
        public void Create_From_Locale_Specified()
        {
            var a = Locale.Create("zh");
            Assert.AreEqual("latn", NumberingSystem.Create(a).Id);

            var b = Locale.Create("zh-u-nu-hanidec");
            Assert.AreEqual("hanidec", NumberingSystem.Create(b).Id);

            var c = Locale.Create("zh-u-nu-unknown");
            Assert.AreEqual("latn", NumberingSystem.Create(c).Id);
        }

        [TestMethod]
        public void Create_From_Locale_Native()
        {
            var hi_default = Locale.Create("hi-IN");
            Assert.AreEqual("latn", NumberingSystem.Create(hi_default).Id);

            var hi_native = Locale.Create("hi-IN-u-nu-native");
            Assert.AreEqual("deva", NumberingSystem.Create(hi_native).Id);
        }

        [TestMethod]
        public void Create_From_Locale_Finance()
        {
            var zh_default = Locale.Create("zh");
            Assert.AreEqual("latn", NumberingSystem.Create(zh_default).Id);

            var zh_finance = Locale.Create("zh-u-nu-finance");
            Assert.AreEqual("hansfin", NumberingSystem.Create(zh_finance).Id);
        }

        [TestMethod]
        public void Create_From_Locale_Traditional()
        {
            var ta_default = Locale.Create("ta");
            Assert.AreEqual("latn", NumberingSystem.Create(ta_default).Id);

            var ta_traditional = Locale.Create("ta-u-nu-traditio");
            Assert.AreEqual("taml", NumberingSystem.Create(ta_traditional).Id);
        }

        [TestMethod]
        public void Digits()
        {
            var ns = NumberingSystem.Create("mathbold");
            Assert.AreEqual("\U0001D7CE", ns.Digits[0]);
            Assert.AreEqual("\U0001D7D7", ns.Digits[9]);
        }
    }
}
