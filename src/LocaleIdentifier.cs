using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Makaretu.Globalization
{
    /// <summary>
    ///   Identifies a set of preferences that tend to be shared across significant swaths of the world.
    /// </summary>
    /// <remarks>
    ///   An identifier is based on <see href="https://www.rfc-editor.org/rfc/bcp/bcp47.txt">BCP47</see>
    ///   and <see href="http://www.unicode.org/reports/tr35/#Unicode_locale_identifier">Unicode TR 35</see> 
    ///   for distinguishing among languages, locales, regions, currencies, 
    ///   time zones, transforms, and so on. 
    /// </remarks>
    public class LocaleIdentifier
    {
        const string langP = @"(?<lang>[a-zA-z]{2,3}|[a-zA-Z]{5,8})";
        const string scriptP = @"(?<script>[a-zA-z]{4})";
        const string regionP = @"(?<region>[a-zA-z]{2}|\d{3})";
        const string sepP = @"[-_]";
        static string pattern =
            "^(root" +
                $"|{langP}({sepP}{scriptP})?" +
                $"|{scriptP})" +
            $"({sepP}{regionP})?" +
            "$"
            ;
        static Regex idRegex = new Regex(pattern, RegexOptions.Compiled);

        /// <summary>
        ///   The language subtag.
        /// </summary>
        /// <value>
        ///   ISO 639 code.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Language { get; private set; }

        /// <summary>
        ///   The script (writing system) subtag.
        /// </summary>
        /// <value>
        ///   ISO 15924 code.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Script { get; private set; }

        /// <summary>
        ///   The region subtag.
        /// </summary>
        /// <value>
        ///   ISO 3166-1 or UN M.49 code.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Region { get; private set; }

        /// <summary>
        ///   Parses the string representation of a locale identifier to a LanguageIdentifier.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale identifier, based on BCP47.
        /// </param>
        /// <exception cref="FormatException">
        ///   <paramref name="s"/> is not in the correct format.
        /// </exception>
        /// <returns>
        ///   A local identifier that refers to <paramref name="s"/>.
        /// </returns>
        /// <seealso cref="TryParse"/>
        public static LocaleIdentifier Parse(string s)
        {
            LocaleIdentifier id;
            if (TryParse(s, out id))
                return id;

            throw new FormatException($"'{s}' is not a valid locale identifier.");
        }

        /// <summary>
        ///   Tries parsing the string representation of a locale identifier.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale identifier, based on BCP47.
        /// </param>
        /// <param name="result">
        ///   A local identifier that refers to <paramref name="s"/> or <b>null</b> if the parsing
        ///   failed.
        /// </param>
        /// <returns>
        ///   <b>true</b> if <paramref name="s"/> was parsed successfully; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   A local identifier that refers to <paramref name="s"/>.
        /// </remarks>
        public static bool TryParse(string s, out LocaleIdentifier result)
        {
            var match = idRegex.Match(s.ToLowerInvariant());
            if (!match.Success)
            {
                result = null;
                return false;
            }

            result = new LocaleIdentifier
            {
                Language = match.Groups["lang"].Value,
                Script = match.Groups["script"].Value,
                Region = match.Groups["region"].Value
            };
            return true;
        }
    }
}
