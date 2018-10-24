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

        [TestMethod]
        public void All_Supplemental_Plural_Rules()
        {
            var rules = Cldr.Instance
                .GetDocuments("common/supplemental/plurals.xml")
                .Elements($"supplementalData/plurals[@type='cardinal']/pluralRules/pluralRule")
                .Select(e => Rule.Parse(e))
                .Where(r => r.Category != "other")
                .OrderBy(r => r.Text)
                .ToList();

            var context = RuleContext.Create(1m);
            foreach (var rule in rules)
            {
                try
                {
                    rule.Matches(context);
                }
                catch (Exception e)
                {
                    throw new Exception($"{rule.Text} => {Rule.ConvertExpression(rule.Text)}", e);
                }
            }
        }

        [TestMethod]
        public void Conversion_Range_To_List()
        {
            Assert.AreEqual("0, 1,14, 15, 16", Rule.ConvertExpression("0..1,14..16"));
            Assert.AreEqual("0, 1, 14, 15, 16", Rule.ConvertExpression("0..1, 14..16"));
            Assert.AreEqual("0, 1, 14, 15, 16", Rule.ConvertExpression("0 .. 1, 14 .. 16"));
        }

        [TestMethod]
        public void Relations()
        {
            Assert.AreEqual("x = 2 or x = 3 or x = 4 or x = 15", Rule.ConvertExpression("x = 2..4, 15"));
            Assert.AreEqual("not (x = 2 or x = 3 or x = 4 or x = 15)", Rule.ConvertExpression("x != 2..4, 15"));
        }

        [TestMethod]
        public void Conversion_Mod_List_To_InFunction()
        {
            Assert.AreEqual("n % 10 = 1 or n % 10 = 2", Rule.ConvertExpression("n % 10 = 1..2"));
        }
    }
}
