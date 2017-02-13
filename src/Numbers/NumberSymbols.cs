using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    /// <summary>
    ///   The localised symbols that are commonly used when formatting numbers in a given locale. 
    /// </summary>
    public class NumberSymbols
    {
        string currencyDecimal;
        string currencyGroup;

        /// <summary>
        ///   Separates the integer and fractional part of the number.
        /// </summary>
        public string Decimal { get; set; }

        /// <summary>
        ///   Separates clusters of integer digits to make large numbers more legible.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        ///   Symbol used to separate numbers in a list intended to represent structured data such as an array.
        /// </summary>
        public string List { get; set; }

        /// <summary>
        ///   Symbol used to indicate a percentage (1/100th) amount.
        /// </summary>
        public string PercentSign { get; set; }

        /// <summary>
        ///   Symbol used to denote negative value.
        /// </summary>
        public string MinusSign { get; set; }

        /// <summary>
        ///   Symbol used to denote positive value.
        /// </summary>
        public string PlusSign { get; set; }

        /// <summary>
        ///   Symbol separating the mantissa and exponent values.
        /// </summary>
        public string Exponential { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuperscriptingExponent { get; set; }

        /// <summary>
        ///   Symbol used to indicate a per-mille (1/1000th) amount.
        /// </summary>
        public string PerMille { get; set; }

        /// <summary>
        ///   The infinity sign.
        /// </summary>
        public string Infinity { get; set; }

        /// <summary>
        ///   The NaN sign.
        /// </summary>
        public string NotANumber { get; set; }

        /// <summary>
        ///   Separates the integer and fractional part of the currency number.
        /// </summary>
        /// <value>Defaults to <see cref="Decimal"/>.</value>
        public string CurrencyDecimal
        {
            get { return currencyDecimal ?? Decimal; }
            set { currencyDecimal = value; }
        }

        /// <summary>
        ///   Separates clusters of integer digits to make large numbers more legible.
        /// </summary>
        /// <value>Defaults to <see cref="Group"/>.</value>
        public string CurrencyGroup
        {
            get { return currencyGroup ?? Group; }
            set { currencyGroup = value; }
        }

        /// <summary>
        ///   Creates the symbols for the specified <see cref="Locale"/>.
        /// </summary>
        /// <param name="locale">
        ///   The locale.
        /// </param>
        /// <returns>
        ///   The symbols that are the best for the <paramref name="locale"/>.
        /// </returns>
        public static NumberSymbols Create(Locale locale)
        {
            var ns = NumberingSystem.Create(locale).Id;
            var path = $"ldml/numbers/symbols[@numberSystem='{ns}']/";
            var symbols = new NumberSymbols
            {
                Decimal = locale.Find(path + "decimal").Value,
                Exponential = locale.Find(path + "exponential").Value,
                Group = locale.Find(path + "group").Value,
                Infinity = locale.Find(path + "infinity").Value,
                List = locale.Find(path + "list").Value,
                MinusSign = locale.Find(path + "minusSign").Value,
                NotANumber = locale.Find(path + "nan").Value,
                PercentSign = locale.Find(path + "percentSign").Value,
                PerMille = locale.Find(path + "perMille").Value,
                PlusSign = locale.Find(path + "plusSign").Value,
                SuperscriptingExponent = locale.Find(path + "superscriptingExponent").Value
            };

            var found = locale.FindOrDefault(path + "currencyDecimal");
            if (found != null)
                symbols.CurrencyDecimal = found.Value;

            found = locale.FindOrDefault(path + "currencyGroup");
            if (found != null)
                symbols.CurrencyGroup = found.Value;

            return symbols;
        }
    }
}
