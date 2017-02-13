using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sepia.Globalization
{
    /// <summary>
    ///   Represents a Unicode BCP 47 U Extension.
    /// </summary>
    /// <seealso href="http://unicode.org/reports/tr35/tr35.html#u_Extension"/>
    public class LocaleExtension
    {
        const string sepP = @"[-_]";
        const string keyP = @"(?<key>[a-z\d][a-z])";
        const string attributeP = @"(?<attr>[a-z\d]{3,8})";
        static string typeP = $"(?<type>[a-z\\d]{{3,8}}({sepP}[a-z\\d]{{3,8}})*)";
        static string keywordP = $"(?<keyword>{keyP}({sepP}{typeP})?)";
        static string pattern = 
            "^u(" +
            $"({sepP}{keywordP})+" +
            $"|({sepP}{attributeP})+({sepP}{keywordP})*" +
            ")$"
            ;
        static Regex extensionRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        ///   An empty locale extension.
        /// </summary>
        /// <value>
        ///   <see cref="Attributes"/> and <see cref="Keywords"/> are an empty sequence.
        /// </value>
        public static LocaleExtension Empty = new LocaleExtension
        {
            Attributes = new string[0],
            Keywords = new Dictionary<string,string>(0)
        };

        /// <summary>
        ///   The attributes.
        /// </summary>
        /// <returns>
        ///   A sequence of attributes.
        /// </returns>
        /// <remarks>
        ///   Currently, no attributes are defined by Unicode.
        /// </remarks>
        public IEnumerable<string> Attributes { get; private set; } 

        /// <summary>
        ///   The two character keyword and type.
        /// </summary>
        /// <value>
        ///   A dictionary whose key is a keyword (such as 'nu' or 'va').
        /// </value>
        /// <remarks>
        ///   If the type is not specified then it is "true".
        /// </remarks>
        public IDictionary<string, string> Keywords { get; set; }

        /// <summary>
        ///   Returns the canonical string representation.
        /// </summary>
        public override string ToString()
        {
            if (Attributes.Count() == 0 && Keywords.Count == 0)
                return "";

            var a = Attributes
                .OrderBy(s => s, StringComparer.InvariantCulture);
            var k = Keywords
                .OrderBy(p => p.Key, StringComparer.InvariantCulture)
                .Select(p => p.Key  + ((p.Value == "true") ? "" : "-" + p.Value)); 

            return "u-" + String.Join("-", a.Concat(k));
        }
        /// <summary>
        ///   Parses the string representation of a Unicode locale extension.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale extension.
        /// </param>
        /// <exception cref="FormatException">
        ///   <paramref name="s"/> is not in the correct format.
        /// </exception>
        /// <returns>
        ///   A locale extension that refers to <paramref name="s"/>.
        /// </returns>
        public static LocaleExtension Parse(string s)
        {
            LocaleExtension extension;
            string message;
            if (TryParse(s, out extension, out message))
                return extension;

            throw new FormatException(message);
        }

        /// <summary>
        ///   Try to parse the string representation of a Unicode locale extension.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale extension.
        /// </param>
        /// <param name="result">
        ///   A locale extension that refers to <paramref name="s"/> or <b>null</b> if the parsing
        ///   failed.
        /// </param>
        /// <returns>
        ///   <b>true</b> if <paramref name="s"/> was parsed successfully; otherwise, <b>false</b>.
        /// </returns>
        public static bool TryParse(string s, out LocaleExtension result)
        {
            string message;
            return TryParse(s, out result, out message);
        }

        /// <summary>
        ///   Try to parse the string representation of a Unicode locale extension.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale extension.
        /// </param>
        /// <param name="result">
        ///   A local extension that refers to <paramref name="s"/> or <b>null</b> if the parsing
        ///   failed.
        /// </param>
        /// <param name="message">
        ///   The reason why the parsing failed.
        /// </param>
        /// <returns>
        ///   <b>true</b> if <paramref name="s"/> was parsed successfully; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   A locale extension that refers to <paramref name="s"/>.
        /// </remarks>
        public static bool TryParse(string s, out LocaleExtension result, out string message)
        {
            result = null;
            message = null;

            var match = extensionRegex.Match(s.ToLowerInvariant());
            if (!match.Success)
            {
                message = $"'{s}' is not a valid locale U extension.";
                return false;
            }

            var attributes = match
                .Groups["attr"]
                .Captures.OfType<Capture>()
                .Select(c => c.Value)
                .ToArray();
            var keywords = match
                .Groups["keyword"]
                .Captures.OfType<Capture>()
                .Select(c => c.Value)
                .ToDictionary(
                    v => v.Substring(0, 2),
                    v => v.Length > 3 ? v.Substring(3) : "true"
                );
            result = new LocaleExtension
            {
                Attributes = attributes,
                Keywords = keywords
            };
            return true;
        }
    }
}
