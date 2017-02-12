using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
{
    /// <summary>
    ///   Determines the rules for formatting numeric quantities.
    /// </summary>
    /// <seealso cref="NumberOptions"/>
    public enum NumberStyle
    {
        /// <summary>
        ///   The normal locale specific way to format a base 10 number. 
        /// </summary>
        Decimal = 0,

        /// <summary>
        ///   Percentage formatting.
        /// </summary>
        Percent,

        /// <summary>
        ///    Scientific (exponent) formatting.
        /// </summary>
        Scientific,

        /// <summary>
        ///   Standard currency formatting.
        /// </summary>
        CurrencyStandard,

        /// <summary>
        ///  Accounting currncy formatting.
        /// </summary>
        CurrencyAccounting
    }
}
