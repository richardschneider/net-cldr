using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Specifies a formatting pattern for a number or for
    ///   a range of values.
    /// </summary>
    /// <example>
    ///   ldml simple pattern
    ///   <code>
    ///     &lt;pattern>#,##0.###&lt;/pattern>
    ///   </code>
    ///   ldml range of values
    ///   <code>
    ///     &lt;pattern type="1000" count="one">0 thousand&lt;/pattern>
    ///  	&lt;pattern type="1000" count="other">0 thousand&lt;/pattern>
	///     &lt;pattern type="10000" count="one">00 thousand&lt;/pattern>
    ///   </code>
    /// </example>
    public class NumberPattern
    {
        /// <summary>
        ///   The minimum value for the pattern.
        /// </summary>
        /// <value>
        ///   Defaults to <b>null</b>.
        /// </value>
        public decimal? MinValue { get; set; }

        /// <summary>
        ///   The plural category.
        /// </summary>
        /// <value>
        ///   Defaults to <see cref="string.Empty"/>.
        /// </value>
        /// <seealso cref="Plurals.Plural"/>
        public string Count { get; set; }

        /// <summary>
        ///   The string used to format a number.
        /// </summary>
        /// <seealso href="http://www.unicode.org/reports/tr35/tr35-33/tr35-numbers.html#Number_Patterns"/>
        public string FormatString { get; set; }

        /// <summary>
        ///   Does a value need adjusting for this pattern.
        /// </summary>
        /// <returns></returns>
        public bool NumberNeedsAdjusting()
        {
            return MinValue.HasValue;
        }

        /// <summary>
        ///   Adjust the number to the pattern.
        /// </summary>
        /// <param name="number">
        ///   The number to eventually format.
        /// </param>
        /// <remarks>
        ///   Adjustments are required when a pattern for a range of values is used.
        /// </remarks>
        public decimal Adjust(decimal number)
        {
            if (!MinValue.HasValue)
                return number;

            var n = FormatString.Count(c => c == '0');
            return Math.Floor(number / MinValue.Value * (decimal)Math.Pow(10, n - 1));
        }

        /// <summary>
        ///   Create a number pattern from the specified <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="xml">
        ///   The XML representation of a number pattern.
        /// </param>
        /// <returns>
        ///   A new number pattern,
        /// </returns>
        /// <remarks>
        ///   The <paramref name="xml"/> must be on a "pattern" element.
        /// </remarks>
        public static NumberPattern Parse(XPathNavigator xml)
        {
            if (xml.LocalName != "pattern")
                throw new Exception($"Expected a 'pattern' element, not '{xml.LocalName}'.");

            var pattern = new NumberPattern
            {
                Count = xml.GetAttribute("count", ""),
                FormatString = xml.Value
            };
            var s = xml.GetAttribute("type", "");
            if (s != String.Empty)
            {
                pattern.MinValue = decimal.Parse(s, CultureInfo.InvariantCulture);
            }

            return pattern;
        }

    }
}
