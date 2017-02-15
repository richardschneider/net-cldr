using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   Grouping of rules into a functional set. 
    /// </summary>
    public class RulesetGroup
    {
        static ILog log = LogManager.GetLogger(typeof(RulesetGroup));

        /// <summary>
        ///   The type name of the group.
        /// </summary>
        /// <value>
        ///    The valid types of rule set groupings are "SpelloutRules", "OrdinalRules", and "NumberingSystemRules".
        /// </value>
        public string Type { get; set; }

        /// <summary>
        ///   The rule set(s) for this group.
        /// </summary>
        /// <value>
        ///   A dictionary whose key is the rule set type name.
        /// </value>
        public IDictionary<string, Ruleset> Rulesets = new Dictionary<string, Ruleset>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public string Format(decimal value, string style)
        {
            var ctx = new RbnfContext
            {
                Number = value,
                RulesetGroup = this
            };
            Rulesets[style].ApplyRules(ctx);

            return ctx.Text.ToString();
        }

        /// <summary>
        ///   Create a rule set group from the specified <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="xml">
        ///   The XML representation of a rule set group.
        /// </param>
        /// <returns>
        ///   A new rule set group.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="xml"/> must be on an "rulesetGrouping" element.
        /// </remarks>
        public static RulesetGroup Parse(XPathNavigator xml)
        {
            if (log.IsDebugEnabled)
                log.Debug("Parsing RBNF from " + xml.BaseURI);

            var group = new RulesetGroup
            {
                Type = xml.GetAttribute("type", ""),
            };

            var rulesets = new Dictionary<string, Ruleset>();
            var children = xml.SelectChildren("ruleset", "");
            while (children.MoveNext())
            {
                var ruleset = Ruleset.Parse(children.Current);
                rulesets.Add(ruleset.Type, ruleset);
            }
            group.Rulesets = rulesets;

            return group;
        }
    }
}
