using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   The options to apply when <see cref="SpellingFormatter">spelling</see> a number.
    /// </summary>
    public class SpellingOptions
    {
        /// <summary>
        ///   The default spelling options.
        /// </summary>
        public static readonly SpellingOptions Default = new SpellingOptions();

        /// <summary>
        ///   The rules for spelling out a numeric quantity.
        /// </summary>
        /// <value>
        ///   The default value is <see cref="SpellingStyle.Cardinal"/>.
        /// </value>
        public SpellingStyle Style { get; set; } = SpellingStyle.Cardinal;
    }
}
