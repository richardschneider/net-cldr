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
            Assert.AreEqual("〇一二三四五六七八九", hanidec.Digits);
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
    }
}
