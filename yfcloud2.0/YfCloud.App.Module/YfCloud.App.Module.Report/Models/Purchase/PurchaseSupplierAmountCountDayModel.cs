using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 采购订单金额态势(月) 模型
    /// </summary>
    public class PurchaseSupplierAmountCountDayModel
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
