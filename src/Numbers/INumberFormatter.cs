using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
{
    /// <summary>
    ///   The numbers that can be localised.
    /// </summary>
    public interface INumberFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ToString(long value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ToString(decimal value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ToString(double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        string ToString(long value, string currencyCode);

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        string ToString(double value, string currencyCode);

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        string ToString(decimal value, string currencyCode);
    }
}
