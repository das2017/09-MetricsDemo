using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Metrics;

namespace MetricsDemo
{
    public class MetricsKey
    {
        public static Meter OrderCount
        {
            //建议MetricsKeyName命名规范：Solution名或应用名+MetricsName
            get { return MetricsHelper.Meter("MetricsDemo.OrderCount", Unit.Custom("单")); }
        }
        public static Meter OrderMoneyCount
        {
            get { return MetricsHelper.Meter("MetricsDemo.OrderMoneyCount", Unit.Custom("元")); }
        }
        public static Meter OrderErrorCount
        {
            get { return MetricsHelper.Meter("MetricsDemo.OrderErrorCount", Unit.Custom("单")); }
        }
    }
}
