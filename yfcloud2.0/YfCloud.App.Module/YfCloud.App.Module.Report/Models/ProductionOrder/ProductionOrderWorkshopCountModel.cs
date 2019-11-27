using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Report.Models.ProductionOrder
{
    /// <summary>
    /// 生产车间入库数量模型
    /// </summary>
    public class ProductionOrderWorkshopCountModel
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 车间名称
        /// </summary>
        public string Name { get; set; }


        public decimal NumberCount { get; set; }
        public decimal Percentage
        {
            get
            {
                return Convert.ToDecimal(string.Format("{0:N2}", (Value / NumberCount * 100)));
            }
        }
        /// <summary>
        /// 车间名称+数量
        /// </summary>
        public string NameValue
        {
            get
            {
                return Name + "：" + Value;
            }
        }
    }
    public class ProductionOrderWorkshopCountModel1
    {
        /// <summary>
        /// 生产单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 车间ID
        /// </summary>
        public int WorkshopId { get; set; }
        /// <summary>
        /// 车间Name
        /// </summary>
        public string WorkshopName { get; set; }
    }
    public class ProductionOrderWorkshopCountModel2
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 生产单ID
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Number { get; set; }
    }
    public class ProductionOrderWorkshopCountModel3
    {
        /// <summary>
        /// 生产单ID
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal Number { get; set; }
    }
}
