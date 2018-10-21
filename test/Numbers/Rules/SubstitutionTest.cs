using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace Sepia.Globalization.Numbers.Rules
{

    [TestClass]
    public class SubstitutionTest
    {
        [TestMethod]
        public void TextOnly()
        {
            var subs = Substitution.Parse("one").ToArray();
            Assert.AreEqual(1, subs.Length);
            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("one", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);
        }

        [TestMethod]
        public void Text_Recurse_LT()
        {
            var subs = Substitution.Parse("←← hundred").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("", subs[0].Text);
            Assert.AreEqual("←←", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual(" hundred", subs[1].Text);
            Assert.AreEqual("", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);
        }

        [TestMethod]
        public void Text_Recurse_GT()
        {
            var subs = Substitution.Parse("minus →→").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("minus ", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual("", subs[1].Text);
            Assert.AreEqual("→→", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);
        }

        [TestMethod]
        public void Text_Recurse_GT2()
        {
            var subs = Substitution.Parse("-→→").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("-", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual("", subs[1].Text);
            Assert.AreEqual("→→", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);
        }

        [TestMethod]
        public void Text_Optional()
        {
            var subs = Substitution.Parse("forty[-→→]").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("forty", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual("", subs[1].Text);
            Assert.AreEqual("", subs[1].Token);
            Assert.AreNotEqual(null, subs[1].Optionals);

            var opts = subs[1].Optionals;
            Assert.AreEqual("", opts[0].Descriptor);
            Assert.AreEqual("-", opts[0].Text);
            Assert.AreEqual("", opts[0].Token);
            Assert.AreEqual(null, opts[0].Optionals);

            Assert.AreEqual("", opts[1].Descriptor);
            Assert.AreEqual("", opts[1].Text);
            Assert.AreEqual("→→", opts[1].Token);
            Assert.AreEqual(null, opts[1].Optionals);
        }

        [TestMethod]
        public void Text_Transfer_to_Rule()
        {
            var subs = Substitution.Parse("e-=%%et-unieme=").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("e-", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("et-unieme", subs[1].Descriptor);
            Assert.AreEqual("", subs[1].Text);
            Assert.AreEqual("=", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);
        }

        [TestMethod]
        public void NumberPattern()
        {
            var subs = Substitution.Parse("=#,##0=").ToArray();
            Assert.AreEqual(1, subs.Length);

            Assert.AreEqual("#,##0", subs[0].Descriptor);
            Assert.AreEqual("", subs[0].Text);
            Assert.AreEqual("=", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);
        }

        [TestMethod]
        public void Text_CallRule1()
        {
            var subs = Substitution.Parse("←← cent→%%cents-m→").ToArray();
            Assert.AreEqual(3, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("", subs[0].Text);
            Assert.AreEqual("←←", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual(" cent", subs[1].Text);
            Assert.AreEqual("", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);

            Assert.AreEqual("cents-m", subs[2].Descriptor);
            Assert.AreEqual("", subs[2].Text);
            Assert.AreEqual("→", subs[2].Token);
            Assert.AreEqual(null, subs[2].Optionals);
        }

        [TestMethod]
        public void Text_CallRule2()
        {
            var subs = Substitution.Parse("vingt[-→%%et-un→]").ToArray();
            Assert.AreEqual(2, subs.Length);

            Assert.AreEqual("", subs[0].Descriptor);
            Assert.AreEqual("vingt", subs[0].Text);
            Assert.AreEqual("", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual("", subs[1].Text);
            Assert.AreEqual("", subs[1].Token);
            Assert.AreNotEqual(null, subs[1].Optionals);

            var opts = subs[1].Optionals;
            Assert.AreEqual("", opts[0].Descriptor);
            Assert.AreEqual("-", opts[0].Text);
            Assert.AreEqual("", opts[0].Token);
            Assert.AreEqual(null, opts[0].Optionals);

            Assert.AreEqual("et-un", opts[1].Descriptor);
            Assert.AreEqual("", opts[1].Text);
            Assert.AreEqual("→", opts[1].Token);
            Assert.AreEqual(null, opts[1].Optionals);
        }

        [TestMethod]
        public void Text_CallRule3()
        {
            var subs = Substitution.Parse("←%%spellout-leading← billiards[ →→]").ToArray();
            Assert.AreEqual(3, subs.Length);

            Assert.AreEqual("spellout-leading", subs[0].Descriptor);
            Assert.AreEqual("", subs[0].Text);
            Assert.AreEqual("←", subs[0].Token);
            Assert.AreEqual(null, subs[0].Optionals);

            Assert.AreEqual("", subs[1].Descriptor);
            Assert.AreEqual(" billiards", subs[1].Text);
            Assert.AreEqual("", subs[1].Token);
            Assert.AreEqual(null, subs[1].Optionals);

            Assert.AreEqual("", subs[2].Descriptor);
            Assert.AreEqual("", subs[2].Text);
            Assert.AreEqual("", subs[2].Token);
            Assert.AreNotEqual(null, subs[2].Optionals);

            var opts = subs[2].Optionals;
            Assert.AreEqual("", opts[0].Descriptor);
            Assert.AreEqual(" ", opts[0].Text);
            Assert.AreEqual("", opts[0].Token);
            Assert.AreEqual(null, opts[0].Optionals);

            Assert.AreEqual("", opts[1].Descriptor);
            Assert.AreEqual("", opts[1].Text);
            Assert.AreEqual("→→", opts[1].Token);
            Assert.AreEqual(null, opts[1].Optionals);
        }
    }
}
