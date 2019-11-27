using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.ProductionOrder
{
    /// <summary>
    /// 生产客户模型
    /// </summary>
    public class ProductionOrderCustomerModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }
    }
}
