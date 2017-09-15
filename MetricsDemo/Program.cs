using System;
using System.Diagnostics;
using System.Threading;

using System.Linq;
using Metrics;
using Metrics.Utils;

namespace MetricsDemo
{
    class Program
    {
        private readonly Histogram searchFlightTime = MetricsHelper.Histogram("MetricsDemo.SearchFlightTime", Unit.Custom("ms"));

        static void Main()
        {
            Program p = new Program();
            while (true)
            {
                p.SearchFlight();
                p.CreateOrder();
            }
        }

        void SearchFlight()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            // 模拟关于航班查询的业务逻辑代码
            Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var n = random.Next(1000);
            Thread.Sleep(n);
            stopwatch.Stop();

            // 统计航班搜索耗时           
            searchFlightTime.Update(stopwatch.ElapsedMilliseconds);
        }

        void CreateOrder()
        {
            try
            {
                // 模拟关于下单的业务逻辑代码
                Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                var n = random.Next(1000);
                Thread.Sleep(n);
                if (n % 7 == 0)
                {
                    throw new ApplicationException();
                }

                // 分别统计成功下单量和下单金额，统一写到MetrisKey中               
                MetricsKey.OrderCount.Mark();
                if (n % 2 == 1)
                {
                    MetricsKey.OrderMoneyCount.Mark("BuyerA", n);
                }
                else
                {
                    MetricsKey.OrderMoneyCount.Mark("BuyerB", n);
                }
            }
            catch (Exception)
            {
                // 统计失败下单量，统一写到MetrisKey中            
                MetricsKey.OrderErrorCount.Mark();

                // 省略异常处理代码......
            }
        }
    }
}
