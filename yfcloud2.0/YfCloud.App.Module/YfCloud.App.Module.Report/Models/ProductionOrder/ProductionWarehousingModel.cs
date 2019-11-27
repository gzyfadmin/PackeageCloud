using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.ProductionOrder
{
    /// <summary>
    /// 生产入库态势模型
    /// </summary>
    public class ProductionWarehousingModel
    {
        /// <summary>
        /// xAxis Data
        /// </summary>
        public string[] xAxisData { get; set; }

        /// <summary>
        /// Series Data
        /// </summary>
        public string[] SeriesData { get; set; }
    }
    public class ProductionWarehousingModel1
    {
        /// <summary>
        /// xAxis Data
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Series Data
        /// </summary>
        public decimal value { get; set; }
    }
}
