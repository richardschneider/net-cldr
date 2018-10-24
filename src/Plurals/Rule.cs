using NCalc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Plurals
{
    /// <summary>
    ///   A plural rule.
    /// </summary>
    /// <seealso href="http://unicode.org/reports/tr35/tr35-numbers.html#Plural_rules_syntax"/>
    public class Rule
    {
        static Regex rangePattern = new Regex(@"(\d+)\s*\.\.\s*(\d+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        static Regex relationPattern = new Regex(@"(\w)\s*(\!\=|\=)\s*((\d+(\s*,\s*\d+)+))", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        Func<RuleContext, bool> expression;

        /// <summary>
        ///   Gets the plural category for the rule.
        /// </summary>
        /// <value>
        ///  "zero", "one", "two", "few", "many" or "other".
        /// </value>
        public string Category { get; private set; }

        /// <summary>
        ///   The textual representation of the expression to evaluate.
        /// </summary>
        public string Text { get; private set; } = String.Empty;

        /// <summary>
        ///   Some examples that match.
        /// </summary>
        public string Samples { get; private set; } = String.Empty;

        /// <summary>
        ///   Determines if the rule matches the <see cref="RuleContext"/>.
        /// </summary>
        /// <param name="context">
        ///   The context.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the rule matches the <paramref name="context"/>.
        /// </returns>
        public bool Matches(RuleContext context)
        {
            if (Category == "other")
                return true;

            if (expression == null)
            {
                expression = new Expression(ConvertExpression(Text))
                    .ToLambda<RuleContext, bool>();
            }
            return expression(context);
        }

        /// <summary>
        ///   Create a rule from the specified <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="xml">
        ///   The XML representation of a plural rule.
        /// </param>
        /// <returns>
        ///   A new rule.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="xml"/> must be on an "pluralRule" element.
        /// </remarks>
        public static Rule Parse(XPathNavigator xml)
        {
            return Parse(xml.GetAttribute("count", ""), xml.Value);
        }

        /// <summary>
        ///   Create a rule from the specified <see cref="Category"/>
        ///   and rule.
        /// </summary>
        /// <param name="category">
        ///  "zero", "one", "two", "few", "many" or "other".
        /// </param>
        /// <param name="s">
        ///   The <see cref="Text"/> and <see cref="Samples"/>
        /// </param>
        /// <returns>
        ///   A new rule.
        /// </returns>
        public static Rule Parse (string category, string s)
        {
            var rule = new Rule
            {
                Category = category
            };

            var at = s.IndexOf('@');
            if (at >= 0)
            {
                rule.Samples = s.Substring(at).Trim();
                s = s.Substring(0, at);
            }

            rule.Text = s.Trim();

            return rule;
        }

        /// <summary>
        ///   Convert a Unicode plural rule expression into
        ///   a NCalc expression.
        /// </summary>
        /// <param name="s">
        ///   A plural rule expression.
        /// </param>
        /// <returns>
        ///   The NCalc equivalent.
        /// </returns>
        /// <remarks>
        ///   Internal method.
        /// </remarks>
        public static string ConvertExpression(string s)
        {
            // Transform value'..'value to a comma seperated list
            // of values.
            s = rangePattern.Replace(s, (match) =>
            {
                var start = Int32.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var end = Int32.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var values = Enumerable
                    .Range(start, end - start + 1)
                    .Select(i => i.ToString(CultureInfo.InvariantCulture));
                return String.Join(", ", values);
            });

            // Transform equal range to an `in` call.
            s = relationPattern.Replace(s, (match) =>
            {
                var op = match.Groups[2].Value == "=" ? "in" : "not in";
                return $"{op}({match.Groups[1].Value}, {match.Groups[3].Value})";
            });

            return s;
        }
    }
}
