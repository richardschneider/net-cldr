using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Determines the rules for <see cref="SpellingFormatter">formatting</see> 
    ///   a numeric quantity into words.
    /// </summary>
    /// <seealso cref="SpellingOptions"/>
    /// <seealso cref="SpellingFormatter"/>
    public enum SpellingStyle
    {
        /// <summary>
        ///   A number that says how many of something there are, such as 
        ///   one, two, three, four, five in English.
        /// </summary>
        Cardinal = 0,

        /// <summary>
        ///   A number that tells the position of something in a list, such as 
        ///   first, second, third in English.
        /// </summary>
        Ordinal
    }
}
