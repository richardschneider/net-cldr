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
        RulesetGroup spelloutRules;

        public RulesetGroupTest()
        {
            var xml = Cldr.Instance
                .GetDocuments("common/rbnf/en.xml")
                .FirstElement("ldml/rbnf/rulesetGrouping");
            spelloutRules = RulesetGroup.Parse(xml);
        }

        [TestMethod]
        public void Loading()
        {
            Assert.AreEqual("SpelloutRules", spelloutRules.Type);
            Assert.AreNotEqual(0, spelloutRules.Rulesets.Count);
        }

        [TestMethod]
        public void Formatting()
        {
            Assert.AreEqual("minus one", spelloutRules.Format(-1m, "spellout-numbering"));
            Assert.AreEqual("zero", spelloutRules.Format(0m, "spellout-numbering"));
            Assert.AreEqual("thirteen", spelloutRules.Format(13m, "spellout-numbering"));
            Assert.AreEqual("twenty", spelloutRules.Format(20m, "spellout-numbering"));
            Assert.AreEqual("twenty-one", spelloutRules.Format(21m, "spellout-numbering"));
            Assert.AreEqual("three hundred", spelloutRules.Format(300m, "spellout-numbering"));
            Assert.AreEqual("three hundred twenty-one", spelloutRules.Format(321m, "spellout-numbering"));
            Assert.AreEqual("one thousand two hundred thirty-four", spelloutRules.Format(1234m, "spellout-numbering"));
            Assert.AreEqual("minus one thousand two hundred thirty-four", spelloutRules.Format(-1234m, "spellout-numbering"));
        }

        [TestMethod]
        public void Formatting_Max_Value()
        {
            Assert.AreEqual("one thousand two hundred thirty-four", spelloutRules.Format(decimal.MaxValue, "spellout-numbering"));
        }

        [TestMethod]
        public void Formatting_Min_Value()
        {
            Assert.AreEqual("one thousand two hundred thirty-four", spelloutRules.Format(decimal.MaxValue, "spellout-numbering"));
        }

    }
}