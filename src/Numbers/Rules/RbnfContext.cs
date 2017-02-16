using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///   The context for formatting a rule based number.
    /// </summary>
    public class RbnfContext
    {
        /// <summary>
        ///   The group of rule sets to use.
        /// </summary>
        public RulesetGroup RulesetGroup { get; set; }

        /// <summary>
        ///   The current rule set.
        /// </summary>
        public Ruleset Ruleset { get; set; }

        /// <summary>
        ///  The number being processed.
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        ///   Internal use.
        /// </summary>
        internal double? DoubleNumber { get; set; }

        /// <summary>
        ///   The generated format of the number.
        /// </summary>
        public StringBuilder Text { get; private set; } = new StringBuilder();
    }
}
