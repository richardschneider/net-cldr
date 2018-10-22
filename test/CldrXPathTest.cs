using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Sepia.Globalization
{

    [TestClass]
    public class CldrXPathTest
    {
        XPathNavigator Match(string xml, string predicate)
        {
            var doc = new XPathDocument(new StringReader(xml));
            var nav = doc.CreateNavigator();
            var expr = nav.Compile(predicate);
            expr.SetContext(CldrContext.Default);
            return nav.SelectSingleNode(expr);
        }

        [TestMethod]
        public void Functions_Require_Prefix()
        {
            var xml = "<foo/>";
            Match(xml, "foo[cldr:contains-code(., 'x')]");

            ExceptionAssert.Throws<XPathException>(() => Match(xml, "foo[contains-code(., 'x')]"));
        }
        [TestMethod]
        public void ContainsCode()
        {
            var xml = @"<codes><some>aa~c</some><some>az</some></codes>";
            Assert.IsNotNull(Match(xml, "codes/some[cldr:contains-code(., 'aa')]"));
            Assert.IsNotNull(Match(xml, "codes/some[cldr:contains-code(., 'ab')]"));
            Assert.IsNotNull(Match(xml, "codes/some[cldr:contains-code(., 'ac')]"));
            Assert.IsNotNull(Match(xml, "codes/some[cldr:contains-code(., 'az')]"));
            Assert.IsNull(Match(xml, "codes/some[cldr:contains-code(., 'ad')]"));
            Assert.IsNull(Match(xml, "codes/some[cldr:contains-code(., 'ax')]"));
        }

        [TestMethod]
        public void WhiteSpace()
        {
            Assert.IsTrue(CldrContext.Default.Whitespace);
            Assert.IsTrue(CldrContext.Default.PreserveWhitespace(null));
        }

        [TestMethod]
        public void ResolveVariables()
        {
            // No variables.
            Assert.IsNull(CldrContext.Default.ResolveVariable("cldr", "foo"));
        }

        [TestMethod]
        public void Resolve_ContainsCode()
        {
            var func = CldrContext.Default.ResolveFunction("cldr", "contains-code", null);
            Assert.IsNotNull(func);

            Assert.AreEqual(2, func.Maxargs);
            Assert.AreEqual(2, func.Minargs);
            Assert.AreEqual(XPathResultType.Boolean, func.ReturnType);
            CollectionAssert.AreEqual(new[] { XPathResultType.NodeSet, XPathResultType.String }, func.ArgTypes);
        }
    }
}
