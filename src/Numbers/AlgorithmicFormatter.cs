using Sepia.Globalization.Numbers.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers
{
    class AlgorithmicFormatter : NumberFormatter
    {
        public override string Format(long value)
        {
            return Format((decimal)value);
        }

        public override string Format(decimal value)
        {
            return NumberingSystem.RulesetGroup.Format(value, NumberingSystem.Ruleset.Type);
        }

        public override string Format(double value)
        {
            throw new NotImplementedException();
        }

        public override string Format(long value, string currencyCode)
        {
            throw new NotImplementedException();
        }

        public override string Format(decimal value, string currencyCode)
        {
            throw new NotImplementedException();
        }

        public override string Format(double value, string currencyCode)
        {
            throw new NotImplementedException();
        }
    }
}
