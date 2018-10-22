using Sepia.Globalization.Numbers.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Localises an ordinal number into its textual representation.
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
        ///   Creates or reuses a spelling formatter for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <returns>
        ///   The spelling formatter that is the best for the <paramref name="locale"/>.
        /// </returns>
        public static SpellingFormatter Create(Locale locale)
        {
            var formatter = new SpellingFormatter
            {
                Locale = locale,
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
            return spelloutRules.Format(value, "spellout-numbering", Locale);
        }

        /// <inheritdoc />
        public string Format(double value)
        {
            return spelloutRules.Format(value, "spellout-numbering", Locale);
        }

    }
}
