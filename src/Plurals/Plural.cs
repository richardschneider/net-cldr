using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Plurals
{
    /// <summary>
    ///   Manages the plural informaton.
    /// </summary>
    public class Plural
    {
        List<Rule> Rules;

        /// <summary>
        ///   The source for localisation information.
        /// </summary>
        public Locale Locale { get; set; }

        /// <summary>
        ///   Gets the plural category of the specified number.
        /// </summary>
        /// <param name="value">
        ///   The number to examine.
        /// </param>
        /// <returns>
        ///  "zero", "one", "two", "few", "many" or "other".
        /// </returns>
        public string Category(decimal value)
        {
            var context = RuleContext.Create(value);
            var match = Rules.FirstOrDefault(r => r.Matches(context));
            return match == null
                ? "other"
                : match.Category;
        }

        /// <summary>
        ///   Creates or reuses plural informmation for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <returns>
        ///   The best for the <paramref name="locale"/>.
        /// </returns>
        public static Plural Create(Locale locale)
        {
            var plural = new Plural
            {
                Locale = locale
            };
            plural.LoadRules();

            return plural;
        }

        void LoadRules()
        {
            var lang = Locale.Id.Language.ToLowerInvariant();
            Rules = Cldr.Instance
                .GetDocuments("common/supplemental/plurals.xml")
                .Elements($"supplementalData/plurals[@type='cardinal']/pluralRules[contains(@locales, '{lang}')]/pluralRule")
                .Select(e => Rule.Parse(e))
                .ToList();
        }
    }
}
