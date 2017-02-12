using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Globalization.Numbers
{
    class NumericFormatter : NumberFormatter
    {
        static string[] digits = new NumberFormatInfo().NativeDigits;

        public override string ToString(long value)
        {
            return value.ToString(Pattern(), NumberFormatInfo());
        }

        public override string ToString(decimal value)
        {
            return Transform(value.ToString(Pattern(), NumberFormatInfo()));
        }

        public override string ToString(double value)
        {
            return Transform(value.ToString(Pattern(), NumberFormatInfo()));
        }

        NumberFormatInfo NumberFormatInfo()
        {
            var nfi = new NumberFormatInfo
            {
                CurrencyDecimalSeparator = Symbols.CurrencyDecimal,
                CurrencyGroupSeparator = Symbols.CurrencyGroup,
                NaNSymbol = Symbols.NotANumber,
                NegativeSign = Symbols.MinusSign,
                NegativeInfinitySymbol = Symbols.MinusSign + Symbols.Infinity,
                NumberDecimalSeparator = Symbols.Decimal,
                NumberGroupSeparator = Symbols.Group,
                // NativeDigits is NOT used the formatter!!
                //NativeDigits = NumberingSystem.Digits,
                PercentDecimalSeparator = Symbols.Decimal,
                PercentGroupSeparator = Symbols.Group,
                PercentSymbol = Symbols.PercentSign,
                PerMilleSymbol = Symbols.PerMille,
                PositiveInfinitySymbol = Symbols.PlusSign + Symbols.Infinity,
                PositiveSign = Symbols.PlusSign
            };

            return nfi;
        }

        string Pattern()
        {
            if (Options.Style == NumberStyle.Decimal)
            {
                var path = $"ldml/numbers/decimalFormats[@numberSystem='{NumberingSystem.Id}']/decimalFormatLength/decimalFormat/pattern";
                return Locale.Find(path).Value;
            }

            if (Options.Style == NumberStyle.Percent)
            {
                var path = $"ldml/numbers/percentFormats[@numberSystem='{NumberingSystem.Id}']/percentFormatLength/percentFormat/pattern";
                return Locale.Find(path).Value;
            }

            if (Options.Style == NumberStyle.Scientific)
            {
                var path = $"ldml/numbers/scientificFormats[@numberSystem='{NumberingSystem.Id}']/scientificFormatLength/scientificFormat/pattern";
                var pattern = Locale.Find(path).Value;
                return pattern == "#E0" ? "0.0######E0" : pattern;
            }

            throw new NotImplementedException();
        }

        string Transform(string s)
        {
            var sb = new StringBuilder(s);

            // Replace digits with number system digits.
            for (int i = 0; i < 10; ++i)
            {
                sb.Replace(digits[i], NumberingSystem.Digits[i]);
            }

            return sb.ToString();
        }

    }
}
