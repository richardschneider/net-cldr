using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Sepia.Globalization
{
    /// <summary>
    ///   The execution context for XPath.
    /// </summary>
    /// <remarks>
    ///   Allows XPath to resolve functions, parameters, and namespaces within 
    ///   XPath expressions.
    ///   
    ///   <list type="table">
    ///     <listheader>  
    ///         <term>Function</term>  
    ///         <description>Usage</description>  
    ///     </listheader>  
    ///     <item>  
    ///         <term>cldr:contains-code</term>  
    ///         <description>code[cldr:contains-code(., 'x')]</description>  
    ///     </item>
    ///   </list>
    /// </remarks>
    public class CldrContext : XsltContext
    {
        /// <summary>
        ///   The default XPath context.
        /// </summary>
        public static CldrContext Default = new CldrContext();

        Dictionary<string, IXsltContextFunction> functions = new Dictionary<string, IXsltContextFunction>
        {
            { "contains-code", new ContainsCodeFunction() },
        };

        /// <inheritdoc/>
        public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
        {
            IXsltContextFunction function = null;
            if (prefix == "cldr")
                functions.TryGetValue(name, out function);

            return function;
        }

        /// <inheritdoc/>
        public override bool Whitespace
        {
            get
            {
               return true;
            }
        }

        /// <inheritdoc/>
        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        /// <inheritdoc/>
        public override bool PreserveWhitespace(XPathNavigator node)
        {
            return true;
        }

        /// <inheritdoc/>
        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            return null;
        }

    }

    class ContainsCodeFunction : IXsltContextFunction
    {
        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
        {
            var text = ((XPathNodeIterator)args[0]).Current.Value;
            var codes = new CodeList(text);
            return codes.Contains((string)args[1]);
        }

        public XPathResultType[] ArgTypes { get { return new[] { XPathResultType.NodeSet, XPathResultType.String }; } }
        public int Maxargs { get { return 2; } }
        public int Minargs { get { return 2; } }
        public XPathResultType ReturnType { get { return XPathResultType.Boolean; } }
    }

}
