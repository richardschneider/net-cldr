using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sepia.Globalization.Numbers.Rules
{
    class NanRule : Rule
    {
        public override void Fire(RbnfContext context)
        {
            foreach (var sub in Substitutions)
            {
                context.Text.Append(sub.Text);
            }
        }

        public override bool Matches(RbnfContext context)
        {
            return context.DoubleNumber.HasValue && double.IsNaN(context.DoubleNumber.Value);
        }
    }
}
