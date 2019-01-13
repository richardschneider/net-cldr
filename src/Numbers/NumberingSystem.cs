using Common.Logging;
using Sepia.Globalization.Numbers.Rules;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Representation of numeric value.
    /// </summary>
    public class NumberingSystem
    {
        static ILog log = LogManager.GetLogger(typeof(NumberingSystem));
        static ConcurrentDictionary<string, NumberingSystem> Cache = new ConcurrentDictionary<string, NumberingSystem>();
        static string[] Others = new[] { "native", "finance", "traditio" };
        static object safe = new Object();

        static RulesetGroup numberingSystemRules;
        static RulesetGroup NumberingSystemRules
        {
            get
            {
                if (numberingSystemRules == null)
                {
                    lock (safe)
                    {
                        if (numberingSystemRules == null)
                        {
                            var xml = Cldr.Instance
                                .GetDocuments("common/rbnf/root.xml")
                                .FirstElement("ldml/rbnf/rulesetGrouping[@type='NumberingSystemRules']");
                            numberingSystemRules = RulesetGroup.Parse(xml);
                        }
                    }
                }

                return numberingSystemRules;
            }
        }

        RulesetGroup rulesetGroup;
        Ruleset ruleset;

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
        public string[] Digits { get; set; }

        /// <summary>
        ///  The RBNF ruleset name used for formatting numbers.
        /// </summary>
        /// <value>
        ///   Only valid when <see cref="Type"/> equals "algorithmic".
        /// </value>
        /// <remarks>
        ///   The rules specifier can contain simply a ruleset name, in which case the ruleset is 
        ///   assumed to be found in the rule set grouping "NumberingSystemRules". Alternatively, 
        ///   the specifier can denote a specific locale, ruleset grouping, and ruleset name, 
        ///   separated by slashes.
        /// </remarks>
        /// <seealso cref="IsAlgorithmic"/>
        public string RuleName { get; set; }

        /// <summary>
        ///   Gets the grouping of rules for the numbering systems.
        /// </summary>
        /// <value>
        ///   Only valid when <see cref="Type"/> equals "algorithmic".
        /// </value>
        public RulesetGroup RulesetGroup
        {
            get
            {
                if (rulesetGroup == null)
                {
                    lock (safe)
                    {
                        if (rulesetGroup == null)
                        {
                            if (RuleName.Contains('/'))
                            {
                                var parts = RuleName.Split('/');
                                var locale = Locale.Create(parts[0]);
                                var xml = locale
                                    .ResourceBundle("common/rbnf/")
                                    .FirstElement($"ldml/rbnf/rulesetGrouping[@type='{parts[1]}']");
                                rulesetGroup = RulesetGroup.Parse(xml);
                                ruleset = rulesetGroup.Rulesets[parts[2]];
                            }
                            else
                            {
                                rulesetGroup = NumberingSystemRules;
                            }
                        }
                    }
                }

                return rulesetGroup;
            }
        }

        /// <summary>
        ///   A set of rules for the numbering system.
        /// </summary>
        /// <value>
        ///   Only valid when <see cref="Type"/> equals "algorithmic".
        /// </value>
        public Ruleset Ruleset
        {
            get
            {
                if (ruleset == null)
                {
                    lock (safe)
                    {
                        var group = RulesetGroup;
                        if (ruleset == null)
                        {
                            ruleset = group.Rulesets[RuleName];
                        }
                    }
                }

                return ruleset;
            }
        }

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
        ///   of a numeric quantity is based on some algorithm or set of <see cref="RuleName"/>. 
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
                if (log.IsDebugEnabled)
                    log.DebugFormat("Loading '{0}'", key);
                return new NumberingSystem
                {
                    Id = xml.GetAttribute("id", ""),
                    Type = xml.GetAttribute("type", ""),
                    Digits = GetTextElements(xml.GetAttribute("digits", "")).ToArray(),
                    RuleName = xml.GetAttribute("rules", "")
                };
            });
        }

        static IEnumerable<string> GetTextElements(string s)
        {
            var text = StringInfo.GetTextElementEnumerator(s);
            while (text.MoveNext())
            {
                yield return text.GetTextElement();
            }
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
        ///   <para>
        ///   The <see href="http://unicode.org/reports/tr35/tr35-numbers.html#otherNumberingSystems">other numbering systems</see>
        ///   ("u-nu-native", "u-nu-finance" and "u-nu-traditio") are also allowed.
        ///   </para>
        /// </remarks>
        public static NumberingSystem Create(Locale locale)
        {
            string name;
            if (locale.Id.UnicodeExtension.Keywords.TryGetValue("nu", out name))
            {
                try
                {
                    if (Others.Contains(name))
                    {
                        // Consistency is the hobgoblin of small minds.
                        var other = name == "traditio"
                            ? "traditional"
                            : name;
                        name = locale.Find($"ldml/numbers/otherNumberingSystems/{other}").Value;
                    }
                    return NumberingSystem.Create(name);
                }
                catch (KeyNotFoundException)
                {
                    // eat it, will fallback to default numbering system.
                }
            }

            // Find the default numbering system for the locale.
            var ns = locale.Find("ldml/numbers/defaultNumberingSystem/text()").Value;

            return NumberingSystem.Create(ns);
        }


    }
}
