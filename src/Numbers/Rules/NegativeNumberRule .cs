using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    class NegativeNumberRule : Rule
    {
        public override void Fire(RbnfContext context)
        {
            foreach (var sub in Substitutions)
            {
                context.Text.Append(sub.Text);
                switch (sub.Token)
                {
                    case "→→":
                        context.Number = Math.Abs(context.Number);
                        if (context.DoubleNumber.HasValue)
                        {
                            if (double.IsNegativeInfinity(context.DoubleNumber.Value))
                                context.DoubleNumber = double.PositiveInfinity;
                            else
                                context.DoubleNumber = Math.Abs(context.DoubleNumber.Value);
                        }
                        context.Ruleset.ApplyRules(context);
                        break;
                    case "":
                        break;
                    default:
                        throw new NotSupportedException($"Substitution token '{sub.Token}' is not allowed.");
                }
            }
        }

        public override bool Matches(RbnfContext context)
        {
            return context.Number < 0 || (context.DoubleNumber.HasValue && double.IsNegativeInfinity(context.DoubleNumber.Value));
        }
    }
}
