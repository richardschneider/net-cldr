using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Makaretu.Globalization
{

    [TestClass]
    public class CodeListTest
    {
        [TestMethod]
        public void Contains()
        {
            var codes = new CodeList(" Latn   Hans~t  \r\n  Jpan ");
            Assert.IsTrue(codes.Contains("Latn"));
            Assert.IsTrue(codes.Contains("Hans"));
            Assert.IsTrue(codes.Contains("Hant"));
            Assert.IsTrue(codes.Contains("Jpan"));
            Assert.IsFalse(codes.Contains("Hung"));
            Assert.IsFalse(codes.Contains("Hanr"));
        }

    }
}
