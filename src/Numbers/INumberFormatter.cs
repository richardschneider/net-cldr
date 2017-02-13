using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   The numbers that can be localised.
    /// </summary>
    public interface INumberFormatter
    {
        /// <summary>
        ///   Localises a <see cref="long"/>.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        /// </returns>
        string Format(long value);

        /// <summary>
        ///   Localises a <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        /// </returns>
        string Format(decimal value);

        /// <summary>
        ///   Localises a <see cref="double"/>.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        /// </returns>
        string Format(double value);

        /// <summary>
        ///   Localises a <see cref="long"/> currency.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <param name="currencyCode">
        ///    An <see href="https://en.wikipedia.org/wiki/ISO_4217">ISO 4217</see> currency code.  For example: "CNY", "NZD" or "JPY".
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/> and
        ///   <paramref name="currencyCode"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        ///   <para>
        ///   The number of significant digits is controlled by currency definition.
        ///   </para>
        /// </returns>
        string Format(long value, string currencyCode);

        /// <summary>
        ///   Localises a <see cref="double"/> currency.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <param name="currencyCode">
        ///    An <see href="https://en.wikipedia.org/wiki/ISO_4217">ISO 4217</see> currency code.  For example: "CNY", "NZD" or "JPY".
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/> and
        ///   <paramref name="currencyCode"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        ///   <para>
        ///   The number of significant digits is controlled by currency definition.
        ///   </para>
        /// </returns>
        string Format(double value, string currencyCode);

        /// <summary>
        ///   Localises a <see cref="decimal"/> currency.
        /// </summary>
        /// <param name="value">
        ///  The number to localise.
        /// </param>
        /// <param name="currencyCode">
        ///    An <see href="https://en.wikipedia.org/wiki/ISO_4217">ISO 4217</see> currency code.  For example: "CNY", "NZD" or "JPY".
        /// </param>
        /// <returns>
        ///   The localised string representation of the <paramref name="value"/> and
        ///   <paramref name="currencyCode"/>
        ///   according to the <see cref="Locale"/> and 
        ///   <see cref="NumberOptions">formatting options</see>.
        ///   <para>
        ///   The number of significant digits is controlled by currency definition.
        ///   </para>
        /// </returns>
        string Format(decimal value, string currencyCode);
    }
}
