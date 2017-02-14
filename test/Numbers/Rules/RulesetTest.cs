using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers.Rules
{

    [TestClass]
    public class RulesetTest
    {
        [TestMethod]
        public void Accessibility()
        {
            Assert.IsTrue(new Ruleset().IsPublic);
            Assert.IsTrue(new Ruleset { Access = "public" }.IsPublic);
            Assert.IsTrue(new Ruleset { Access = "" }.IsPublic);
            Assert.IsFalse(new Ruleset { Access = "private" }.IsPublic);
        }
    }
}
