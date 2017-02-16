using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    internal class Substitution
    {
        static Regex SubstitutionRegex = new Regex(@"(?<text>[^\[=→←]*)?(?<token>=|→{2,3}|←←)?(%*(?<desc>.+)=)?(\[(?<opt>.*)\])?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Token;
        public string Text;
        public string Descriptor;
        public Substitution Optional;

        public static IEnumerable<Substitution> Parse(string s)
        {
            for (var match = SubstitutionRegex.Match(s); match.Success; match = match.NextMatch())
            {
                var substitution = new Substitution
                {
                    Token = match.Groups["token"].Value,
                    Text = match.Groups["text"].Value,
                    Descriptor = match.Groups["desc"].Value
                };

                if (match.Groups["opt"].Success)
                {
                    substitution.Optional = Parse(match.Groups["opt"].Value).First();
                }

                // Ignore empty matches; everything is optional in the regex.
                if (!string.IsNullOrEmpty(substitution.Text) || !string.IsNullOrEmpty(substitution.Token))
                    yield return substitution;
            }
        }
    }
}
