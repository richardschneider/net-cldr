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
            return context.Number < 0;
        }
    }
}
