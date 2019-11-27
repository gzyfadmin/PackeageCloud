using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Sales
{
    /// <summary>
    /// 款型分布
    /// </summary>
    public class SalesPackageAmountCountModel
    {
        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal? Value { get; set; }
        /// <summary>
        /// 款型
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 款型+数量
        /// </summary>
        public string NameValue
        {
            get
            {
                return Name + ":" + Value;
            }
        }
    }
}
