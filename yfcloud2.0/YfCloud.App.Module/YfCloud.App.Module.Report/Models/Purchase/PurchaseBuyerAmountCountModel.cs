using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Purchase
{
    /// <summary>
    /// 采购员采购业绩模型
    /// </summary>
    public class PurchaseBuyerAmountCountModel
    {
        /// <summary>
        /// 采购金额
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 采购信息
        /// </summary>
        public string Name
        {
            get
            {
                return "姓名：" + UserName + "  订单数量：" + OrderCount + "  采购金额：" + Value;
            }
        }
    }
}
