using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.Warehouse
{
    /// <summary>
    /// 成品库存态势模型
    /// </summary>
    public class InventoryAmountCountDayModel
    {
        /// <summary>
        /// xAxis Data
        /// </summary>
        public string[] xAxisData { get; set; }

        /// <summary>
        /// Series Data
        /// </summary>
        public List<InventoryAmountCountDay> SeriesData { get; set; }
    }
    public class InventoryAmountCountDay
    {
        /// <summary>
        /// xAxis Data
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Series Data
        /// </summary>
        public string[] SeriesData { get; set; }
    }
    public class InventoryAmountCountDayModel1
    {
        public DateTime WarehousingDate { get; set; }
        public string DicValue { get; set; }
        public decimal inAmount { get; set; }
        public decimal outAmount { get; set; }
    }


    public class InventoryAmountCountDayModel2
    {
        public DateTime WarehousingDate { get; set; }
        public string DicValue { get; set; }
        public decimal Amount { get; set; }
    }

}
