using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 供应商采购金额分析模型
    /// </summary>
    public class PurchaseSupplierAmountCountModel
    {
        /// <summary>
        /// 采购金额
        /// </summary>
        public decimal? Value { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }
    }
}
