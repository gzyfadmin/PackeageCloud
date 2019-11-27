using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Warehouse.Models.TWMSalesMain
{
    public class TWMSalesMainAduit
    {
        /// <summary>
        /// 出库单ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过
        /// </summary>
        public byte AuditStatus { get; set; }
    }
}
