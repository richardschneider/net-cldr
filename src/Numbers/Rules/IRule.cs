using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    /// <summary>
    ///    A formatting rule.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        ///   Determines if the rule matches the context.
        /// </summary>
        /// <param name="context">
        ///   The context for formatting a rule based number.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the rule matches the <paramref name="context"/>; otherwise, <b>false</b>.
        /// </returns>
        bool Matches(RbnfContext context);

        /// <summary>
        ///   Performs the rule.
        /// </summary>
        /// <param name="context">
        ///   The context for formatting a rule based number.
        /// </param>
        void Fire(RbnfContext context);
    }
}
