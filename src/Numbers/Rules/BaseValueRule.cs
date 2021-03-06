﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    class BaseValueRule : Rule
    {
        public decimal LowerLimit;
        public decimal UpperLimit = decimal.MaxValue;
        public int radix = 10;

        public override void Fire(RbnfContext context)
        {
            foreach (var sub in Substitutions)
            {
                Apply(sub, context);
            }

        }

        void Apply(Substitution sub, RbnfContext context)
        {
            var number = context.Number;
            context.Text.Append(sub.Text);
            switch (sub.Token)
            {
                // Divide the number by the rule's divisor and format the remainder.
                case "→→":
                    context.Number = Decimal.Remainder(number, Divisor());
                    context.Ruleset.ApplyRules(context);
                    break;

                // Divide the number by the rule's divisor and format the quotient.
                case "←←":
                    context.Number = Math.Floor(number / Divisor());
                    context.Ruleset.ApplyRules(context);
                    break;

                case "=":
                    // Fallback number formating?
                    if (sub.Descriptor.StartsWith("#") || sub.Descriptor.StartsWith("0"))
                    {
                        var formatter = NumberFormatter.Create(context.Locale);
                        context.Text.Append(formatter.Format(context.Number));
                    }

                    // Else goto the ruleset.
                    else
                    {
                        var ruleset = context.RulesetGroup.Rulesets[sub.Descriptor];
                        ruleset.ApplyRules(context);
                    }
                    break;

                // Divide the number by the rule's divisor and format the remainder
                // with the specified ruleset.
                case "→":
                    context.Number = Decimal.Remainder(number, Divisor());
                    context.RulesetGroup
                        .Rulesets[sub.Descriptor]
                        .ApplyRules(context);
                    break;

                // Divide the number by the rule's divisor and format the quotient
                // with the specified ruleset.
                case "←":
                    context.Number = Math.Floor(number / Divisor());
                    context.RulesetGroup
                        .Rulesets[sub.Descriptor]
                        .ApplyRules(context);
                    break;

                case "":
                    break;

                default:
                    throw new NotSupportedException($"Substitution token '{sub.Token}' is not allowed.");
            }

            if (sub.Optionals != null && (number % Divisor()) != 0)
            {
                foreach (var optional in sub.Optionals)
                {
                    Apply(optional, context);
                }
            }

            context.Number = number;
        }

        /// <summary>
        ///   The highest power of 10 that is less than or equal to the base value.
        /// </summary>
        decimal Divisor()
        {
            int pow;
            if (radix == 10)
            {
                pow = (LowerLimit == 0)
                    ? 0
                    : ((int)Math.Floor(Math.Log10((double)LowerLimit)));
            }
            else
            {
                pow = (LowerLimit == 0)
                    ? 0
                    : ((int)Math.Floor(Math.Log((double)LowerLimit, radix)));
            }
            return (decimal) Math.Pow(radix, pow);
        }

        public override bool Matches(RbnfContext context)
        {
            return LowerLimit <= context.Number && context.Number <= UpperLimit;
        }

    }
}
