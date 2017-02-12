using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
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
            if (numberingSystem.IsNumeric)
            {
                return new NumericFormatter
                {
                    Locale = locale,
                    NumberingSystem = numberingSystem,
                    Symbols = NumberSymbols.Create(locale),
                    Options = options ?? new NumberOptions()
                };
            }

            else // Must be Algorithmic
                throw new NotImplementedException($"Algorithmic number systems, such as '{numberingSystem.Id}', are not yet implemented.");
       }

        /// <inheritdoc />
        public abstract string ToString(long value);

        /// <inheritdoc />
        public abstract string ToString(decimal value);

        /// <inheritdoc />
        public abstract string ToString(double value);
    }
}
