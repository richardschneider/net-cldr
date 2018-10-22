using Sepia.Globalization.Numbers.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Localises a number into its words.
    /// </summary>
    /// <remarks>
    ///    In English the number 321 is represented as 'three hundred twenty-one'. 
    /// </remarks>
    public class SpellingFormatter
    {
        RulesetGroup spelloutRules;

        /// <summary>
        ///   The source for localisation information.
        /// </summary>
        public Locale Locale { get; set; }

        /// <summary>
        ///   The options to apply when spelling a number.
        /// </summary>
        public SpellingOptions Options { get; set; }

        /// <summary>
        ///   Creates or reuses a spelling formatter for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <param name="options">
        ///   The options to apply when spelling out a number.
        /// </param>
        /// <returns>
        ///   The spelling formatter that is the best for the <paramref name="locale"/>.
        /// </returns>
        public static SpellingFormatter Create(Locale locale, SpellingOptions options = null)
        {
            var formatter = new SpellingFormatter
            {
                Locale = locale,
                Options = options ?? SpellingOptions.Default
            };

            var xml = locale
                .ResourceBundle("common/rbnf/")
                .FirstElement("ldml/rbnf/rulesetGrouping");
            formatter.spelloutRules = RulesetGroup.Parse(xml);

            return formatter;
        }

        /// <inheritdoc />
        public string Format(long value)
        {
            return Format(Convert.ToDecimal(value));
        }

        /// <inheritdoc />
        public string Format(decimal value)
        {
            return spelloutRules.Format(value, RuleType(), Locale);
        }

        /// <inheritdoc />
        public string Format(double value)
        {
            return spelloutRules.Format(value, RuleType(), Locale);
        }

        string RuleType()
        {
            string ruleName;
            switch (Options.Style)
            {
                case SpellingStyle.Ordinal:
                    ruleName = "spellout-ordinal";
                    break;

                case SpellingStyle.Cardinal:
                default:
                    ruleName = "spellout-numbering";
                    break;
            }

            // TODO: verbose and other variants
            return ruleName;
        }
    }
}
