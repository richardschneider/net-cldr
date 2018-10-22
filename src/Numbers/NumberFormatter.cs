using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   Localises a number.
    /// </summary>
    public abstract class NumberFormatter : INumberFormatter
    {
        /// <summary>
        ///   The options to apply when formating a number.
        /// </summary>
        public NumberOptions Options { get; set; }

        /// <summary>
        ///   The source for localisation information.
        /// </summary>
        public Locale Locale { get; set; }

        /// <summary>
        ///   The localised numbering system to use.
        /// </summary>
        public NumberingSystem NumberingSystem { get; set; }

        /// <summary>
        ///   The localised symbols to use.
        /// </summary>
        public NumberSymbols Symbols { get; set; }

        /// <summary>
        ///   Resolve any bindings.
        /// </summary>
        /// <remarks>
        ///   This called when all the properties are set for the formatter.  Derived classes
        ///   can then load any extra data that is required.
        /// </remarks>
        public virtual void Resolve()
        {
        }

        /// <summary>
        ///   Creates or reuses a number formatter for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <param name="options">
        ///   The options to apply when formating a number.
        /// </param>
        /// <returns>
        ///   The formatter that is the best for the <paramref name="locale"/>.
        /// </returns>
        public static INumberFormatter Create(Locale locale, NumberOptions options = null)
        {
            var numberingSystem = NumberingSystem.Create(locale);
            NumberFormatter formatter;
            if (numberingSystem.IsNumeric)
            {
                formatter = new NumericFormatter
                {
                    Locale = locale,
                    NumberingSystem = numberingSystem,
                    Symbols = NumberSymbols.Create(locale),
                    Options = options ?? NumberOptions.Default
                };
            }

            else // Must be Algorithmic
            {
                formatter = new AlgorithmicFormatter
                {
                    Locale = locale,
                    NumberingSystem = numberingSystem,
                    Options = options ?? NumberOptions.Default
                };
            }

            formatter.Resolve();
            return formatter;
       }

        /// <inheritdoc />
        public abstract string Format(long value);

        /// <inheritdoc />
        public abstract string Format(decimal value);

        /// <inheritdoc />
        public abstract string Format(double value);

        /// <inheritdoc />
        public abstract string Format(long value, string currencyCode);

        /// <inheritdoc />
        public abstract string Format(decimal value, string currencyCode);

        /// <inheritdoc />
        public abstract string Format(double value, string currencyCode);
    }
}
