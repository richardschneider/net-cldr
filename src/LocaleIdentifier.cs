using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;

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
        ///   The language subtag is case insensitive but stored in the lower-case form.
        /// </remarks>
        public string Language { get; private set; }

        /// <summary>
        ///   The script (writing system) subtag.
        /// </summary>
        /// <value>
        ///   ISO 15924 code or the empty string.
        /// </value>
        /// <remarks>
        ///   The script subtag is case insensitive but stored in the title-case form.
        /// </remarks>
        public string Script { get; private set; }

        /// <summary>
        ///   The region subtag.
        /// </summary>
        /// <value>
        ///   ISO 3166-1 or UN M.49 code or the empty string.
        /// </value>
        /// <remarks>
        ///   The region subtag is case insensitive but stored in the upper-case form.
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
            var tags = new[] { Language, Script, Region }
                .Concat(Variants)
                .Concat(Extensions)
                .Where(tag => tag != String.Empty);
            return String.Join("-", tags);
        }

        /// <summary>
        ///   Get the Unicode Language ID.
        /// </summary>
        /// <returns>
        ///   The unicode language ID, consisting of the <see cref="Language"/>, <see cref="Script"/> and
        ///   <see cref="Region"/> separated by "_".
        /// </returns>
        /// <remarks>
        ///   Uses the casing recommendations in [BCP47] for subtag casing. 
        ///   The <see cref="Region"/> subtag is in uppercase, 
        ///   the <see cref="Script"/> subtag is in title case, and all other subtags are 
        ///   in lowercase.
        /// </remarks>
        public string ToUnicodeLanguage()
        {
            var tags = new[] { Language, Script, Region }
                .Where(tag => tag != String.Empty);
            return String.Join("_", tags);
        }

        /// <summary>
        ///   A new locale with the all the empty subtags filled in
        ///   with a likely value.
        /// </summary>
        /// <returns>
        ///   A new locale with the all subtags filled in.
        /// </returns>
        public LocaleIdentifier MostLikelySubtags()
        {
            var result = (LocaleIdentifier)this.MemberwiseClone();

            // Remove the script code 'Zzzz' and the region code 'ZZ' if they occur.
            if (result.Script == "Zzzz")
                result.Script = String.Empty;
            if (result.Region == "ZZ")
                result.Region = String.Empty;

            // Short cut if all subtags have a value.
            if (result.Language != "" && result.Script != "" && result.Region != "")
                return result;


            // Find the language in likely subtags.
            var keys = new[]
            {
                $"{result.Language}_{result.Script}_{result.Region}",
                $"{result.Language}_{result.Region}",
                $"{result.Language}_{result.Script}",
                $"{result.Language}",
                $"und_{result.Script}",
            }
                .Select(k => k.Replace("__", "_").Trim('_'))
                .Where(k => k != String.Empty)
                .Distinct();

            XPathNavigator likely = null;
            foreach (var key in keys)
            {
                likely = Cldr.Instance
                    .GetDocuments("common/supplemental/likelySubtags.xml")
                    .FirstElementOrDefault($"supplementalData/likelySubtags/likelySubtag[@from='{key}']");
                if (likely != null)
                    break;
            }

            if (likely != null)
            {
                var defaults = LocaleIdentifier.ParseBcp47(likely.GetAttribute("to", ""));
                if (result.Language == "")
                    result.Language = defaults.Language;
                if (result.Script == "")
                    result.Script = defaults.Script;
                if (result.Region == "")
                    result.Region = defaults.Region;
            }

            return result;

        }

        static string ToTitleCase(string s)
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
        /// <remarks>
        ///   The transformation rules for converting a BCP 47 tag into a
        ///   Unicode Locale ID are <b>not</b> applied.
        /// </remarks>
        public static LocaleIdentifier ParseBcp47(string s)
        {
            LocaleIdentifier id;
            string message;
            if (TryParseBcp47(s, out id, out message))
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
            if (TryParseBcp47(s, out result, out message))
            {
                message = result.TransformFromBcp47();
            }
            return message == null;
        }

        /// <summary>
        ///   Tries parsing the string representation of a locale identifier.
        /// </summary>
        /// <param name="s">
        ///   A case insensitive string containing a locale identifier, based on BCP47.
        /// </param>
        /// <param name="result">
        ///   A BCP 47 language identifier that refers to <paramref name="s"/> or <b>null</b> if the parsing
        ///   failed.
        /// </param>
        /// <param name="message">
        ///   The reason why the parsing failed.
        /// </param>
        /// <returns>
        ///   <b>true</b> if <paramref name="s"/> was parsed successfully; otherwise, <b>false</b>.
        /// </returns>
        /// <remarks>
        ///   The transformation rules for converting a BCP 47 tag into a
        ///   Unicode Locale ID are <b>not</b> applied.
        /// </remarks>
        public static bool TryParseBcp47(string s, out LocaleIdentifier result, out string message)
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
                Script = ToTitleCase(match.Groups["script"].Value),
                Region = match.Groups["region"].Value.ToUpperInvariant(),
                Variants = variants,
                Extensions = extensions.ToArray()
            };

            return true;
        }

        /// <summary>
        ///   Convert BCP 47 tag to a valid Unicode locale identifier
        /// </summary>
        string TransformFromBcp47()
        {

            // 1. Canonicalize the language tag (afterwards, there will be no extlang subtag)

            // 2. Replace the BCP 47 primary language subtag "und" with "root" if no script, region, 
            //    or variant subtags are present
            if (Language == "und" && Script == "" && Region == "" && Variants.Count() == 0)
            {
                Language = "root";
            }

            // 3. If the BCP 47 primary language subtag matches the type attribute of a languageAlias 
            //    element in Supplemental Data, replace the language subtag with the replacement value.
            // 3.1 If there are additional subtags in the replacement value, add them to the result, but only 
            //     if there is no corresponding subtag already in the tag.
            var languageAlias = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalMetadata.xml")
                .FirstElementOrDefault($"supplementalData/metadata/alias/languageAlias[@type='{Language}']");
            if (languageAlias != null)
            {
                var replacement = LocaleIdentifier.ParseBcp47(languageAlias.GetAttribute("replacement", ""));
                Language = replacement.Language;
                if (Script == "")
                    Script = replacement.Script;
                if (Region == "")
                    Region = replacement.Region;
            }

            // 4. If the BCP 47 region subtag matches the type attribute of a territoryAlias 
            //    element in Supplemental Data, replace the language subtag with the replacement value, as follows:
            var territoryAlias = Cldr.Instance
                .GetDocuments("common/supplemental/supplementalMetadata.xml")
                .FirstElementOrDefault($"supplementalData/metadata/alias/territoryAlias[@type='{Region}']");
            if (territoryAlias != null)
            {
                var replacements = territoryAlias.GetAttribute("replacement", "").Split(' ');
                //    4.1 If there is a single territory in the replacement, use it.
                var replacementValue = replacements[0];
                //    4.2 If there are multiple territories:
                //        4.2.1 Look up the most likely territory for the base language code(and script, if there is one).
                //        4.2.2 If that likely territory is in the list, use it.
                //        4.2.3 Otherwise, use the first territory in the list.
                if (replacements.Length > 1)
                {
                    var best = Cldr.Instance
                        .GetDocuments("common/supplemental/likelySubtags.xml")
                        .FirstElementOrDefault($"supplementalData/likelySubtags/likelySubtag[@from='{Language}']");
                    if (best != null)
                    {
                        var to = LocaleIdentifier.ParseBcp47(best.GetAttribute("to", ""));
                        if (replacements.Contains(to.Region))
                            replacementValue = to.Region;
                    }
                }
                Region = replacementValue;
            }

            // Verify that the subtags are defined.
            // TODO: Need StringRanges, see https://github.com/richardschneider/net-cldr/issues/2
            if (Language != "" && !Cldr.Instance.IsLanguageDefined(Language))
                return $"Language '{Language}' is not defined.";
            if (Script != "" && !Cldr.Instance.IsScriptDefined(Script))
                return $"Script '{Script}' is not defined.";
            if (Region != "" && !Cldr.Instance.IsRegionDefined(Region))
                return $"Region '{Region}' is not defined.";
            // TODO: variants
            // TODO: U extensions
            // TODO: T extensions

            return null;
        }

    }
}
