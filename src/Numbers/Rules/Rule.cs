using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   A formatting rule for a particular number or sequence of numbers.
    /// </summary>
    public class Rule
    {
        /// <summary>
        ///   Create a rule from the specified <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="xml">
        ///   The XML representation of a rule based number format.
        /// </param>
        /// <returns>
        ///   A new rule.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="xml"/> must be on an "rbnfrule" element.
        /// </remarks>
        public static Rule Parse(XPathNavigator xml)
        {
            var rule = new Rule();

            return rule;
        }

    }
}
