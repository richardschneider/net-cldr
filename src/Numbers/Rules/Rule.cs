using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   An abstract formatting rule for a particular number or sequence of numbers.
    /// </summary>
    public abstract class Rule : IRule
    {
        /// <summary>
        /// 
        /// </summary>
        internal Substitution[] Substitutions { get; set; }

        /// <inheritdoc />
        public abstract void Fire(RbnfContext context);

        /// <inheritdoc />
        public abstract bool Matches(RbnfContext context);

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
        public static IRule Parse(XPathNavigator xml)
        {
            Rule rule = null;
            var value = xml.GetAttribute("value", "");
            var radix = xml.GetAttribute("radix", "");
            int radixNumber = radix.Length == 0
                ? 10
                : Int32.Parse(radix, CultureInfo.InvariantCulture);

            switch (value)
            {
                case "-x":
                    rule = new NegativeNumberRule();
                    break;
                case "Inf":
                    rule = new InfinityRule();
                    break;
                case "NaN":
                    rule = new NanRule();
                    break;
                case "x.x":
                    rule = new ImproperFractionRule();
                    break;
                case "0.x":
                    rule = new ProperFractionRule();
                    break;
                case "x.0":
                    rule = new MasterRule();
                    break;
                default:
                    var c = value[0];
                    if ('0' <= c && c <= '9')
                    {
                        rule = new BaseValueRule
                        {
                            LowerLimit = decimal.Parse(value),
                            radix = radixNumber
                        };
                    }
                    else
                        throw new FormatException($"Unknown rule value '{value}'.");
                    break;
            }

            var body = xml.Value;
            rule.Substitutions = Substitution.Parse(body.TrimEnd(';')).ToArray();

            return rule;
        }

    }
}
