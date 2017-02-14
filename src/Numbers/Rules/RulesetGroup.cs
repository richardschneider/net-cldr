using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   Grouping of rules into a functional set. 
    /// </summary>
    public class RulesetGroup
    {
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
    }
}
