using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
{
    /// <summary>
    ///   Representation of numeric value.
    /// </summary>
    public class NumberingSystem
    {
        static ConcurrentDictionary<string, NumberingSystem> Cache = new ConcurrentDictionary<string, NumberingSystem>();

        /// <summary>
        ///   Unique identifier of the numbering system.
        /// </summary>
        /// <value>
        ///   Such as "latn", "arab", "hanidec", etc.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///   The type of numbering system.
        /// </summary>
        /// <value>
        ///   Either "algorithmic" or "numeric".
        /// </value>
        /// <seealso cref="IsNumeric"/>
        /// <seealso cref="IsAlgorithmic"/>
        public string Type { get; set; }

        /// <summary>
        ///  The digits used to represent numbers, in order, starting from zero. 
        /// </summary>
        /// <value>
        ///   Only valid when <see cref="Type"/> equals "numeric".
        /// </value>
        /// <seealso cref="IsNumeric"/>
        public string Digits { get; set; }

        /// <summary>
        ///  The RBNF ruleset to be used for formatting numbers
        /// </summary>
        /// <value>
        ///   Only valid when <see cref="Type"/> equals "algorithmic".
        /// </value>
        /// <seealso cref="IsAlgorithmic"/>
        public object Rules { get; set; }

        /// <summary>
        ///   Determines if the <see cref="Type"/> is numeric.
        /// </summary>
        /// <value>
        ///   <b>true</b> if <see cref="Type"/> equals "numeric"; otherwise, <b>false</b>.
        /// </value>
        /// <remarks>
        ///   Numeric systems are simply a decimal based system that uses a predefined set of 
        ///   <see cref="Digits"/> to represent numbers. 
        /// </remarks>
        public bool IsNumeric { get { return Type == "numeric"; } }

        /// <summary>
        ///   Determines if the <see cref="Type"/> is algorithmic.
        /// </summary>
        /// <value>
        ///   <b>true</b> if <see cref="Type"/> equals "algorithmic"; otherwise, <b>false</b>.
        /// </value>
        /// <remarks>
        ///   Algorithmic systems are complex in nature, since the proper formatting and presentation
        ///   of a numeric quantity is based on some algorithm or set of <see cref="Rules"/>. 
        /// </remarks>
        public bool IsAlgorithmic { get { return Type == "algorithmic"; } }

        /// <summary>
        ///   Creates or reuses a numbering system with the specified identifier.
        /// </summary>
        /// <param name="id">
        ///   A case insensitive string containing the <see cref="Id"/>.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        ///   The <paramref name="id"/> numbering system is not defined.
        /// </exception>
        /// <returns>
        ///   A numbering system for the specified <paramref name="id"/>.
        /// </returns>
        public static NumberingSystem Create(string id)
        {
            id = id.ToLowerInvariant();
            return Cache.GetOrAdd(id, key =>
            {
                var xml = Cldr.Instance
                    .GetDocuments("common/supplemental/numberingSystems.xml")
                    .FirstElement($"supplementalData/numberingSystems/numberingSystem[@id='{key}']");
                return new NumberingSystem
                {
                    Id = xml.GetAttribute("id", ""),
                    Type = xml.GetAttribute("type", ""),
                    Digits = xml.GetAttribute("digits", ""),
                    Rules = xml.GetAttribute("rules", "")
                };
            });
        }

        /// <summary>
        ///   Creates or reuses a numbering system for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <returns>
        ///   A numbering system that is the best for the <paramref name="locale"/>.
        /// </returns>
        /// <remarks>
        ///   The locale identifier can use the "u-nu-XXX" extension to specify a numbering system.
        ///   If the extension's numbering system doesn't exist or is not specified, 
        ///   then the default numbering system for the locale is used.
        /// </remarks>
        public static NumberingSystem Create(Locale locale)
        {
            var possibilities = locale.Id.Extensions
                .Where(x => x.StartsWith("u-nu-"))
                .Select(x => x.Substring(5));
            foreach (var name in possibilities)
            {
                try
                {
                    return NumberingSystem.Create(name);
                }
                catch (KeyNotFoundException)
                {
                    // eat it, will fallbace to default numbering system.
                }
            }

            // Find the default numbering system for the locale.
            var ns = locale
                .ResourceBundle()
                .FirstElement("ldml/numbers/defaultNumberingSystem/text()").Value;

            return NumberingSystem.Create(ns);
        }


    }
}
