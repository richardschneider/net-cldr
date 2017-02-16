using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   A specific rule set for the number formatter.
    /// </summary>
    public class Ruleset
    {
        /// <summary>
        ///   The type name of the rule set.
        /// </summary>
        /// <value>
        ///    Any non-empty string.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        ///   The accessibility of the rule set.
        /// </summary>
        /// <value>
        ///    One of "public", "private" or "".  When empty, "public" is assumed.
        /// </value>
        public string Access { get; set; }

        /// <summary>
        ///   Determines if the rule set has public access.
        /// </summary>
        /// <value>
        ///   <b>true</b> if <see cref="Access"/> is "public" or empty; otherwise, <b>false</b>.
        /// </value>
        public bool IsPublic
        {
            get
            {
                return Access == "public" || string.IsNullOrEmpty(Access);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ApplyRules(RbnfContext context)
        {
            var previous = context.Ruleset;
            context.Ruleset = this;
            var rule = Rules.FirstOrDefault(r => r.Matches(context));
            if (rule != null)
                rule.Fire(context);
            context.Ruleset = previous;
        }

        /// <summary>
        ///   Formatting rules.
        /// </summary>
        /// <value>
        ///   A sequence of formatting rules.
        /// </value>
        public ICollection<IRule> Rules { get; set; }

        /// <summary>
        ///   Create a rule set from the specified <see cref="XPathNavigator"/>.
        /// </summary>
        /// <param name="xml">
        ///   The XML representation of a rule set.
        /// </param>
        /// <returns>
        ///   A new rule set.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="xml"/> must be on an "ruleset" element.
        /// </remarks>
        public static Ruleset Parse(XPathNavigator xml)
        {
            var ruleset = new Ruleset
            {
                Type = xml.GetAttribute("type", ""),
                Access = xml.GetAttribute("access", ""),
            };

            var rules = new List<IRule>();
            var children = xml.SelectChildren("rbnfrule", "");
            while (children.MoveNext())
            {
                var rule = Rule.Parse(children.Current);
                rules.Add(rule);
            }
            ruleset.Rules = rules;

            // Need to set upper limit of a base value rule.
            var bvrs = rules.OfType<BaseValueRule>().ToArray();
            for (int i = 1; i < bvrs.Length; ++i)
            {
                bvrs[i - 1].UpperLimit = bvrs[i].LowerLimit - 1;
            }
            return ruleset;
        }

    }
}
