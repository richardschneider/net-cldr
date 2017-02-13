using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization
{

    [TestClass]
    public class StringRangeTest
    {
        [TestMethod, Ignore]
        public void TR35_Example_1()
        {
            var strings = StringRange.Enumerate("ab", "ad").ToArray();
            Assert.AreEqual(3, strings.Length);
            CollectionAssert.Contains(strings, "ab");
            CollectionAssert.Contains(strings, "ac");
            CollectionAssert.Contains(strings, "ad");
        }

        [TestMethod]
        public void TR35_Example_2()
        {
            var strings = StringRange.Enumerate("ab", "d").ToArray();
            Assert.AreEqual(3, strings.Length);
            CollectionAssert.Contains(strings, "ab");
            CollectionAssert.Contains(strings, "ac");
            CollectionAssert.Contains(strings, "ad");
        }

        [TestMethod, Ignore]
        public void TR35_Example_3()
        {
            var strings = StringRange.Enumerate("ab", "cd").ToArray();
            Assert.AreEqual(9, strings.Length);
            CollectionAssert.Contains(strings, "ab");
            CollectionAssert.Contains(strings, "ac");
            CollectionAssert.Contains(strings, "ad");
            CollectionAssert.Contains(strings, "bb");
            CollectionAssert.Contains(strings, "bc");
            CollectionAssert.Contains(strings, "bd");
            CollectionAssert.Contains(strings, "cb");
            CollectionAssert.Contains(strings, "cc");
            CollectionAssert.Contains(strings, "cd");
        }
    }
}
