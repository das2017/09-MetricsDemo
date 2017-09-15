using System;
using System.Configuration;
using System.Diagnostics;

using System.Linq;
using System.Collections.Generic;
using Metrics;
using Metrics.Utils;

namespace MetricsDemo
{
    public class MetricsHelper
    {
        private static readonly Dictionary<string, string> environment = AppEnvironment.Current.ToDictionary(e => e.Name, e => e.Value);
        private static readonly string appID = ConfigurationManager.AppSettings["AppID"];

        /// <summary>
        /// Meter度量
        /// </summary>
        /// <param name="metricsName">一个MetricsName一个数据表，建议明确定义</param>
        /// <param name="unit">单位</param>
        /// <returns>Meter对象</returns>          
        public static Meter Meter(string metricsName, Unit unit)
        {
            return Meter(metricsName, unit, null);
        }

        /// <summary>
        /// Meter度量
        /// </summary>
        /// <param name="metricsName">一个MetricsName一个数据表，建议明确定义</param>
        /// <param name="unit">单位</param>
        /// <param name="tags">tags, Metric级别元数据</param>
        /// <returns>Meter对象</returns>   
        public static Meter Meter(string metricsName, Unit unit, params string[] tags)
        {
            string appIDTag = string.Format("AppID={0}", appID);
            string serverIPTag = string.Format("ServerIP={0}", environment["IPAddress"]);
            string processNameTag = string.Format("ProcessName={0}", environment["ProcessName"]);

            string[] metricsTagList = new[] { appIDTag, serverIPTag, processNameTag };
            if (tags != null && tags.Any())
            {
                metricsTagList = metricsTagList.Concat(tags.ToList()).ToArray();
            }
            MetricTags metricsTags = new MetricTags(metricsTagList);

            return Metric.Meter(metricsName, unit, tags: metricsTags);
        }

        /// <summary>
        /// Histogram度量
        /// </summary>
        /// <param name="metricsName">一个MetricsName一个数据表，建议明确定义</param>
        /// <param name="unit">单位</param>
        /// <param name="tags">tags, Metric级别元数据</param>
        /// <returns>Histogram对象</returns>
        public static Histogram Histogram(string metricsName, Unit unit)
        {
            return Histogram(metricsName, unit, null);
        }

        /// <summary>
        /// Histogram度量
        /// </summary>
        /// <param name="metricsName">一个MetricsName一个数据表，建议明确定义</param>
        /// <param name="unit">单位</param>
        /// <param name="tags">tags, 应用级别</param>
        /// <returns>Histogram对象</returns>   
        public static Histogram Histogram(string metricsName, Unit unit, params string[] tags)
        {
            string appIDTag = string.Format("AppID={0}", appID);
            string serverIPTag = string.Format("ServerIP={0}", environment["IPAddress"]);
            string processNameTag = string.Format("ProcessName={0}", environment["ProcessName"]);

            string[] metricsTagList = new[] { appIDTag, serverIPTag, processNameTag };
            if (tags != null && tags.Any())
            {
                metricsTagList = metricsTagList.Concat(tags.ToList()).ToArray();
            }
            MetricTags metricsTags = new MetricTags(metricsTagList);

            return Metric.Histogram(metricsName, unit, tags: metricsTags);
        }

        static MetricsHelper()
        {
            string uri = ConfigurationManager.AppSettings["Metrics.DBUri"];
            string user = ConfigurationManager.AppSettings["Metrics.UserName"];
            string pass = ConfigurationManager.AppSettings["Metrics.Password"];
            string database = ConfigurationManager.AppSettings["Metrics.Database"];

            Metric.Config                
                .WithReporting(
                    i => i.WithInfluxDb(uri, user, pass, database, TimeSpan.FromSeconds(60))
                  );
        }
    }
}