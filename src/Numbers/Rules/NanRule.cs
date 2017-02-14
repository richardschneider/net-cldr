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
        }

        public override bool Matches(RbnfContext context)
        {
            return false;
        }
    }
}
