using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   A <see cref="Rule"/> action.
    /// </summary>
    public class Substitution
    {
        //static Regex SubstitutionRegex = new Regex(@"(?<text>[^\[=→←]*)?(?<token>=|→{2,3}|←←)?(%*(?<desc>.+)=)?(\[(?<opt>.*)\])?", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        static string textP = @"(?<text>[^\[=→←]+)";
        static string tokenP = @"(?<token>→{2,3}|←←)";
//        static string descP = @"(%*(?<desc>.+)=)";
        static string descP = @"(?<desc>[=→←][^=→←]+[=→←])";
        static string optP = @"(\[(?<opt>[^\]]+)\])";
//        static string pattern = $"{descP}|({textP}?{tokenP}?{optP})";
        static string pattern = $"{descP}|{textP}|{tokenP}|{optP}";
        static Regex SubstitutionRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        ///   An operation to perform.
        /// </summary>
        public string Token;

        /// <summary>
        ///   Text to add to the output.
        /// </summary>
        public string Text;

        /// <summary>
        ///   Another ruleset to use.
        /// </summary>
        public string Descriptor;

        /// <summary>
        ///   Actions to uses if the number is not a multiple of 10.
        /// </summary>
        public Substitution[] Optionals;

        /// <summary>
        ///   Parses a rule body.
        /// </summary>
        /// <param name="s">
        ///   The text representation of action(s) to perform.
        /// </param>
        /// <returns>
        ///   A sequence of actions to perform.
        /// </returns>
        public static IEnumerable<Substitution> Parse(string s)
        {
            for (var match = SubstitutionRegex.Match(s); match.Success; match = match.NextMatch())
            {
                var substitution = new Substitution
                {
                    Token = match.Groups["token"].Value,
                    Text = match.Groups["text"].Value,
                    Descriptor = ""
                };

                if (match.Groups["desc"].Success)
                {
                    var desc = match.Groups["desc"].Value;
                    substitution.Token = desc.Substring(0, 1);
                    desc = desc.Substring(1, desc.Length - 2);
                    substitution.Descriptor = desc
                        .TrimStart('%');
                }

                if (match.Groups["opt"].Success)
                {
                    var opt = match.Groups["opt"].Value;
                    substitution.Optionals = Parse(opt).ToArray();
                }

                // Ignore empty matches; everything is optional in the regex.
                //if (!string.IsNullOrEmpty(substitution.Text) || !string.IsNullOrEmpty(substitution.Token))
                    yield return substitution;
            }
        }
    }
}
