using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   The options to apply when <see cref="NumberFormatter">formatting</see> a number.
    /// </summary>
    public class NumberOptions
    {
        /// <summary>
        ///   The default number options.
        /// </summary>
        /// <value>
        ///   <see cref="Style"/> = Decimal, <see cref="UseGrouping"/> = true.
        /// </value>
        public static readonly NumberOptions Default = new NumberOptions();

        /// <summary>
        ///    The rules for formatting numeric quantities.
        /// </summary>
        /// <value>Default value is <see cref="NumberStyle.Decimal"/>.</value>
        public NumberStyle Style { get; set; } = NumberStyle.Decimal;

        /// <summary>
        ///   The length for formatting numeric quantities.
        /// </summary>
        /// <remarks>
        ///   If the locale does not define a pattern for the number length,
        ///   then the defaul number length is used.
        /// </remarks>
        public NumberLength Length { get; set; } = NumberLength.Default;

        /// <summary>
        ///   Determines if grouping seperators should be used.
        /// </summary>
        /// <value>Defaults to <b>true</b>.</value>
        /// <remarks>
        ///   When <b>true</b>, clusters of integer digits are 
        ///   seperated with the <see cref="NumberSymbols.Group">group symbol</see>.
        /// </remarks>
        public bool UseGrouping { get; set; } = true;
    }
}
