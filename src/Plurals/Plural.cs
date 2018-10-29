using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Plurals
{
    /// <summary>
    ///   Manages the plural information.
    /// </summary>
    public class Plural
    {
        /// <summary>
        ///   A cache of Plural.  The key is the language code, NOT the
        ///   locale ID.
        /// </summary>
        static ConcurrentDictionary<string, Plural> Cache = new ConcurrentDictionary<string, Plural>();

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
        /// <remarks>
        ///   The category is only a mnemonic; the name does noy necessarily imply 
        ///   the exact contents of the category. For example, for both English and French the
        ///   number 1 has the category "one" (singular). In English, every other number has a
        ///   plural form, and is given the category "other". French is similar, except that 
        ///   the number 0 has the category "one" and not "other" or "zero", 
        ///   because the form of units qualified by 0 is also singular.
        /// </remarks>
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
            var lang = locale.Id.Language.ToLowerInvariant();
            return Cache.GetOrAdd(lang, key =>
            {
                var plural = new Plural
                {
                    Locale = locale
                };
                plural.LoadRules();

                return plural;
            });
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
