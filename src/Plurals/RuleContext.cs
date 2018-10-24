using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Plurals
{
    /// <summary>
    ///   The context to an evaulate a <see cref="Rule"/>.
    /// </summary>
    /// <seealso href="http://unicode.org/reports/tr35/tr35-numbers.html#Operands"/>
    public class RuleContext
    {
        /// <summary>
        ///   Absolute value of the source number.
        /// </summary>
        public decimal n;

        /// <summary>
        ///   Integer digits of n.
        /// </summary>
        public decimal i;

        /// <summary>
        ///   Number of visible fraction digits in n, with trailing zeros.
        /// </summary>
        public int v;

        /// <summary>
        ///   Number of visible fraction digits in n, without trailing zeros.
        /// </summary>
        public int w;

        /// <summary>
        ///   Visible fractional digits in n, with trailing zeros.
        /// </summary>
        public decimal f;

        /// <summary>
        ///   Visible fractional digits in n, without trailing zeros.
        /// </summary>
        public decimal t;

        /// <summary>
        ///   Creates a <see cref="RuleContext"/> for a specific number.
        /// </summary>
        /// <param name="value">
        ///   The specific number.
        /// </param>
        /// <returns>
        ///   A new context.
        /// </returns>
        public static RuleContext Create(decimal value)
        {
            var s = value.ToString(CultureInfo.InvariantCulture);
            var dot = s.IndexOf('.');
            var context = new RuleContext
            {
                n = Math.Abs(value),
                i = Math.Floor(value),
            };
            if (dot >= 0)
            {
                context.w = s.Length - dot - 1;
                context.t = decimal.Parse(s.Substring(dot + 1));
            }
            context.v = context.w;
            context.f = context.t;
            return context;
        }
    }

}
