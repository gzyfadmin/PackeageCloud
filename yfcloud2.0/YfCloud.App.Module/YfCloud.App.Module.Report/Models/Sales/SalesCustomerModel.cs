using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Sales
{
    /// <summary>
    /// 销售客户信息
    /// </summary>
    public class SalesCustomerModel
    {
        /// <summary>
        /// 客户 ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
    }
}
