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
        /// 
        /// </summary>
        public RulesetGroup RulesetGroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Ruleset Ruleset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StringBuilder Text { get; private set; } = new StringBuilder();
    }
}
