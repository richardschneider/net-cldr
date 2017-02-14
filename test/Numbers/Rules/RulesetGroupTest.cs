using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers.Rules
{
    [TestClass]
    public class RulesetGroupTest
    {
        [TestMethod]
        public void Parsing()
        {
            var xml = Cldr.Instance
                .GetDocuments("common/rbnf/en.xml")
                .FirstElement("ldml/rbnf/rulesetGrouping");
            var group = RulesetGroup.Parse(xml);
            Assert.AreEqual("SpelloutRules", group.Type);
            Assert.AreNotEqual(0, group.Rulesets.Count);
        }
    }
}
