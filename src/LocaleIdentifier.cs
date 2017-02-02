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
        const string sepP = @"[-_]";
        const string langP = @"(?<lang>[a-z]{2,3}|[a-z]{5,8})";
        const string scriptP = @"(?<script>[a-z]{4})";
        const string regionP = @"(?<region>[a-z]{2}|\d{3})";
        const string variantP = @"([a-z][\da-z]{4,7}|\d[\da-z]{3})";
        static string extensionP = $"(?<ext>[a-wy-z]({sepP}[\\da-z]{{2,8}})+)";
        static string pattern =
            "^(root" +
                $"|{langP}({sepP}{scriptP})?" +
                $"|{scriptP})" +
            $"({sepP}{regionP})?" +
            $"(?<variants>({sepP}{variantP})*)" +
            $"(({sepP}{extensionP})*)" +
            "$"
            ;
        static Regex idRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        ///   The language subtag.
        /// </summary>
        /// <value>
        ///   ISO 639 code or the empty string.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Language { get; private set; }

        /// <summary>
        ///   The script (writing system) subtag.
        /// </summary>
        /// <value>
        ///   ISO 15924 code or the empty string.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Script { get; private set; }

        /// <summary>
        ///   The region subtag.
        /// </summary>
        /// <value>
        ///   ISO 3166-1 or UN M.49 code or the empty string.
        /// </value>
        /// <remarks>
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Region { get; private set; }

        /// <summary>
        ///   Locale variations.
        /// </summary>
        /// <value>
        ///   A sequence of variant subtags.
        /// </value>
        /// <remarks>
        ///   Variant subtags are used to indicate additional, well-recognized
        ///   variations that define a language or its dialects that are not
        ///   covered by other available subtags.
        ///   
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public IEnumerable<string> Variants { get; private set; }

        /// <summary>
        ///   Locale extensions.
        /// </summary>
        /// <value>
        ///   A sequence of extension subtags.
        /// </value>
        /// <remarks>
        ///   Extensions provide a mechanism for extending language tags for 
        ///   use in various applications.
        ///   
        ///   All subtags are case insensitive but stored in the lower-case form.
        /// </remarks>
        public IEnumerable<string> Extensions { get; private set; }

        /// <inheritdoc/>
        /// <remarks>
        ///   Uses the casing recommendations in [BCP47] for subtag casing. 
        ///   The <see cref="Region"/> subtag is in uppercase, 
        ///   the <see cref="Script"/> subtag is in title case, and all other subtags are 
        ///   in lowercase.
        /// </remarks>
        public override string ToString()
        {
            var tags = new[] {
                Language,
                ToTitleCase(Script),
                Region.ToUpperInvariant()
            }
                .Concat(Variants)
                .Concat(Extensions)
                .Where(tag => tag != String.Empty);
            return String.Join("-", tags);
        }

        string ToTitleCase(string s)
        {
            if (s.Length > 1 &&  'a' <= s[0] && s[0] <= 'z')
                return s[0].ToString().ToUpperInvariant() + s.Substring(1);
            return s;
        }
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
        public static LocaleIdentifier Parse(string s)
        {
            LocaleIdentifier id;
            string message;
            if (TryParse(s, out id, out message))
                return id;

            throw new FormatException(message);
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
            string message;
            return TryParse(s, out result, out message);
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
        /// <param name="message">
        ///   The reason why the parsing failed.
        /// </param>
        /// <returns>
        ///   <b>true</b> if <paramref name="s"/> was parsed successfully; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   A local identifier that refers to <paramref name="s"/>.
        /// </remarks>
        public static bool TryParse(string s, out LocaleIdentifier result, out string message)
        { 
            result = null;
            message = null;

            var match = idRegex.Match(s.ToLowerInvariant());
            if (!match.Success)
            {
                message = $"'{s}' is not a valid locale identifier.";
                return false;
            }

            // Variants cannot be repeated.
            var variants = match.Groups["variants"]
                .Value.Split('-', '_')
                .Where(sv => !string.IsNullOrEmpty(sv))
                .ToArray();
            if (variants.Distinct().Count() != variants.Length)
            {
                message = $"'{s}' is not a valid locale identifier because a variant is duplicated.";
                return false;
            }

            // Extensions cannot be repeated.
            var extensions = new List<string>();
            foreach (Capture capture in match.Groups["ext"].Captures)
            {
                extensions.Add(capture.Value.Replace('_', '-'));
            }
            if (extensions.Distinct().Count() != extensions.Count)
            {
                message = $"'{s}' is not a valid locale identifier because an extension is duplicated.";
                return false;
            }

            result = new LocaleIdentifier
            {
                Language = match.Groups["lang"].Value,
                Script = match.Groups["script"].Value,
                Region = match.Groups["region"].Value,
                Variants = variants,
                Extensions = extensions.ToArray()
            };
            return true;
        }
    }
}
