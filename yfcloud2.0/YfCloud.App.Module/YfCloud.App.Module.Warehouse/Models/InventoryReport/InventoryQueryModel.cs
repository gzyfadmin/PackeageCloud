using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.InventoryReport
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryQueryModel
    {
        /// <summary>
        /// 物料代码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public int? WarehouseId { get; set; }
    }
}
