using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Sepia.Globalization.Plurals
{

    [TestClass]
    public class RuleContextTest
    {
        [TestMethod]
        public void Creating_1()
        {
            var context = RuleContext.Create(1);
            Assert.AreEqual(1, context.n);
            Assert.AreEqual(0, context.v);
            Assert.AreEqual(0, context.w);
            Assert.AreEqual(0, context.f);
            Assert.AreEqual(0, context.t);
        }

        [TestMethod]
        public void Creating_1_3()
        {
            var context = RuleContext.Create(1.3m);
            Assert.AreEqual(1.3m, context.n);
            Assert.AreEqual(1, context.v);
            Assert.AreEqual(1, context.w);
            Assert.AreEqual(3, context.f);
            Assert.AreEqual(3, context.t);
        }

        [TestMethod]
        public void Creating_1_03()
        {
            var context = RuleContext.Create(1.03m);
            Assert.AreEqual(1.03m, context.n);
            Assert.AreEqual(2, context.v);
            Assert.AreEqual(2, context.w);
            Assert.AreEqual(3, context.f);
            Assert.AreEqual(3, context.t);
        }

        [TestMethod]
        public void Creating_1_23()
        {
            var context = RuleContext.Create(1.23m);
            Assert.AreEqual(1.23m, context.n);
            Assert.AreEqual(2, context.v);
            Assert.AreEqual(2, context.w);
            Assert.AreEqual(23, context.f);
            Assert.AreEqual(23, context.t);
        }

    }
}
