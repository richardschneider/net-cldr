using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
