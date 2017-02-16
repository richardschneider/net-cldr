using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    class ImproperFractionRule : Rule
    {
        public override void Fire(RbnfContext context)
        {
            var number = context.Number;
            foreach (var sub in Substitutions)
            {
                context.Text.Append(sub.Text);
                switch (sub.Token)
                {
                    case "←←":
                        context.Number = Math.Truncate(number);
                        context.Ruleset.ApplyRules(context);
                        break;
                    case "→→":
                        {
                            var sep = context.Text[context.Text.Length - 1];
                            var x = number - Math.Truncate(number);
                            while (x != 0)
                            {
                                if (sep != context.Text[context.Text.Length - 1])
                                    context.Text.Append(sep);
                                x = x * 10;
                                context.Number = Math.Truncate(x);
                                context.Ruleset.ApplyRules(context);
                                x = x - Math.Truncate(x);
                            }
                        }
                        break;
                    case "→→→":
                        {
                            var x = number - Math.Truncate(number);
                            while (x != 0)
                            {
                                x = x * 10;
                                context.Number = Math.Truncate(x);
                                context.Ruleset.ApplyRules(context);
                                x = x - Math.Truncate(x);
                            }
                        }
                        break;
                    case "":
                        break;
                    case "=":
                        // Fallback number formating?
                        if (sub.Descriptor.StartsWith("#") || sub.Descriptor.StartsWith("0"))
                        {
                            context.Text.Append(context.Number.ToString(sub.Descriptor, CultureInfo.InvariantCulture));
                        }

                        // Else goto the ruleset.
                        else
                        {
                            var ruleset = context.RulesetGroup.Rulesets[sub.Descriptor];
                            ruleset.ApplyRules(context);
                        }
                        break;
                    default:
                        throw new NotSupportedException($"Substitution token '{sub.Token}' is not allowed.");
                }
            }
            context.Number = number;

        }

        public override bool Matches(RbnfContext context)
        {
            return Math.Truncate(context.Number) != context.Number;
        }
    }
}
