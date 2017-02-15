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
        static object safe = new Object();
        static RulesetGroup rulesetGroup;

        public override void Resolve()
        {
            if (rulesetGroup == null)
            {
                lock (safe)
                {
                    if (rulesetGroup == null)
                    {
                        var xml = Cldr.Instance
                            .GetDocuments("common/rbnf/root.xml")
                            .FirstElement("ldml/rbnf/rulesetGrouping[@type='NumberingSystemRules']");
                        rulesetGroup = RulesetGroup.Parse(xml);
                    }
                }
            }
        }

        public override string Format(long value)
        {
            return Format((decimal)value);
        }

        public override string Format(decimal value)
        {
            return rulesetGroup.Format(value, (string)NumberingSystem.Rules);
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
