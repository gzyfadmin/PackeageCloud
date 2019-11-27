using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 采购业务员模型
    /// </summary>
    public class PurchaseBuyerModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public string Name { get; set; }
    }
}
