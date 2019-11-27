using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Warehouse
{
    /// <summary>
    /// 总入库量，出库量，库存分析
    /// </summary>
    public class InventoryAmountCountModel
    {
        /// <summary>
        /// 入库量
        /// </summary>
        public decimal? InAmountCount { get; set; }
        /// <summary>
        /// 出库量
        /// </summary>
        public decimal? OutAmountCount { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public decimal? Inventory { get; set; }

    }
}
