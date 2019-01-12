using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Determines the length rules for <see cref="NumberFormatter">formatting</see> numeric quantities.
    /// </summary>
    /// <seealso cref="NumberOptions"/>
    /// <seealso cref="NumberFormatter"/>
    public enum NumberLength
    {
        /// <summary>
        ///   The normal locale specific way to format a base 10 number. 
        /// </summary>
        Default = 0,

        /// <summary>
        ///   The full locale specific wat to format a base 10 number.
        /// </summary>
        /// <remarks>
        ///   Only used with <see cref="NumberStyle.Scientific"/>.
        /// </remarks>
        Full,

        /// <summary>
        ///   The long locale specific way to format a base 10 number; such as '1 thousand' for '1,000'.
        /// </summary>
        Long,

        /// <summary>
        ///   The medium locale specific wat to format a base 10 number.
        /// </summary>
        /// <remarks>
        ///   Only used with <see cref="NumberStyle.Scientific"/>.
        /// </remarks>
        Medium,

        /// <summary>
        ///   The compact locale specific way to format a base 10 number; such as '1K' for '1,000'.
        /// </summary>
        Short
    }
}
