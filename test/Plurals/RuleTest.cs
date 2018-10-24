using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Sepia.Globalization.Plurals
{

    [TestClass]
    public class RuleTest
    {
        [TestMethod]
        public void Parsing()
        {
            var rule = Rule.Parse("one", "n = 1");
            Assert.AreEqual("one", rule.Category);
            Assert.AreEqual("", rule.Samples);
            Assert.AreEqual("n = 1", rule.Text);

            rule = Rule.Parse("one", "n = 1 @integer 1");
            Assert.AreEqual("one", rule.Category);
            Assert.AreEqual("@integer 1", rule.Samples);
            Assert.AreEqual("n = 1", rule.Text);
        }

    }
}
