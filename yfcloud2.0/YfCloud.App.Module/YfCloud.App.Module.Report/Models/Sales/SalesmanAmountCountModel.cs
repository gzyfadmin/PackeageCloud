using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Sales
{
    /// <summary>
    /// 业务员销售订单金额统计
    /// </summary>
    public class SalesmanAmountCountModel
    {
        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal? Value { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public string Name { get; set; }
    }
}
