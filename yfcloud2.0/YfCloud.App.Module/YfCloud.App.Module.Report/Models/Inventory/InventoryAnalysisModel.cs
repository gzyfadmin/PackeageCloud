using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Inventory
{
    /// <summary>
    /// 库存量分析
    /// </summary>
    public class InventoryAnalysisModel
    {
        /// <summary>
        /// 库存量
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 包型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 包型名称+库存量
        /// </summary>
        public string NameValue
        {
            get
            {
                return Name + "：" + Value;
            }
        }
    }
    public class InventoryAnalysisModel1
    {
        public string DicValue { get; set; }
        public decimal inAmount { get; set; }
        public decimal outAmount { get; set; }
    }
}
