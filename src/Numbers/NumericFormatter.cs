using Sepia.Globalization.Plurals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    class NumericFormatter : NumberFormatter
    {
        static string[] digits = new NumberFormatInfo().NativeDigits;
        static Regex significantDigitsPattern = new Regex(@"\.0+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        static Regex groupingPattern = new Regex(@"[\#0]+[,\.]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public override string Format(long value)
        {
            var pattern = FindPattern(value);
            if (pattern.NumberNeedsAdjusting())
            {
                value = (long) pattern.Adjust(value);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            var nfi = NumberInfo(null, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), null, pattern.FormatString, nfi);
        }

        public override string Format(decimal value)
        {
            var pattern = FindPattern(value);
            if (pattern.NumberNeedsAdjusting())
            {
                value = pattern.Adjust(value);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10((double)Math.Abs(value))) + 1);
            var nfi = NumberInfo(null, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), null, pattern.FormatString, nfi);
        }

        public override string Format(double value)
        {
            decimal dvalue = Double.IsInfinity(value) || Double.IsNaN(value)
                ? 0
                : (decimal)value;
            var pattern = FindPattern(dvalue);
            if (pattern.NumberNeedsAdjusting())
            {
                value = (double)pattern.Adjust(dvalue);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            var nfi = NumberInfo(null, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), null, pattern.FormatString, nfi);
        }

        public override string Format(long value, string currencyCode)
        {
            var pattern = FindPattern(value);
            if (pattern.NumberNeedsAdjusting())
            {
                value = (long)pattern.Adjust(value);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            var nfi = NumberInfo(currencyCode, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), currencyCode, pattern.FormatString, nfi);
        }

        public override string Format(decimal value, string currencyCode)
        {
            var pattern = FindPattern(value);
            if (pattern.NumberNeedsAdjusting())
            {
                value = pattern.Adjust(value);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10((double)Math.Abs(value))) + 1);
            var nfi = NumberInfo(currencyCode, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), currencyCode, pattern.FormatString, nfi);
        }

        public override string Format(double value, string currencyCode)
        {
            decimal dvalue = Double.IsInfinity(value) || Double.IsNaN(value)
                ? 0
                : (decimal)value;
            var pattern = FindPattern(dvalue);
            if (pattern.NumberNeedsAdjusting())
            {
                value = (double)pattern.Adjust(dvalue);
            }
            int nDigits = (value == 0) ? 1 : ((int)Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            var nfi = NumberInfo(currencyCode, nDigits, pattern);
            return Transform(value.ToString(pattern.FormatString, nfi), currencyCode, pattern.FormatString, nfi);
        }


        NumberFormatInfo NumberInfo(string currencyCode, int nDigits, NumberPattern pattern)
        {
            var nfi = new NumberFormatInfo
            {
                NaNSymbol = Symbols.NotANumber,
                NegativeSign = Symbols.MinusSign,
                NegativeInfinitySymbol = Symbols.MinusSign + Symbols.Infinity,
                NumberDecimalSeparator = Symbols.Decimal,
                NumberGroupSeparator = Symbols.Group,
                // NativeDigits is NOT by used the C# formatter!!
                //NativeDigits = NumberingSystem.Digits,
                PercentDecimalSeparator = Symbols.Decimal,
                PercentSymbol = Symbols.PercentSign,
                PerMilleSymbol = Symbols.PerMille,
                PositiveInfinitySymbol = Symbols.Infinity,
                PositiveSign = Symbols.PlusSign
            };

            if (Options.Style == NumberStyle.Scientific && pattern.FormatString == "#E0")
            {
                pattern.FormatString = "0.0######E0";
            }

            else if (Options.Style == NumberStyle.CurrencyStandard || Options.Style == NumberStyle.CurrencyAccounting)
            {
                nfi.NumberDecimalSeparator = Symbols.CurrencyDecimal;
                nfi.NumberGroupSeparator = Symbols.CurrencyGroup;

                // Apply currency decimal places
                if (currencyCode == null)
                    currencyCode = Locale.CurrencyCode;
                var digits = Cldr.Instance
                    .GetDocuments("common/supplemental/supplementalData.xml")
                    .FirstElementOrDefault($"supplementalData/currencyData/fractions/info[@iso4217='{currencyCode}']/@digits");
                if (digits != null)
                {
                    int significantDigits = Int32.Parse(digits.Value);
                    pattern.FormatString = significantDigitsPattern.Replace(pattern.FormatString, "." + new String('0', significantDigits));
                }
            }

            // Grouping of digits.
            var useGrouping = Options.UseGrouping;
            if (useGrouping)
            {
                // Grouping digits, "#,##,##0" => [3, 2, 1]
                var parts = pattern.FormatString.Split(';');
                nfi.NumberGroupSizes = groupingPattern
                    .Matches(parts[0])
                    .Cast<Match>()
                    .Skip(1)
                    .Reverse()
                    .Select(m => m.Length - 1)
                    .DefaultIfEmpty(3)
                    .ToArray();

                // Don't group if min grouping digits is not met.
                var minElement = Locale.FindOrDefault("ldml/numbers/minimumGroupingDigits");
                if (minElement != null)
                {
                    var minDigits = Int32.Parse(minElement.Value) + nfi.NumberGroupSizes[0];
                    useGrouping = nDigits > minDigits;
                }
            }

            if (!useGrouping)
            {
                nfi.NumberGroupSeparator = "";
            }
            nfi.PercentGroupSizes = nfi.NumberGroupSizes;
            nfi.PercentGroupSeparator = nfi.NumberGroupSeparator;

            return nfi;
        }

        NumberPattern FindPattern(decimal value, NumberLength? nl = null)
        {
            if (!nl.HasValue)
            {
                nl = Options.Length;
            }
            value = Math.Abs(value);

            // Determine the format length type.
            var flt = nl == NumberLength.Default
                ? "[not(@type)]"
                : $"[@type='{nl.ToString().ToLowerInvariant()}']";

            // Determine the path in the ldml to the pattern(s)
            var path = String.Empty;
            if (Options.Style == NumberStyle.Decimal)
            {
                path = $"ldml/numbers/decimalFormats[@numberSystem='{NumberingSystem.Id}']/decimalFormatLength{flt}/decimalFormat";
            }
            else if (Options.Style == NumberStyle.Percent)
            {
                path = $"ldml/numbers/percentFormats[@numberSystem='{NumberingSystem.Id}']/percentFormatLength{flt}/percentFormat";
            }
            else if (Options.Style == NumberStyle.Scientific)
            {
                path = $"ldml/numbers/scientificFormats[@numberSystem='{NumberingSystem.Id}']/scientificFormatLength{flt}/scientificFormat";
            }
            else if (Options.Style == NumberStyle.CurrencyStandard)
            {
                path = $"ldml/numbers/currencyFormats[@numberSystem='{NumberingSystem.Id}']/currencyFormatLength{flt}/currencyFormat[@type='standard']";
            }
            else if (Options.Style == NumberStyle.CurrencyAccounting)
            {
                path = $"ldml/numbers/currencyFormats[@numberSystem='{NumberingSystem.Id}']/currencyFormatLength{flt}/currencyFormat[@type='accounting']";
            }
            else
            {
                throw new NotImplementedException($"Unknown NumberStyle '{Options.Style}'.");
            }

            var xml = Locale.FindOrDefault(path);

            // Fall back to default number length;
            if (xml == null && nl != NumberLength.Default)
            {
                return FindPattern(value, NumberLength.Default);
            }

            // Should not happen.
            if (xml == null)
            {
                throw new KeyNotFoundException($"Cannot find CLDR '{path}'.");
            }

            // Get the best pattern.
            var category = Plural.Create(Locale).Category(value);
            NumberPattern best = null;
            NumberPattern previous = null;
            var pattern = xml.SelectChildren("pattern", "");
            while (pattern.MoveNext())
            {
                var p = NumberPattern.Parse(pattern.Current);

                // If not a value range, then this is the best pattern.
                if (!p.MinValue.HasValue)
                {
                    best = p;
                    break;
                }

                // Only consider a pattern with the correct count.
                if (p.Count != category)
                {
                    continue;
                }

                // Get closest value in the range.
                if (p.MinValue.Value > value)
                {
                    best = previous;
                    break;
                }

                previous = p;
            }
            best = best ?? previous ?? new NumberPattern { FormatString = "0" };

            return best;
        }

        string Transform(string s, string currencyCode, string pattern, NumberFormatInfo nfi)
        {
            var sb = new StringBuilder(s);

            // Replace digits with number system digits.
            for (int i = 0; i < 10; ++i)
            {
                sb.Replace(digits[i], NumberingSystem.Digits[i]);
            }

            // Replace currency symbol
            if (s.Contains("¤"))
            {
                if (currencyCode == null)
                {
                    currencyCode = Locale.CurrencyCode;
                }

                if (s.Contains("¤¤¤¤¤"))
                {
                    throw new NotImplementedException();
                }
                else if (s.Contains("¤¤¤¤"))
                {
                    throw new NotImplementedException();
                }
                else if (s.Contains("¤¤¤"))
                {
                    // Currency display name
                    throw new NotImplementedException();
                }
                else if (s.Contains("¤¤"))
                {
                    // ISO currency symbol
                    sb.Replace("¤¤", currencyCode);
                }
                else if (s.Contains("¤"))
                {
                    // Standard currency symbol
                    var symbol = $"ldml/numbers/currencies/currency[@type='{currencyCode}']/symbol";
                    sb.Replace("¤", Locale.Find(symbol).Value);
                }
            }

            return sb.ToString();
        }

    }
}
