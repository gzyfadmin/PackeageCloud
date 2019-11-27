using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Sales
{
    /// <summary>
    /// 销售订单金额统计模型
    /// </summary>
    public class SalesAmountCountModel
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
}
