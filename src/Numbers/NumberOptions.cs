using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
{
    /// <summary>
    ///   The options to apply when formating a number.
    /// </summary>
    public class NumberOptions
    {
        /// <summary>
        ///    The rules for formatting numeric quantities.
        /// </summary>
        /// <value>Default value is <see cref="NumberStyle.Decimal"/>.</value>
        public NumberStyle Style { get; set; } = NumberStyle.Decimal;

        /// <summary>
        ///   Determines if grouping seperators should be used.
        /// </summary>
        /// <value>Defaults to <b>true</b>.</value>
        /// <remarks>
        ///   When <b>true</b>, clusters of integer digits are 
        ///   seperated with the <see cref="NumberSymbols.Group">group symbol</see>.
        /// </remarks>
        public bool useGrouping { get; set; } = true;
    }
}
